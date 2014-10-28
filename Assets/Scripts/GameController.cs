using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	public Transform Sphere { get; private set; }

	Poser defaultPoser;

	// Use this for initialization
	void Start () {

		Instance = this;

		Sphere = GameObject.Find ("/Sphere").transform;

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		defaultPoser = defaultPuppet.GetComponent<Poser> ();
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

	public void OnHoverEnter(Picker picker, GameObject target) {
		Debug.Log ("OnHoverEnter: " + target.name);
	}

	public void OnHoverExit(Picker picker, GameObject target) {
		Debug.Log ("OnHoverExit: " + target.name);
	}

	public void OnPick(Picker picker, GameObject target) {
		Debug.Log ("OnPick: " + target.name);
	}

	public void OnUnpick(Picker picker, GameObject target) {
		Debug.Log ("OnUnpick: " + target.name);
	}

}
