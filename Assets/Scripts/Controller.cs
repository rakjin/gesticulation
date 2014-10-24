using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		Poser defaultPoser = defaultPuppet.GetComponent<Poser> ();
		Pose defaultPose = defaultPoser.GetCurrentPose ();
		Debug.Log (defaultPose.RootRotation.eulerAngles);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
