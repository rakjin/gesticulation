using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	Poser defaultPoser;

	// Use this for initialization
	void Start () {

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		defaultPoser = defaultPuppet.GetComponent<Poser> ();
		Pose defaultPose = defaultPoser.GetCurrentPose ();
		Debug.Log (defaultPose);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 200, 30), "print pose")) {
			Debug.Log (defaultPoser.GetCurrentPose());
		}
	}

}
