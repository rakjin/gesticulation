using UnityEngine;
using System.Collections;

using Leap;

public class Picker : MonoBehaviour {

	const float PINCH_DISTANCE = 0.5f;

	public Vector3 MiddlePosition {
		get {
			return middlePoint.center;
		}
	}

	GameController gameController;

	FingerModel thumb;
	FingerModel index;
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

		HandModel handModel = GetComponent<HandModel> ();
		thumb = handModel.fingers [0];
		index = handModel.fingers [1];


		gameObject.AddComponent<Rigidbody> ();
		rigidbody.isKinematic = true;

		middlePoint = gameObject.AddComponent<SphereCollider> ();
		middlePoint.radius = 0.0625f / transform.localScale.x;
		middlePoint.isTrigger = true;
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 thumbPosition = thumb.GetTipPosition () - transform.localPosition;
		Vector3 indexPosition = index.GetTipPosition () - transform.localPosition;
		Vector3 middlePosition = (thumbPosition + indexPosition) * 0.5f;
		Vector3 localScale = transform.localScale;
		middlePosition = new Vector3 (middlePosition.x / localScale.x, middlePosition.y / localScale.y, middlePosition.z / localScale.z); 

		middlePoint.center = middlePosition;

		float distance = Vector3.Distance (thumbPosition, indexPosition);
		isPinching = (distance < PINCH_DISTANCE);
		if (isPinching != wasPinching) {
			StateTransition();
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = isPinching ? Color.red : Color.white;
		Gizmos.DrawWireSphere (thumb.GetTipPosition (), 0.125f);
		Gizmos.DrawWireSphere (index.GetTipPosition (), 0.125f);
	}

	void OnTriggerEnter(Collider other) {
		if (currentCollider != null) {
			return;
		}

		currentCollider = other;
		StateTransition ();
	}
	
	void OnTriggerExit(Collider other) {
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
			} else {
				Assert (false);

			}

			break;


		case GameController.PickState.PickingNothing:

			if (isPinching != wasPinching) { // PickNothing to None
				Assert (wasPinching == true);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.None;

			} else if (prevCollider != currentCollider) { // PickNothing to PickNothingColliding
				prevCollider = currentCollider;

			} else {
				Assert (false);

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
