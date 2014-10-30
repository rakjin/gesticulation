using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public static GameController Instance;

	const string TAG_PART = "Part";

	Poser defaultPoser;

	// Use this for initialization
	void Start () {

		Instance = this;

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		if (defaultPuppet) {
			defaultPoser = defaultPuppet.GetComponent<Poser> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 200, 30), "(un)select")) {
			if (defaultPoser) {
				defaultPoser.Pose (Pose.RandomPose01());
			}
		}
	}

	public enum PickState {
		None,
		Hovering,
		Picking,
		Pulling,
		PickingNothing,
		PickingNothingColliding,
	}
	public void OnPickStateChanged(PickState prevState, PickState currentState, Picker picker, GameObject target) {
		switch (currentState) {

		case PickState.None:
			
			if (target && target.tag.Equals(TAG_PART)) {
				Part part = target.GetComponent<Part>();
				part.Highlighted = Part.HighlightDegree.None;
				part.DisconnectFromRigidbody();
				picker.Reset();
			}
			
			break;


		case PickState.Hovering:
			
			if (target && target.tag.Equals(TAG_PART)) {
				Part part = target.GetComponent<Part>();
				part.Highlighted = Part.HighlightDegree.Half;
				part.DisconnectFromRigidbody();
				if (prevState == PickState.Picking) {
					part.Highlighted = Part.HighlightDegree.None;
					picker.Reset();
				}
			}
			
			break;
			

		case PickState.Picking:

			if (target && target.tag.Equals(TAG_PART)) {
				Part part = target.GetComponent<Part>();
				part.Highlighted = Part.HighlightDegree.Full;
				part.ConnectToRigidbody(picker.MiddlePointContainer.rigidbody, Vector3.zero);
			}

			break;
		}
	}

}
