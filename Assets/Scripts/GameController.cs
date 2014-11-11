using UnityEngine;
using System.Collections;

using Leap;

public class GameController : MonoBehaviour {

	public enum State {
		Show,
		Edit,
		TypeTextInfo,
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

	readonly Vector3 buttonDisablePosition = new Vector3 (0, -.5f, 0);
	readonly Vector3 buttonDisableScale = new Vector3 (0.00048828125f, 0.00048828125f, 0.00048828125f);
	readonly Vector3 buttonSwollenScale = Vector3.one;

	Button3D editButton;
	readonly Vector3 editButtonEnablePosition = new Vector3 (0, 2.3f, -1);
	readonly Vector3 editButtonEnableScale = new Vector3 (.5625f, .5625f, .5625f);
	Button3D saveEditingButton;
	readonly Vector3 saveEditingButtonEnablePosition = new Vector3 (2f, 2.7f, -1);
	readonly Vector3 saveEditingButtonEnableScale = new Vector3 (.5f, .5f, .5f);
	Button3D cancelEditingButton;
	readonly Vector3 cancelEditingButtonEnablePosition = new Vector3 (2f, 2.2f, -1);
	readonly Vector3 cancelEditingButtonEnableScale = new Vector3 (.5f, .5f, .5f);
	Button3D doneEditingButton;
	readonly Vector3 doneEditingButtonEnablePosition = new Vector3 (2f, 2.2f, -1);
	readonly Vector3 doneEditingButtonEnableScale = new Vector3 (.5f, .5f, .5f);

	bool showDebugUI = false;

	// Use this for initialization
	void Start () {
		Instance = this;

		if (shelf == null) {
			Debug.LogError ("shelf required");
		}

		Preset preset = shelf.CurrentPreset();
		displayingTitle = preset.Title;
		displayingAuthor = preset.Author;

		Setup3DGUI ();
	}
	
	Poser GetDefaultPoser() {
		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		if (defaultPuppet) {
			Poser defaultPoser = defaultPuppet.GetComponent<Poser> ();
			return defaultPoser;
		}
		return null;
	}

	void Setup3DGUI() {
		editButton = Setup3DButton ("편집", editButtonEnablePosition, editButtonEnableScale);
		cancelEditingButton = Setup3DButton ("취소", cancelEditingButtonEnablePosition, cancelEditingButtonEnableScale);
		saveEditingButton = Setup3DButton ("저장", saveEditingButtonEnablePosition, saveEditingButtonEnableScale);
		doneEditingButton = Setup3DButton ("완료", doneEditingButtonEnablePosition, doneEditingButtonEnableScale);
	}

	Button3D Setup3DButton(string label, Vector3 enablePosition, Vector3 enableScale) {
		Transform buttonTransform = (Transform)Instantiate(button3D, buttonDisablePosition, Quaternion.identity);
		buttonTransform.parent = buttonContainer;
		buttonTransform.localScale = buttonDisableScale;
		buttonTransform.localPosition = buttonDisablePosition;
		Button3D button = buttonTransform.GetComponent<Button3D>();
		button.EnablePosition = enablePosition;
		button.EnableScale = enableScale;
		button.DisablePosition = enablePosition;
		button.DisableScale = buttonDisableScale;
		button.SwollenScale = buttonSwollenScale;
		button.Text = label;
		button.enabled = false;
		return button;
	}


	void OnGUI () {

		float screenWidth = UnityEngine.Screen.width;
		float screenHeight = UnityEngine.Screen.height;
		float unit = screenWidth / 120;

		float padding = unit*2;
		float gap = unit;


		if (state == State.Show || state == State.TypeTextInfo) {
			float authorWidth = unit*60;
			float authorHeight = unit*3;
			Rect authorRect = new Rect (padding, screenHeight - authorHeight - padding, authorWidth, authorHeight);
			authorStyle.fontSize = (int)(unit * 3);

			float titleWidth = unit*60;
			float titleHeight = unit*5;
			Rect titleRect = new Rect (padding, screenHeight - authorHeight - gap - titleHeight - padding, titleWidth, titleHeight);
			titleStyle.fontSize = (int)(unit * 4);

			if (state == State.Show) {
				GUI.Label (titleRect, displayingTitle, titleStyle);
				GUI.Label (authorRect, displayingAuthor, authorStyle);

			} else if (state == State.TypeTextInfo) {
				GUI.SetNextControlName("title");
				displayingTitle = GUI.TextField (titleRect, displayingTitle, titleStyle);
				GUI.SetNextControlName("author");
				displayingAuthor = GUI.TextField (authorRect, displayingAuthor, authorStyle);
				if (GUI.GetNameOfFocusedControl() == string.Empty) {
					GUI.FocusControl("title");
				}
			}
		}



		if (showDebugUI) {
			if (GUI.Button (new Rect(210, 10, 50, 30), "<<")) {
				OnGestureSwipe(toLeft:true);
			} else if (GUI.Button (new Rect(260, 10, 50, 30), ">>")) {
				OnGestureSwipe(toLeft:false);
			}
		}
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.BackQuote)) {
			showDebugUI = !showDebugUI;
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

			} else if (target && target.tag.Equals(TAG_BUTTON)) {
				Highlightable highlightable = target.GetComponentInChildren<Highlightable>();
				highlightable.Highlighted = Highlightable.HighlightDegree.Full;

				Button3D button = target.transform.parent.GetComponent<Button3D>();
				if (button == editButton) {
					OnEditButtonPicked();
				
				} else if (button == saveEditingButton) {
					OnSaveButtonPicked();

				} else if (button == cancelEditingButton) {
					OnCancelButtonPicked();

				} else if (button == doneEditingButton) {
					OnDoneButtonPicked();

				}

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
				editButton.enabled = true;

			} else {
				editButton.enabled = false;
				
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

	IEnumerator FadeInTitleAuthorTextField() {

		displayingTitle = "TITLE";
		displayingAuthor = "NAME";

		int frames = 10;
		Color titleTextColor = titleStyle.normal.textColor;
		Color authorTextColor = authorStyle.normal.textColor;
		
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

		if (state != State.Show) {
			return;
		}

		state = State.Edit;

		editButton.SwellAndDisable ();
		cancelEditingButton.enabled = true;
		saveEditingButton.enabled = true;

		Poser poser = shelf.CurrentPoser();
		poser.EditEnabled = true;
		poser.Highlighted = Highlightable.HighlightDegree.None;
	}

	#endregion


	#region CancelEditingButton

	void OnCancelButtonPicked() {

		if (state != State.Edit) {
			return;
		}

		state = State.Show;

		cancelEditingButton.SwellAndDisable ();
		editButton.enabled = true;
		saveEditingButton.enabled = false;

		Poser poser = shelf.CurrentPoser ();
		poser.ApplyPose (Pose.DefaultPose (), 1);
		poser.Highlighted = Highlightable.HighlightDegree.Pale;
		poser.EditEnabled = false;
	}

	#endregion


	#region SaveEditingButton

	void OnSaveButtonPicked() {

		if (state != State.Edit) {
			return;
		}

		state = State.TypeTextInfo;

		saveEditingButton.SwellAndDisable ();
		cancelEditingButton.enabled = false;
		doneEditingButton.enabled = true;

		StartCoroutine(FadeInTitleAuthorTextField());
	}

	#endregion


	#region DoneButton

	void OnDoneButtonPicked() {

		if (state != State.TypeTextInfo) {
			return;
		}

		state = State.Show;

		doneEditingButton.enabled = false;

		//
	}

	#endregion

}
