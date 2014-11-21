using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Leap;

public class GameController : MonoBehaviour {

	public enum State {
		Splash,
		Show,
		Edit,
		TypeTextInfo,
	}

	public static GameController Instance { get; private set; }

	const string TAG_PART = "Part";
	const string TAG_BUTTON = "Button3D";


	public Transform ground;
	public Transform buttonContainer;
	public Transform button3D;
	public Shelf shelf;
	public GUIStyle titleStyle;
	public GUIStyle authorStyle;
	public Texture2D texSplash;
	public Texture2D texTextFieldBorder;
	float splashAlpha = 1;
	public Texture2D texVignette;
	public Texture2D texRec;
	float vignetteAlpha = 0;
	Texture2D texEmpty;


	State state = State.Splash;

	string displayingTitle = "";
	string displayingAuthor = "";
	bool needsSetFocusToTitleTextField = true;
	const string TITLE_PLACEHOLDER = "키보드로 제목을 입력해주세요.";
	const string AUTHOR_PLACEHOLDER = "이름을 입력해주세요. (ENTER)";

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
	readonly Vector3 doneEditingButtonEnablePosition = new Vector3 (2f, 1.7f, -1);
	readonly Vector3 doneEditingButtonEnableScale = new Vector3 (.5f, .5f, .5f);

	bool showDebugUI = false;

	const float recordDuration = 17;
	const float recordFPS = 2;
	const float recordInterval = 1f / recordFPS;
	const int recordCount = (int) (recordDuration * recordFPS);
	List<Pose> records;
	bool isRecording = false;
	float recordBeginTime;

	bool isPlaying = false;
	float playBeginTime;
	const float PLAYBACK_SPEED = 2;


	const float GROUND_SHIFT = 2;


	const float helpAlphaInc = 0.125f;
	const float helpAlphaDec = helpAlphaInc / -2;
	float helpAlpha = 0;


	// Use this for initialization
	IEnumerator Start () {
		Instance = this;

		if (shelf == null) {
			Debug.LogError ("shelf required");
		}
		shelf.OnFlipComplete += OnShelfFlipComplete;

		texEmpty = new Texture2D (1, 1);
		texEmpty.anisoLevel = 0;
		texEmpty.filterMode = FilterMode.Point;
		texEmpty.SetPixel (0, 0, Color.white);

		Preset preset = shelf.CurrentPreset();
		displayingTitle = preset.Title;
		displayingAuthor = preset.Author;

		Setup3DGUI ();

		yield return new WaitForSeconds (0.00001f);

		for (float alpha = 1; alpha >= 0; alpha-= 0.0078125f) {
			splashAlpha = alpha;
			yield return null;
		}

		state = State.Show;
	}

	void OnShelfFlipComplete () {
		BeginPlayCenterSlotIfAnimated ();
	}

	void BeginPlayCenterSlotIfAnimated ()
	{
		Preset centerPreset = shelf.CurrentPreset ();
		if (centerPreset.Type == Preset.PresetType.Animated) {
			Poser centerPoser = shelf.CurrentPoser();
			List<Pose> motion = centerPreset.Motion;
			StartCoroutine(centerPoser.BeginMotion(motion, recordInterval / PLAYBACK_SPEED));
			isPlaying = true;
			playBeginTime = Time.time;
		}
	}

