using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public enum State {
		Show,
		Edit,
	}

	public static GameController Instance { get; private set; }

	const string TAG_PART = "Part";
	const string TAG_BUTTON = "Button3D";


	public Transform buttonContainer;
	public Transform button3D;
	public Shelf shelf;
	public GUIStyle titleStyle;
	public GUIStyle authorStyle;



	State state = State.Show;

	string displayingTitle = "";
	string displayingAuthor = "";
	Button3D editButton;
	readonly Vector3 editButtonEnablePositon = new Vector3 (0, 2.3f, -1);
	readonly Vector3 editButtonEnableScale = new Vector3 (.5625f, .5625f, .5625f);
	readonly Vector3 editButtonDisablePositon = new Vector3 (0, -.5f, 0);
	readonly Vector3 editButtonDisableScale = Vector3.zero;

	// Use this for initialization
	void Start () {
		Instance = this;

		if (shelf == null) {
			Debug.LogError ("shelf required");
		}

		Preset preset = shelf.CurrentPreset();
		displayingTitle = preset.Title;
		displayingAuthor = preset.Author;

		Transform editButtonTransform = (Transform)Instantiate(button3D, editButtonDisablePositon, Quaternion.identity);
		editButtonTransform.localScale = editButtonDisableScale;
		editButtonTransform.parent = buttonContainer;
		editButton = editButtonTransform.GetComponent<Button3D>();
		editButton.Text = "편집";
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

		float screenWidth = UnityEngine.Screen.width;
		float screenHeight = UnityEngine.Screen.height;
		float unit = screenWidth / 120;

		float padding = unit*2;
		float gap = unit;


		if (state == State.Show) {
			float authorWidth = unit*60;
			float authorHeight = unit*3;
			Rect authorRect = new Rect (padding, screenHeight - authorHeight - padding, authorWidth, authorHeight);
			authorStyle.fontSize = (int)(unit * 3);

			float titleWidth = unit*60;
			float titleHeight = unit*5;
			Rect titleRect = new Rect (padding, screenHeight - authorHeight - gap - titleHeight - padding, titleWidth, titleHeight);
			titleStyle.fontSize = (int)(unit * 4);

			GUI.Label (titleRect, displayingTitle, titleStyle);
			GUI.Label (authorRect, displayingAuthor, authorStyle);
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

			} else if (target && target.tag.Equals(TAG_BUTTON)) {
				Highlightable button = target.GetComponentInChildren<Highlightable>();
				button.Highlighted = Highlightable.HighlightDegree.None;
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

			} else if (target && target.tag.Equals(TAG_BUTTON)) {
				Highlightable button = target.GetComponentInChildren<Highlightable>();
				button.Highlighted = Highlightable.HighlightDegree.Half;
			}
			
			break;
			

		case PickState.Picking:

			if (target && target.tag.Equals(TAG_PART)) {
				Part part = target.GetComponent<Part>();
				part.Highlighted = Part.HighlightDegree.Full;
				part.ConnectToRigidbody(picker.MiddlePointContainer.rigidbody, Vector3.zero);

			} else if (target && target.tag.Equals(TAG_BUTTON) && state == State.Show) {
				Highlightable button = target.GetComponentInChildren<Highlightable>();
				button.Highlighted = Highlightable.HighlightDegree.Full;

				OnEditButtonPicked();
			}

			break;
		}
	}

	#region gesture

	bool ignoreGesture = false;
	const float ignoreGestureTimeSpan = 0.5625f;

	public void OnGestureSwipe(bool toLeft) {
		if(ignoreGesture == false && state == State.Show) {
			ignoreGesture = true;
			shelf.Flip(toLeft);
			StartCoroutine(ResetIgnoreGestureFlag());

			Preset preset = shelf.CurrentPreset();
			StartCoroutine(FadeTitleAuthor(preset.Title, preset.Author));

			if (preset.Type == Preset.PresetType.NewPresetPlaceHolder) {
				LeanTween.moveLocal(
					editButton.gameObject,
					editButtonEnablePositon,
					0.5625f)
					.setEase (LeanTweenType.easeInOutCubic);
				LeanTween.scale(
					editButton.gameObject,
					editButtonEnableScale,
					0.5625f)
					.setEase (LeanTweenType.easeInOutCubic);

			} else {
				LeanTween.moveLocal(
					editButton.gameObject,
					editButtonDisablePositon,
					0.125f)
					.setEase (LeanTweenType.easeInOutCubic);
				LeanTween.scale(
					editButton.gameObject,
					editButtonDisableScale,
					0.125f)
					.setEase (LeanTweenType.easeInOutCubic);
				
			}
		}
	}

	IEnumerator ResetIgnoreGestureFlag() {
		yield return new WaitForSeconds(ignoreGestureTimeSpan);
		ignoreGesture = false;
		yield break;
	}

	#endregion


	#region Fade

	IEnumerator FadeTitleAuthor(string title, string author) {

		int frames = 10;
		Color titleTextColor = titleStyle.normal.textColor;
		Color authorTextColor = authorStyle.normal.textColor;

		for (int i = 0; i <= frames; i++) {
			float alpha = (frames-i)/(float)frames;
			titleStyle.normal.textColor = new Color(titleTextColor.r, titleTextColor.g, titleTextColor.b, alpha);
			authorStyle.normal.textColor = new Color(authorTextColor.r, authorTextColor.g, authorTextColor.b, alpha);
			yield return new WaitForSeconds(1f/60);
		}

		displayingTitle = title;
		displayingAuthor = author;

		for (int i = 0; i <= frames; i++) {
			float alpha = i/(float)frames;
			titleStyle.normal.textColor = new Color(titleTextColor.r, titleTextColor.g, titleTextColor.b, alpha);
			authorStyle.normal.textColor = new Color(authorTextColor.r, authorTextColor.g, authorTextColor.b, alpha);
			yield return new WaitForSeconds(1f/60);
		}

		yield break;
	}

	#endregion


	#region EditButton

	void OnEditButtonPicked() {

		state = State.Edit;

		LeanTween.scale (
			editButton.gameObject,
			Vector3.one,
			0.25f)
			.setEase (LeanTweenType.easeInOutCubic);

		LeanTween.scale (
			editButton.gameObject,
			Vector3.zero,
			0.125f)
			.setEase (LeanTweenType.easeInOutCubic)
			.setDelay (0.25f);

		Poser poser = shelf.CurrentPoser();
		poser.EditEnabled = true;
		poser.Highlighted = Highlightable.HighlightDegree.None;
	}

	#endregion
}
