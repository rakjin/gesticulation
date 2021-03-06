﻿using UnityEngine;
using System.Collections;

using Leap;

public class Picker : MonoBehaviour {

	const float PINCH_DISTANCE = 0.5f;

	int scrollThresholdAccumulatedFrames = 0;
	const int SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MIN = 30;
	const int SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MAX = 600;
	const float SCROLL_THRESHOLD_X = 0.15f;

	public Transform PointerPrefab;

	public Vector3 MiddlePosition {
		get {
			return middlePointContainer.transform.localPosition;
		}
	}

	GameController gameController;

	HandModel handModel;
	FingerModel thumb;
	FingerModel index;

	Transform middlePointContainer;
	public Transform MiddlePointContainer {
		get {
			return middlePointContainer;
		}
	}
	SphereCollider middlePoint;

	Collider currentCollider;
	Collider prevCollider;
	Collider pickedCollider; // to store picked collider during pulling state
	bool isPinching = false;
	bool wasPinching = false;

	GameController.PickState currentPickState = GameController.PickState.None;
	GameController.PickState prevPickState = GameController.PickState.None;

	// Use this for initialization
	void Start () {

		gameController = GameController.Instance;

		handModel = GetComponent<HandModel> ();
		thumb = handModel.fingers [0];
		index = handModel.fingers [1];

		GameObject middlePointContainerGO = new GameObject ();
		middlePointContainerGO.AddComponent<MiddlePointContainer> ().SetPicker (this);
		middlePointContainer = middlePointContainerGO.transform;
		middlePointContainer.parent = gameObject.transform;

		middlePointContainer.gameObject.AddComponent<Rigidbody> ();
		middlePointContainer.rigidbody.isKinematic = true;

		middlePoint = middlePointContainer.gameObject.AddComponent<SphereCollider> ();
		middlePoint.radius = 0.001953125f; // / transform.localScale.x;
		middlePoint.isTrigger = true;
	
		Transform pointer = (Transform)Instantiate (PointerPrefab);
		pointer.parent = middlePointContainer;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 thumbPosition = thumb.GetTipPosition () - transform.localPosition;
		Vector3 indexPosition = index.GetTipPosition () - transform.localPosition;
		Vector3 middlePosition = (thumbPosition + indexPosition) * 0.5f;
		Vector3 localScale = transform.localScale;
		middlePosition = new Vector3 (middlePosition.x / localScale.x, middlePosition.y / localScale.y, middlePosition.z / localScale.z); 

		middlePointContainer.transform.localPosition = middlePosition;

		float distance = Vector3.Distance (thumbPosition, indexPosition);
		isPinching = (distance < PINCH_DISTANCE);
		if (isPinching != wasPinching) {
			StateTransition();
		}

		float x = middlePosition.x;
		if (Mathf.Abs(x) > SCROLL_THRESHOLD_X) {
			scrollThresholdAccumulatedFrames += 1;
			if (scrollThresholdAccumulatedFrames > SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MIN) {
				x = (x > 0)? +1 : -1;

				if (scrollThresholdAccumulatedFrames > SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MAX) {
					scrollThresholdAccumulatedFrames = SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MAX;
				}
				float strengthMultiplierUponAccumulatedFrames = ((float)scrollThresholdAccumulatedFrames-SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MIN)/(SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MAX-SCROLL_THRESHOLD_ACCUMULATED_FRAMES_MIN);

				gameController.OnGestureScroll(strength:x*strengthMultiplierUponAccumulatedFrames);
			}
		} else {
			scrollThresholdAccumulatedFrames = 0;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = isPinching ? Color.red : Color.white;
		Gizmos.DrawWireSphere (thumb.GetTipPosition (), 0.125f);
		Gizmos.DrawWireSphere (index.GetTipPosition (), 0.125f);
	}

	public void Reset() {
		currentCollider = null;
		prevCollider = null;
		pickedCollider = null;
		isPinching = false;
		wasPinching = false;

		currentPickState = GameController.PickState.None;
		prevPickState = GameController.PickState.None;
	}

	public void TriggerEnter(Collider other) {
		if (currentCollider != null) {
			return;
		}

		currentCollider = other;
		StateTransition ();
	}
	
	public void TriggerExit(Collider other) {
		if (currentCollider != other) {
			return;
		}

		currentCollider = null;
		StateTransition ();
	}

	void StateTransition() {

		switch(prevPickState) {

		case GameController.PickState.None:

			if (isPinching != wasPinching) { // None to PickingNothing
				Assert (wasPinching == false);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.PickingNothing;

			} else if (currentCollider != prevCollider && currentCollider != null) { // None to Hovering
				prevCollider = currentCollider;
				prevPickState = currentPickState = GameController.PickState.Hovering;
				gameController.OnPickStateChanged(
					GameController.PickState.None,
					GameController.PickState.Hovering,
					this,
					currentCollider.gameObject);

			}

			break;


		case GameController.PickState.Hovering:

			if (isPinching != wasPinching && currentCollider != null) { // Hovering to Picking
				Assert (wasPinching == false);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.Picking;
				pickedCollider = currentCollider;
				gameController.OnPickStateChanged(
					GameController.PickState.Hovering,
					GameController.PickState.Picking,
					this,
					currentCollider.gameObject);

			} else if (currentCollider != prevCollider && prevCollider != null) { // Hovering to None
				prevPickState = currentPickState = GameController.PickState.None;
				gameController.OnPickStateChanged(
					GameController.PickState.Hovering,
					GameController.PickState.None,
					this,
					prevCollider.gameObject);
				prevCollider = currentCollider;
				pickedCollider = null;

			} else {
				Assert (false);

			}

			break;


		case GameController.PickState.Picking:

			if (isPinching != wasPinching && currentCollider != null) { // Picking to Hovering
				Assert (wasPinching == true);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.Hovering;
				gameController.OnPickStateChanged(
					GameController.PickState.Picking,
					GameController.PickState.Hovering,
					this,
					currentCollider.gameObject);
				pickedCollider = null;

			} else if (currentCollider != prevCollider) { // Picking to Pulling
				Assert (prevCollider != null);
				Assert (currentCollider == null);
				Assert (pickedCollider != null);
				prevCollider = currentCollider; //null
				prevPickState = currentPickState = GameController.PickState.Pulling;
				gameController.OnPickStateChanged(
					GameController.PickState.Picking,
					GameController.PickState.Pulling,
					this,
					pickedCollider.gameObject);
			} else {
				Assert (false);

			}

			break;


		case GameController.PickState.Pulling:

			if (isPinching != wasPinching) { // Pulling to None
				Assert (wasPinching == true);
				Assert (isPinching == false);
				Assert (pickedCollider != null);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.None;
				gameController.OnPickStateChanged(
					GameController.PickState.Pulling,
					GameController.PickState.None,
					this,
					pickedCollider.gameObject);
				pickedCollider = null;

			} else if (currentCollider != prevCollider) { // Pulling to Picking
				Assert (isPinching == true);

				if (currentCollider != pickedCollider) {
					currentCollider = null;
					return; // ignore entering other than previously picked collider

				} else {
					Assert (currentCollider == pickedCollider);
					prevCollider = pickedCollider;
					gameController.OnPickStateChanged(
						GameController.PickState.Pulling,
						GameController.PickState.Picking,
						this,
						currentCollider.gameObject);

				}
			}

			break;


		case GameController.PickState.PickingNothing:

			if (isPinching != wasPinching) { // PickNothing to None
				Assert (wasPinching == true);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.None;

			} else if (prevCollider != currentCollider) { // PickNothing to PickNothingColliding
				prevCollider = null;
				currentCollider = null;

			}

			break;


		case GameController.PickState.PickingNothingColliding:

			if (isPinching != wasPinching) { // PickNothingColliding to Hovering
				Assert (wasPinching == true);
				wasPinching = isPinching;
				gameController.OnPickStateChanged(
					GameController.PickState.None,
					GameController.PickState.Hovering,
					this,
					currentCollider.gameObject);

			} else if (prevCollider != currentCollider) { // PickNothingColliding to PickNothing
				Assert (prevCollider != null);
				Assert (currentCollider == null);
				prevCollider = currentCollider; //null

			} else {
				Assert (false);

			}

			break;

		}

	}

	void OnDestroy() {

		if (currentPickState == GameController.PickState.Hovering ||
		    currentPickState == GameController.PickState.Picking ||
		    currentPickState == GameController.PickState.Pulling) {

			gameController.OnPickStateChanged(
				currentPickState,
				GameController.PickState.None,
				this,
				currentCollider == null ? null : currentCollider.gameObject);

		}

	}

	void Assert(bool assertion, string desc = "") {
		if (!assertion) {
			Debug.LogError ("Assertion Failed: " + desc);
		}
	}
}
