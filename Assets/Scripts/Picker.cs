using UnityEngine;
using System.Collections;

using Leap;

public class Picker : MonoBehaviour {

	const float PICK_DISTANCE = 0.25f;

	FingerModel thumb;
	FingerModel index;
	SphereCollider middlePoint;

	Collider currentCollider;
	GameController.PickState pickState = GameController.PickState.None;
	GameController gameController;

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
	
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (thumb.GetTipPosition (), 0.125f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (index.GetTipPosition (), 0.125f);
	}

	void OnTriggerEnter(Collider other) {
		if (currentCollider == null) {
			currentCollider = other;

			pickState = GameController.PickState.Hovering;
			gameController.OnPickStateChanged(
				GameController.PickState.None,
				GameController.PickState.Hovering,
				this,
				other.gameObject);

		}
	}
	
	void OnTriggerExit(Collider other) {
		if (currentCollider == other) {
			currentCollider = null;

			pickState = GameController.PickState.None;
			gameController.OnPickStateChanged(
				GameController.PickState.Hovering,
				GameController.PickState.None,
				this,
				other.gameObject);

		}
	}
}
