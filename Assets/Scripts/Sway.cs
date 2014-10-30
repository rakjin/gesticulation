using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour {

	Vector3 initialPosition;
	Vector3 lookAt;

	// Use this for initialization
	void Start () {
	
		initialPosition = transform.localPosition;
		lookAt = GameObject.Find ("/LookAt").transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		float x = Mathf.Sin (Time.time * 0.125f) * 0.25f;
		float y = Mathf.Cos (Time.time * 0.0625f) * 0.125f;

		Vector3 swayPosition = new Vector3 (
			initialPosition.x + x,
			initialPosition.y + y,
			initialPosition.z);

		transform.localPosition = swayPosition;

		transform.LookAt (lookAt);
	
	}
}
