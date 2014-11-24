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

	public const string SAVE_FILE_NAME_FORMAT = "gest_data/user_{0}.pose";

	const string TAG_PART = "Part";
	const string TAG_BUTTON = "Button3D";

	Color WHITE_0_9 = new Color (1, 1, 1, 0.9f);
	Color WHITE_0_5 = new Color (1, 1, 1, 0.5f);
	Color WHITE_0_25 = new Color (1, 1, 1, 0.25f);

	public Transform container;
	public Transform ground;
	public Transform buttonContainer;
	public Transform button3D;
	Shelf currentShelf;
	public Shelf sampleShelf;
	public Shelf userShelf;
	public GUIStyle infoStyle;
	public GUIStyle commentStyle;
	public GUIStyle titleStyle;
	public GUIStyle authorStyle;
	public Texture2D texSplash;
	public Texture2D texTextFieldBorder;
	float splashAlpha = 1;
	public Texture2D texVignette;
	public Texture2D texRec;
	float vignetteAlpha = 0;
	Texture2D texEmpty;
	public Texture2D texHelp;
	public Texture2D texHelpIcon;


	State state = State.Splash;

	string displayingTitle = "";
	string displayingAuthor = "";
	bool needsSetFocusToTitleTextField = true;
	const string TITLE_PLACEHOLDER = "키보드로 제목을 입력해주세요.";
	const string AUTHOR_PLACEHOLDER = "이름을 입력해주세요. (ENTER)";

	string displayingInfo = "";
	const float INFO_ALPHA = 0.3f;
	float infoAlpha = INFO_ALPHA;
	const string INFO_TEXT_SAMPLE_GALLERY = "샘플 갤러리";
	const string INFO_TEXT_USER_GALLERY = "관람객 참여 갤러리";
	const string INFO_TEXT_NEW_RECORDING = "새 작품 녹화";
	const string INFO_TEXT_NOW_RECORDING = "녹화중...";
	const string INFO_TEXT_RECORDING_DONE = "녹화 완료";

	string displayingComment = "";
	Queue<string> displayingCommentQueue = new Queue<string>();
	const float COMMENT_ALPHA = 0.7f;
	float commentAlpha = COMMENT_ALPHA;
	const int COMMENT_DISPLAY_FRAMES = 5 * 60;
	int commentDisplayFrames = 0;
	const string COMMENT_TEXT_SAMPLE_GALLERY = "미리 만들어둔 샘플 작품들입니다.";
	const string COMMENT_TEXT_USER_GALLERY = "여러분이 직접 만든 작품들입니다. 오른쪽 끝의 인형으로 이동해서 참여해보세요.";
	const string COMMENT_TEXT_ENCOURAGE_PICK_A_PART = "목각인형을 붙잡아 자세를 바꿔보세요. 그 과정이 녹화됩니다.";
	const string COMMENT_TEXT_RECORDING_DONE_BY_TIMER = "세션이 끝났습니다. 저장하시겠습니까?";
	const string COMMENT_TEXT_RECORDING_DONE_BY_BUTTON = "작품 제목을 입력해주세요.";

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

	const float recordDuration = 90;
	const float recordFPS = 4;
	const float recordInterval = 1f / recordFPS;
	const int recordCount = (int) (recordDuration * recordFPS);
	List<Pose> records;
	bool isRecording = false;
	float recordBeginTime;

	bool isPlaying = false;
	float playBeginTime;
	const float PLAYBACK_SPEED = 3;


	const float GROUND_SHIFT = 2;


	const float helpAlphaInc = 0.125f;
	const float helpAlphaDec = helpAlphaInc / -2;
	float helpAlpha = 0;


	#region audio clips

	public AudioClip audioHover;
	public AudioClip audioFlipLeft;
	public AudioClip audioFlipRight;
	public AudioClip audioSwapGallery;
	public AudioClip audioPickPart;
	public AudioClip audioPickButton;
	public AudioClip audioSaveSuccess;

	#endregion


	// Use this for initialization
	IEnumerator Start () {
		Instance = this;

		if (sampleShelf == null || userShelf == null) {
			Debug.LogError ("shelves required");
		}
		currentShelf = sampleShelf;
		sampleShelf.OnFlipComplete += OnShelfFlipComplete;
		userShelf.OnFlipComplete += OnShelfFlipComplete;

		texEmpty = new Texture2D (1, 1);
		texEmpty.anisoLevel = 0;
		texEmpty.filterMode = FilterMode.Point;
		texEmpty.SetPixel (0, 0, Color.white);

		Preset preset = currentShelf.CurrentPreset();
		displayingTitle = preset.Title;
		displayingAuthor = preset.Author;

		Setup3DGUI ();

		yield return new WaitForSeconds (0.00001f);

		for (float alpha = 1; alpha >= 0; alpha-= 0.0078125f) {
			splashAlpha = alpha;
			yield return null;
		}

		state = State.Show;
		StartCoroutine (CheckAndDequeueCommentText ());

		UpdateGalleryInfoAndComment ();
	}

	void OnShelfFlipComplete () {
		BeginPlayCenterSlotIfAnimated ();
	}

	void BeginPlayCenterSlotIfAnimated ()
	{
		Preset centerPreset = currentShelf.CurrentPreset ();
		if (centerPreset.Type == Preset.PresetType.Animated) {
			Poser centerPoser = currentShelf.CurrentPoser();
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
			float authorX = padding;
			float authorY = screenHeight - authorHeight - padding*3;
			Rect authorRect = new Rect (authorX, authorY, authorWidth, authorHeight);
			authorStyle.fontSize = (int)(unit * 3);
			authorStyle.padding.top = (int)authorHeight/16;

			float titleWidth = unit*80;
			float titleHeight = unit*6;
			float titleX = authorX;
			float titleY = authorY - gap - titleHeight;
			Rect titleRect = new Rect (titleX, titleY, titleWidth, titleHeight);
			titleStyle.fontSize = (int)(unit * 4);
			titleStyle.padding.top = (int)titleHeight/16;

			if (state == State.Show) {
				GUI.color = WHITE_0_9;
				GUI.Label (titleRect, displayingTitle, titleStyle);
				GUI.color = WHITE_0_5;
				GUI.Label (authorRect, displayingAuthor, authorStyle);

				if (currentShelf.CurrentPreset ().Type == Preset.PresetType.Animated && isPlaying) {
					float progress = Mathf.Clamp01( (Time.time - playBeginTime)/(currentShelf.CurrentPreset().Motion.Count*recordInterval/PLAYBACK_SPEED) );
					if (playBeginTime == 0) {
						progress = 0;
					}
					DrawProgressBar(new Vector2(screenWidth, screenHeight), unit, progress*progress*4, progress);
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
				GUI.color = showFlickeringRec? Color.white : WHITE_0_25;
				GUI.DrawTexture(new Rect(recX, recY, recWidth, recHeight), texRec, ScaleMode.ScaleToFit);
			}

		}

		{ // info and comment text
			float infoWidth = screenWidth;
			float infoHeight = unit*3;
			float infoX = 0;
			float infoY = padding;
			Rect infoRect = new Rect (infoX, infoY, infoWidth, infoHeight);
			infoStyle.fontSize = (int)(unit * 2);
			infoStyle.padding.top = (int)infoHeight/16;
			GUI.color = new Color(1, 1, 1, infoAlpha);
			GUI.Label (infoRect, displayingInfo, infoStyle);

			if (commentAlpha >= 0) {
				float commentWidth = screenWidth;
				float commentHeight = unit*3;
				float commentX = 0;
				float commentY = infoY + infoHeight + gap;
				Rect commentRect = new Rect (commentX, commentY, commentWidth, commentHeight);
				commentStyle.fontSize = (int)(unit * 2);
				commentStyle.padding.top = (int)commentHeight/16;
				GUI.color = new Color(1, 1, 1, commentAlpha);
				GUI.Label (commentRect, displayingComment, commentStyle);
			}
		}

		{ // help icon
			float helpIconWidth = unit*13;
			float helpIconHeight = unit*8;
			float helpIconX = screenWidth-helpIconWidth-gap/4;
			float helpIconY = screenHeight-helpIconHeight-gap/4;
			GUI.color = new Color(1, 1, 1, (1-vignetteAlpha)*0.5f);
			GUI.DrawTexture(new Rect(helpIconX, helpIconY, helpIconWidth, helpIconHeight), texHelpIcon, ScaleMode.ScaleToFit);
		}

		if (helpAlpha > 0) {
			float helpWidth = screenWidth;
			float helpHeight = screenHeight;
			float helpX = (screenWidth-helpWidth)/2;
			float helpY = (screenHeight-helpHeight)/2;
			GUI.color = new Color(1, 1, 1, helpAlpha);
			GUI.DrawTexture(new Rect(helpX, helpY, helpWidth, helpHeight), texHelp, ScaleMode.ScaleToFit);
		}


		if (showDebugUI) {
			GUI.color = Color.white;
			if (GUI.Button (new Rect(0, 10, 200, 30), "print pose")) {
				Debug.Log (currentShelf.CurrentPoser().GetCurrentPose());
			} else if (GUI.Button (new Rect(210, 10, 50, 30), "<<")) {
				OnGestureSwipe(GestureDetector.Direction.ToLeft);
			} else if (GUI.Button (new Rect(260, 10, 50, 30), ">>")) {
				OnGestureSwipe(GestureDetector.Direction.ToRight);
			}
		}
	}

	void DrawProgressBar(Vector2 screenSize, float unit, float baseAlpha, float progress) {
		baseAlpha = Mathf.Clamp01 (baseAlpha);
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
		} else if (Input.GetKey (KeyCode.F1)) {
			OnGestureHelp();
		}

		if (helpAlpha > 0) {
			helpAlpha += helpAlphaDec;
		}

		infoAlpha = INFO_ALPHA + (infoAlpha-INFO_ALPHA)*0.95f;
		if (displayingComment.Equals(string.Empty) == false) {
			commentAlpha = COMMENT_ALPHA + (commentAlpha-COMMENT_ALPHA)*0.97f;
		}
	}


	#region Pick

	bool pickedAnyPart = false;

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

			if (currentShelf.CurrentPreset().Type == Preset.PresetType.NewPresetPlaceHolder) {
				audio.PlayOneShot(audioHover);
			}
			
			break;
			

		case PickState.Picking:

			if (target && target.tag.Equals(TAG_PART) && picker != null) {
				Part part = target.GetComponent<Part>();
				part.Highlighted = Part.HighlightDegree.Full;
				part.ConnectToRigidbody(picker.MiddlePointContainer.rigidbody, Vector3.zero);

				pickedAnyPart = true;

				if (currentShelf.CurrentPreset().Type == Preset.PresetType.NewPresetPlaceHolder) {
					audio.PlayOneShot(audioPickPart);
				}

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

				audio.PlayOneShot(audioPickButton);

			}

			break;
		}
	}

	#endregion


	#region gesture

	bool ignoreGesture = false;
	const float ignoreGestureTimeSpan = Shelf.FLIP_DURATION + 0.015625f;

	public void OnGestureSwipe(GestureDetector.Direction direction, float speedMultiplier = 1) {
		if(ignoreGesture == false && state == State.Show) {
			ignoreGesture = true;
			StartCoroutine(ResetIgnoreGestureFlag(speedMultiplier));

			if (direction == GestureDetector.Direction.ToLeft ||
			    direction == GestureDetector.Direction.ToRight) {

				bool toLeft = (direction == GestureDetector.Direction.ToLeft);
				bool flipped = currentShelf.Flip(toLeft, speedMultiplier);
				if (flipped) {
					UpdateTitleAuthorAndEditButton ();

					float groundShift = toLeft? -GROUND_SHIFT : GROUND_SHIFT;
					LeanTween.moveLocalX(ground.gameObject, groundShift, Shelf.FLIP_DURATION / speedMultiplier)
						.setEase(LeanTweenType.easeInOutSine)
						.setOnComplete(() => {
							Vector3 backToOrigin = new Vector3(0, ground.localPosition.y, ground.localPosition.z);
							ground.localPosition = backToOrigin;
						});

					audio.PlayOneShot(toLeft ? audioFlipLeft : audioFlipRight);
				}
			} else if (direction == GestureDetector.Direction.Pull ||
			           direction == GestureDetector.Direction.Push) {

				if((currentShelf == sampleShelf && direction == GestureDetector.Direction.Push) ||
				   (currentShelf == userShelf && direction == GestureDetector.Direction.Pull)) {
					return;
				}

				if (currentShelf.CurrentPreset().Type == Preset.PresetType.Animated) {
					currentShelf.CurrentPoser().StopMotion();
				}

				SwapShelf();
			}
		}
	}

	IEnumerator ResetIgnoreGestureFlag(float speedMultiplier = 1) {
		yield return new WaitForSeconds(ignoreGestureTimeSpan / speedMultiplier);
		ignoreGesture = false;
		yield break;
	}


	public void OnGestureHelp() {
		if(helpAlpha < 1) {
			helpAlpha += helpAlphaInc;
		}
	}

	public void OnGestureScroll(float strength) {
		bool toLeft = (strength < 0);
		GestureDetector.Direction direction = toLeft ? GestureDetector.Direction.ToLeft : GestureDetector.Direction.ToRight;
		float scrollSpeedMultiplier = (Mathf.Abs (strength)*4) + 1;
		OnGestureSwipe (direction, scrollSpeedMultiplier);
	}

	#endregion


	#region Fade

	IEnumerator FadeTitleAuthor(string title, string author) {

		titleStyle.normal.background = null;
		authorStyle.normal.background = null;

		const int frames = 13;
		Color titleTextColor = titleStyle.normal.textColor;
		Color authorTextColor = authorStyle.normal.textColor;

		for (int i = 0; i <= frames; i++) {
			float alpha = (frames-i)/(float)frames;
			titleStyle.normal.textColor = new Color(titleTextColor.r, titleTextColor.g, titleTextColor.b, alpha);
			authorStyle.normal.textColor = new Color(authorTextColor.r, authorTextColor.g, authorTextColor.b, alpha);
			yield return null;
		}

		displayingTitle = title;
		displayingAuthor = author;

		for (int i = 0; i <= frames; i++) {
			float alpha = i/(float)frames;
			titleStyle.normal.textColor = new Color(titleTextColor.r, titleTextColor.g, titleTextColor.b, alpha);
			authorStyle.normal.textColor = new Color(authorTextColor.r, authorTextColor.g, authorTextColor.b, alpha);
			yield return null;
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


	#region Info and Comment TextField

	void UpdateInfoText(string text) {
		displayingInfo = text;
		infoAlpha = 1;
	}

	void EnqueueCommentText(string text) {
		displayingCommentQueue.Enqueue (text);
	}

	void ClearCommentTextQueueAndImmediatelyUpdate(string text) {
		displayingCommentQueue.Clear ();
		EnqueueCommentText(text);
		commentDisplayFrames = COMMENT_DISPLAY_FRAMES;
	}

	IEnumerator CheckAndDequeueCommentText() {
		while(true) {
			if (displayingCommentQueue.Count > 0) {
				displayingComment = displayingCommentQueue.Dequeue();
				commentAlpha = 1;
				commentDisplayFrames = 0;
				while(commentDisplayFrames < COMMENT_DISPLAY_FRAMES) {
					commentDisplayFrames += 1;
					yield return null;
				}

			} else {
				if (displayingComment.Equals(string.Empty) == false) {
					for(int i = 0; i < 60; i++) {
						commentAlpha -= 0.03125f;
						yield return null;
					}
					displayingComment = string.Empty;
				}
				yield return null;

			}
		}
	}

	#endregion


	#region EditButton

	IEnumerator OnEditButtonPicked() {

		if (state != State.Show) {
			yield break;
		}

		state = State.Edit;

		UpdateInfoText (INFO_TEXT_NEW_RECORDING);
		EnqueueCommentText (COMMENT_TEXT_ENCOURAGE_PICK_A_PART);

		editButton.SwellAndDisable ();
		cancelEditingButton.enabled = true;

		StartCoroutine (FadeInVignette ());
		yield return new WaitForSeconds (0.5f);

		Poser poser = currentShelf.CurrentPoser();		
		poser.EditEnabled = true;
		poser.Highlighted = Highlightable.HighlightDegree.Full;

		yield return new WaitForSeconds (.25f);
		
		poser.Highlighted = Highlightable.HighlightDegree.None;

		isWaitingFirstPick = true;
		pickedAnyPart = false;
		StartCoroutine (WaitFirstPickAndRecord ());
	}

	#endregion


	#region CancelEditingButton

	IEnumerator OnCancelButtonPicked() {

		if (state != State.Edit) {
			yield break;
		}

		pickedAnyPart = false;
		isWaitingFirstPick = false;
		isRecording = false;
		records = null;

		cancelEditingButton.SwellAndDisable ();
		editButton.enabled = true;
		saveEditingButton.enabled = false;

		yield return StartCoroutine (FadeOutVignette ());
		state = State.Show;

		Poser poser = currentShelf.CurrentPoser ();
		poser.ApplyPose (Pose.DefaultPose (), 1);
		poser.Highlighted = Highlightable.HighlightDegree.Pale;
		poser.EditEnabled = false;

		UpdateGalleryInfoAndComment (skipComment:true);
	}

	#endregion


	#region SaveEditingButton

	IEnumerator OnSaveButtonPicked() {

		if (state != State.Edit) {
			yield break;
		}

		UpdateInfoText (INFO_TEXT_RECORDING_DONE);
		EnqueueCommentText (COMMENT_TEXT_RECORDING_DONE_BY_BUTTON);

		isRecording = false;

		saveEditingButton.SwellAndDisable ();
		cancelEditingButton.enabled = false;
		doneEditingButton.enabled = true;
		
		Poser poser = currentShelf.CurrentPoser ();
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

		Poser poser = currentShelf.CurrentPoser ();
		poser.EditEnabled = true;
		poser.Highlighted = Highlightable.HighlightDegree.None;
		poser.EditEnabled = false;
		
		List<Pose> motion = records;
		records = null;
		Preset preset = new Preset (motion, displayingTitle, displayingAuthor);

		currentShelf.InsertPresetBeforeLast (preset);

		BeginPlayCenterSlotIfAnimated ();

		UpdateGalleryInfoAndComment (skipComment:true);

		StartCoroutine (SavePresetAsFile (preset, currentShelf.Count-1));

		audio.PlayOneShot(audioSaveSuccess);
	}

	#endregion


	#region Record

	bool isWaitingFirstPick = false;

	IEnumerator WaitFirstPickAndRecord() {
		while (isWaitingFirstPick && !pickedAnyPart) {
			yield return null;
		}

		if (isWaitingFirstPick == false) {
			yield break;

		} else if (pickedAnyPart) {
			saveEditingButton.enabled = true;
			yield return StartCoroutine(Record());

		}
	}

	IEnumerator Record() {
		records = new List<Pose> (recordCount);
		isRecording = true;
		recordBeginTime = Time.time;
		Poser poser = currentShelf.CurrentPoser ();

		UpdateInfoText (INFO_TEXT_NOW_RECORDING);

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

		UpdateInfoText (INFO_TEXT_RECORDING_DONE);
		EnqueueCommentText (COMMENT_TEXT_RECORDING_DONE_BY_TIMER);
	}

	#endregion


	#region Swap Shelf

	void SwapShelf() {

		bool toUserShelf = (currentShelf == sampleShelf) ? true : false;
		float destZ = (toUserShelf) ? 17 : 0;
		currentShelf = (toUserShelf) ? userShelf : sampleShelf;
		LeanTween.moveLocalZ (container.gameObject, destZ, Shelf.FLIP_DURATION).setEase(LeanTweenType.easeInOutExpo);

		UpdateTitleAuthorAndEditButton ();
		BeginPlayCenterSlotIfAnimated ();
		UpdateGalleryInfoAndComment ();

		audio.PlayOneShot(audioSwapGallery);
	}

	void UpdateGalleryInfoAndComment(bool skipComment = false) {
		bool isSampleShelf = (currentShelf == sampleShelf) ? true : false;
		UpdateInfoText (isSampleShelf ? INFO_TEXT_SAMPLE_GALLERY : INFO_TEXT_USER_GALLERY);
		if (skipComment) {
			return;
		}
		ClearCommentTextQueueAndImmediatelyUpdate (isSampleShelf ? COMMENT_TEXT_SAMPLE_GALLERY : COMMENT_TEXT_USER_GALLERY);
	}

	#endregion


	void UpdateTitleAuthorAndEditButton ()
	{
		isPlaying = false;
		playBeginTime = 0;
		Preset preset = currentShelf.CurrentPreset ();
		StartCoroutine (FadeTitleAuthor (preset.Title, preset.Author));
		if (preset.Type == Preset.PresetType.NewPresetPlaceHolder) {
			editButton.enabled = true;
		}
		else {
			editButton.enabled = false;
		}
	}


	#region Save as file

	IEnumerator SavePresetAsFile (Preset preset, int index) {
		try {
			preset.Serialize ().SaveToFile (string.Format (SAVE_FILE_NAME_FORMAT, index));
		} catch {
		}
		yield break;
	}

	#endregion

}
