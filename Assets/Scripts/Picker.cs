using UnityEngine;
using System.Collections;

using Leap;

public class Picker : MonoBehaviour {

	const float PINCH_DISTANCE = 0.25f;

	GameController gameController;

	FingerModel thumb;
	FingerModel index;
	SphereCollider middlePoint;

	Collider currentCollider;
	Collider prevCollider;
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
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (thumb.GetTipPosition (), 0.125f);
		Gizmos.color = Color.red;
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
				gameController.OnPickStateChanged(
					GameController.PickState.None,
					GameController.PickState.PickingNothing,
					this,
					currentCollider.gameObject);

			} else if (currentCollider != prevCollider) { // None to Hovering
				Assert (currentCollider != null);
				prevCollider = currentCollider;
				prevPickState = currentPickState = GameController.PickState.Hovering;
				gameController.OnPickStateChanged(
					GameController.PickState.None,
					GameController.PickState.Hovering,
					this,
					currentCollider.gameObject);

			} else {
				Assert (false);

			}

			break;


		case GameController.PickState.Hovering:

			if (isPinching != wasPinching) { // Hovering to Picking
				Assert (wasPinching == false);
				wasPinching = isPinching;
				prevPickState = currentPickState = GameController.PickState.Picking;
				gameController.OnPickStateChanged(
					GameController.PickState.Hovering,
					GameController.PickState.Picking,
					this,
					currentCollider.gameObject);

			} else if (currentCollider != prevCollider) { // Hovering to None
				Assert (prevCollider != null);
				Assert (currentCollider == null);
				prevPickState = currentPickState = GameController.PickState.None;
				gameController.OnPickStateChanged(
					GameController.PickState.Hovering,
					GameController.PickState.None,
					this,
					prevCollider.gameObject);
				prevCollider = currentCollider;

			} else {
				Assert (false);

			}

			break;

		}

	}

	void Assert(bool assertion, string desc = "") {
		if (!assertion) {
			Debug.LogError ("Assertion Failed: " + desc);
		}
	}
}
