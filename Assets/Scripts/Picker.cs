using UnityEngine;
using System.Collections;

using Leap;

public class Picker : MonoBehaviour {

	FingerModel thumb;
	FingerModel index;
	SphereCollider middlePoint;

	// Use this for initialization
	void Start () {

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
	
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (thumb.GetTipPosition (), 0.125f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (index.GetTipPosition (), 0.125f);
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.tag);
	}
	
	void OnTriggerExit(Collider other) {
		Debug.Log (other.tag);
	}
	
	void OnDestroy() {
		Debug.Log ("I'm Dying!");
	}
}