	void Setup3DGUI() {
		editButton = Setup3DButton ("새로 만들기", editButtonEnablePosition, editButtonEnableScale);
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
		float gap = unit/2;


		if (state == State.Show || state == State.TypeTextInfo) {
			float authorWidth = unit*80;
			float authorHeight = unit*4;
			Rect authorRect = new Rect (padding, screenHeight - authorHeight - padding, authorWidth, authorHeight);
			authorStyle.fontSize = (int)(unit * 3);
			authorStyle.padding.top = (int)authorHeight/16;

			float titleWidth = unit*80;
			float titleHeight = unit*6;
			Rect titleRect = new Rect (padding, screenHeight - authorHeight - gap - titleHeight - padding, titleWidth, titleHeight);
			titleStyle.fontSize = (int)(unit * 4);
			titleStyle.padding.top = (int)titleHeight/16;

			if (state == State.Show) {
				GUI.Label (titleRect, displayingTitle, titleStyle);
				GUI.Label (authorRect, displayingAuthor, authorStyle);

				if (shelf.CurrentPreset ().Type == Preset.PresetType.Animated && isPlaying) {
					float progress = Mathf.Clamp01( (Time.time - playBeginTime)/(shelf.CurrentPreset().Motion.Count*recordInterval/PLAYBACK_SPEED) );
					if (playBeginTime == 0) {
						progress = 0;
					}
					DrawProgressBar(new Vector2(screenWidth, screenHeight), unit, titleStyle.normal.textColor.a, progress);
				}
				
			} else if (state == State.TypeTextInfo) {
				GUI.SetNextControlName("title");
				displayingTitle = GUI.TextField (titleRect, displayingTitle, titleStyle);
				GUI.SetNextControlName("author");
				displayingAuthor = GUI.TextField (authorRect, displayingAuthor, authorStyle);
				if (needsSetFocusToTitleTextField || GUI.GetNameOfFocusedControl() == string.Empty) {
					GUI.FocusControl("title");
					needsSetFocusToTitleTextField = false;
				}

				Event e = Event.current;
				if (e.isKey && e.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl().Equals("title")) {
					GUI.FocusControl("author");

				} else if (e.isKey && e.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl().Equals("author")) {
					OnDoneButtonPicked();

				}
			}

		} else if (state == State.Splash) {
			float splashWidth = screenWidth;
			float splashHeight = screenHeight;
			float splashX = (screenWidth-splashWidth)/2;
			float splashY = (screenHeight-splashHeight)/2;
			GUI.color = new Color(1, 1, 1, splashAlpha);
			GUI.DrawTexture(new Rect(splashX, splashY, splashWidth, splashHeight), texSplash, ScaleMode.ScaleToFit);

		} else if (state == State.Edit) {
			float vignetteWidth = screenWidth;
			float vignetteHeight = screenHeight;
			float vignetteX = (screenWidth-vignetteWidth)/2;
			float vignetteY = (screenHeight-vignetteHeight)/2;
			GUI.color = new Color(1, 1, 1, vignetteAlpha);
			GUI.DrawTexture(new Rect(vignetteX, vignetteY, vignetteWidth, vignetteHeight), texVignette, ScaleMode.ScaleToFit);

			float progress = Mathf.Clamp01( (Time.time - recordBeginTime)/recordDuration );
			if (recordBeginTime == 0) {
				progress = 0;
			}
			DrawProgressBar(new Vector2(screenWidth, screenHeight), unit, vignetteAlpha, progress);
			
			float recWidth = unit*16;
			float recHeight = unit*8;
			float recX = unit*4;
			float recY = unit*4;
			if (isRecording) {
				bool showFlickeringRec = ((Time.frameCount % 200) > 100);
				GUI.color = showFlickeringRec? Color.white : new Color(1, 1, 1, 0.25f);
				GUI.DrawTexture(new Rect(recX, recY, recWidth, recHeight), texRec, ScaleMode.ScaleToFit);
			}

		}


		if (helpAlpha > 0) {
			Debug.Log (helpAlpha);
		} else {
			Debug.Log ("");
		}


		if (showDebugUI) {
			GUI.color = Color.white;
			if (GUI.Button (new Rect(0, 10, 200, 30), "print pose")) {
				Debug.Log (shelf.CurrentPoser().GetCurrentPose());
			} else if (GUI.Button (new Rect(210, 10, 50, 30), "<<")) {
				OnGestureSwipe(toLeft:true);
			} else if (GUI.Button (new Rect(260, 10, 50, 30), ">>")) {
				OnGestureSwipe(toLeft:false);
			}
		}
	}

