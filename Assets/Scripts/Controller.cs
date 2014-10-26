using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	Poser defaultPoser;

	// Use this for initialization
	void Start () {

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		defaultPoser = defaultPuppet.GetComponent<Poser> ();

		defaultPoser.Pose (Pose.RandomPose01 (), 3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 200, 30), "(un)select")) {
			defaultPoser.Head.Highlighted = !defaultPoser.Head.Highlighted;
			defaultPoser.Chest.Highlighted = !defaultPoser.Chest.Highlighted;
		}
	}

}
