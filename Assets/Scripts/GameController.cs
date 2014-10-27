using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public Transform Sphere { get; private set; }

	Transform sphereContainer;
	Poser defaultPoser;

	// Use this for initialization
	void Start () {

		Instance = this;

		sphereContainer = GameObject.Find ("/SphereContainer").transform;
		Sphere = GameObject.Find ("/SphereContainer/Sphere").transform;

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		defaultPoser = defaultPuppet.GetComponent<Poser> ();

		defaultPoser.Pose (Pose.RandomPose01 (), 3f);
	}
	
	// Update is called once per frame
	void Update () {
		sphereContainer.Rotate (new Vector3 (0, 0.25f, 0));
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 200, 30), "(un)select")) {
			defaultPoser.Head.Highlighted = !defaultPoser.Head.Highlighted;
			defaultPoser.Chest.Highlighted = !defaultPoser.Chest.Highlighted;
		}
	}

}