	void DrawProgressBar(Vector2 screenSize, float unit, float baseAlpha, float progress) {
		GUI.color = new Color(1, 1, 1, baseAlpha*0.5f);
		{
			float barBackgroundWidth = unit*90;
			float barBackgroundHeight = unit*3;
			float barBackgroundX = (screenSize.x-barBackgroundWidth)/2;
			float barBackgroundY = screenSize.y - barBackgroundHeight - unit*1.5f;
			GUI.DrawTexture(new Rect(barBackgroundX, barBackgroundY, barBackgroundWidth, barBackgroundHeight), texEmpty, ScaleMode.StretchToFill);
			
			float barWidth = barBackgroundWidth - unit;
			barWidth *= progress;
			float barHeight = barBackgroundHeight - unit;
			float barX = barBackgroundX + unit/2;
			float barY = barBackgroundY + unit/2;
			GUI.DrawTexture(new Rect(barX, barY, barWidth, barHeight), texEmpty, ScaleMode.StretchToFill);
		}
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.BackQuote)) {
			showDebugUI = !showDebugUI;
		}

		if (helpAlpha > 0) {
			helpAlpha += helpAlphaDec;
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
				if (picker != null) {
					picker.Reset();
				}

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
					if (picker != null) {
						picker.Reset();
					}
				}

			} else if (target && target.tag.Equals(TAG_BUTTON)) {
				Highlightable button = target.GetComponentInChildren<Highlightable>();
				button.Highlighted = Highlightable.HighlightDegree.Half;
			}
			
			break;
			

		case PickState.Picking:

			if (target && target.tag.Equals(TAG_PART) && picker != null) {
				Part part = target.GetComponent<Part>();
				part.Highlighted = Part.HighlightDegree.Full;
				part.ConnectToRigidbody(picker.MiddlePointContainer.rigidbody, Vector3.zero);

			} else if (target && target.tag.Equals(TAG_BUTTON)) {
				Highlightable highlightable = target.GetComponentInChildren<Highlightable>();
				highlightable.Highlighted = Highlightable.HighlightDegree.Full;

				Button3D button = target.transform.parent.GetComponent<Button3D>();
				if (button == editButton) {
					StartCoroutine(OnEditButtonPicked());
				
				} else if (button == saveEditingButton) {
					StartCoroutine(OnSaveButtonPicked());

				} else if (button == cancelEditingButton) {
					StartCoroutine(OnCancelButtonPicked());

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

			shelf.CurrentPoser().StopMotion();
			isPlaying = false;
			playBeginTime = 0;

			bool flipped = shelf.Flip(toLeft);
			StartCoroutine(ResetIgnoreGestureFlag());

			Preset preset = shelf.CurrentPreset();
			StartCoroutine(FadeTitleAuthor(preset.Title, preset.Author));

			if (preset.Type == Preset.PresetType.NewPresetPlaceHolder) {
				editButton.enabled = true;

			} else {
				editButton.enabled = false;
				
			}

			if (flipped) {
				float groundShift = toLeft? -GROUND_SHIFT : GROUND_SHIFT;
				LeanTween.moveLocalX(ground.gameObject, groundShift, Shelf.FLIP_DURATION)
					.setEase(LeanTweenType.easeInOutSine)
					.setOnComplete(() => {
						Vector3 backToOrigin = new Vector3(0, ground.localPosition.y, ground.localPosition.z);
						ground.localPosition = backToOrigin;
					});
			}
		}
	}

	IEnumerator ResetIgnoreGestureFlag() {
		yield return new WaitForSeconds(ignoreGestureTimeSpan);
		ignoreGesture = false;
		yield break;
	}


	public void OnGestureHelp() {
		if(helpAlpha < 1) {
			helpAlpha += helpAlphaInc;
		}
	}

	#endregion


	#region Fade

	IEnumerator FadeTitleAuthor(string title, string author) {

		titleStyle.normal.background = null;
		authorStyle.normal.background = null;

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

		needsSetFocusToTitleTextField = true;
		titleStyle.normal.background = texTextFieldBorder;
		authorStyle.normal.background = texTextFieldBorder;

		displayingTitle = TITLE_PLACEHOLDER;
		displayingAuthor = AUTHOR_PLACEHOLDER;

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

	IEnumerator FadeInVignette() {
		for (float alpha = 0; alpha <= 1; alpha+= 0.015625f) {
			vignetteAlpha = alpha;
			yield return null;
		}
	}

	IEnumerator FadeOutVignette() {
		for (float alpha = 1; alpha >= 0; alpha-= 0.0625f) {
			vignetteAlpha = alpha;
			yield return null;
		}
	}

	#endregion


	#region EditButton

	IEnumerator OnEditButtonPicked() {

		if (state != State.Show) {
			yield break;
		}

		state = State.Edit;

		editButton.SwellAndDisable ();
		cancelEditingButton.enabled = true;
		saveEditingButton.enabled = true;

		StartCoroutine (FadeInVignette ());
		yield return new WaitForSeconds (0.5f);

		Poser poser = shelf.CurrentPoser();		
		poser.EditEnabled = true;
		poser.Highlighted = Highlightable.HighlightDegree.Full;

		yield return new WaitForSeconds (.25f);
		
		poser.Highlighted = Highlightable.HighlightDegree.None;

		StartCoroutine (Record ());
	}

	#endregion


	#region CancelEditingButton

	IEnumerator OnCancelButtonPicked() {

		if (state != State.Edit) {
			yield break;
		}

		isRecording = false;
		records = null;

		cancelEditingButton.SwellAndDisable ();
		editButton.enabled = true;
		saveEditingButton.enabled = false;

		yield return StartCoroutine (FadeOutVignette ());
		state = State.Show;

		Poser poser = shelf.CurrentPoser ();
		poser.ApplyPose (Pose.DefaultPose (), 1);
		poser.Highlighted = Highlightable.HighlightDegree.Pale;
		poser.EditEnabled = false;
	}

	#endregion


	#region SaveEditingButton

	IEnumerator OnSaveButtonPicked() {

		if (state != State.Edit) {
			yield break;
		}

		isRecording = false;

		saveEditingButton.SwellAndDisable ();
		cancelEditingButton.enabled = false;
		doneEditingButton.enabled = true;
		
		Poser poser = shelf.CurrentPoser ();
		poser.Highlighted = Highlightable.HighlightDegree.Pale;
		poser.EditEnabled = false;

		yield return StartCoroutine (FadeOutVignette ());
		state = State.TypeTextInfo;
		yield return StartCoroutine(FadeInTitleAuthorTextField());
	}

	#endregion


	#region DoneButton

	void OnDoneButtonPicked() {

		if (state != State.TypeTextInfo) {
			return;
		}

		state = State.Show;

		if (displayingTitle.Equals(TITLE_PLACEHOLDER)) {
			displayingTitle = "무제";
		}
		if (displayingAuthor.Equals(AUTHOR_PLACEHOLDER)) {
			displayingAuthor = "익명";
		}
		titleStyle.normal.background = null;
		authorStyle.normal.background = null;

		doneEditingButton.SwellAndDisable ();

		Poser poser = shelf.CurrentPoser ();
		poser.EditEnabled = true;
		poser.Highlighted = Highlightable.HighlightDegree.None;
		poser.EditEnabled = false;
		
		List<Pose> motion = records;
		records = null;
		Preset preset = new Preset (motion, displayingTitle, displayingAuthor);

		shelf.InsertPresetBeforeLast (preset);

		BeginPlayCenterSlotIfAnimated ();
	}

	#endregion


	#region Record

	IEnumerator Record() {
		records = new List<Pose> (recordCount);
		isRecording = true;
		recordBeginTime = Time.time;
		Poser poser = shelf.CurrentPoser ();

		for (int i = 0; i < recordCount; i++) {
			if (isRecording == false || records == null) {
				recordBeginTime = 0;
				yield break;
			}
			records.Add (poser.GetCurrentPose());
			yield return new WaitForSeconds(recordInterval);

		}

		isRecording = false;

		poser.DisconnectFromRigidbody ();
		poser.Highlighted = Highlightable.HighlightDegree.Pale;
		poser.EditEnabled = false;
	}

	#endregion

}
