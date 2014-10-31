using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public static GameController Instance { get; private set; }

	const string TAG_PART = "Part";


	public Shelf shelf;
	public GUIStyle titleStyle;

	// Use this for initialization
	void Start () {
		Instance = this;

		if (shelf == null) {
			Debug.LogError ("shelf required");
		}
	}
	
	Poser GetDefaultPoser() {
		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		if (defaultPuppet) {
			Poser defaultPoser = defaultPuppet.GetComponent<Poser> ();
			return defaultPoser;
		}
		return null;
	}

	void OnGUI () {

		Poser poser = GetDefaultPoser ();

		float y = -20;

		if (GUI.Button (new Rect(10, y += 30, 200, 30), "Print Pose")) {
			if (poser) {
				Debug.Log (poser.GetCurrentPose());
			}
		} else if (GUI.Button (new Rect(10, y += 30, 200, 30), "RandomPose00")) {
			if (poser) {
				poser.ApplyPose (Pose.RandomPose00());
			}
		} else if (GUI.Button (new Rect(10, y += 30, 200, 30), "RandomPose01")) {
			if (poser) {
				poser.ApplyPose (Pose.RandomPose01());
			}
		} else if (GUI.Button (new Rect(10, y += 30, 200, 30), "RandomPose02")) {
			if (poser) {
				poser.ApplyPose (Pose.RandomPose02());
			}
		} else if (GUI.Button (new Rect(10, y += 30, 200, 30), "RandomPose03")) {
			if (poser) {
				poser.ApplyPose (Pose.RandomPose03());
			}
		}

		if (GUI.Button (new Rect(210, 10, 50, 30), "<<")) {
			shelf.FlipLeft();
		} else if (GUI.Button (new Rect(260, 10, 50, 30), ">>")) {
			shelf.FlipRight();
		}




		float screenWidth = UnityEngine.Screen.width;
		float screenHeight = UnityEngine.Screen.height;
		float unit = screenWidth / 120;

		float padding = unit*2;

		float titleWidth = unit*50;
		float titleHeight = unit*5;

		Rect titleRect = new Rect (padding, screenHeight - titleHeight - padding, titleWidth, titleHeight);
		titleStyle.fontSize = (int)(unit * 4);

		GUI.Label (titleRect, "안녕안녕?", titleStyle);

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

	#region gesture

	bool ignoreGesture = false;
	const float ignoreGestureTimeSpan = 0.75f;

	public void OnGestureSwipe(bool toLeft) {
		if(ignoreGesture == false) {
			ignoreGesture = true;
			shelf.Flip(toLeft);
			StartCoroutine(ResetIgnoreGestureFlag());
		}
	}

	IEnumerator ResetIgnoreGestureFlag() {
		yield return new WaitForSeconds(ignoreGestureTimeSpan);
		ignoreGesture = false;
		yield break;
	}

	#endregion
}
