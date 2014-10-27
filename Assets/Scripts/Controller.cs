using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public static Controller Instance;

	public Transform FixedSphere { get; private set; }

	Poser defaultPoser;

	// Use this for initialization
	void Start () {

		Instance = this;

		FixedSphere = GameObject.Find ("/FixedSphere").transform;

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
