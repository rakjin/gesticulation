using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour {

	const float SPEED = 4;

	Vector3 initialPosition;
	Transform lookAtTransform;
	Vector3 lookAt;

	// Use this for initialization
	void Start () {
	
		initialPosition = transform.localPosition;
		lookAtTransform = GameObject.Find ("/LookAt").transform;
		lookAt = lookAtTransform.position;
	}
	
	// Update is called once per frame
	void Update () {

		float x = Mathf.Sin (Time.time * 0.125f * SPEED) * 0.25f;
		float y = Mathf.Cos (Time.time * 0.0625f * SPEED) * 0.125f;

		Vector3 swayPosition = new Vector3 (
			initialPosition.x + x,
			initialPosition.y + y,
			initialPosition.z);

		transform.localPosition = swayPosition;

		lookAt = lookAtTransform.position;
		transform.LookAt (lookAt);
	
	}
}
