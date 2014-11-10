using UnityEngine;
using System.Collections;

public class Button3D : MonoBehaviour {

	public Vector3 EnablePosition;
	public Vector3 EnableScale;
	public Vector3 DisablePosition;
	public Vector3 DisableScale;
	public Vector3 SwollenScale;
	
	TextMesh textMesh;
	Transform boxShape;

	bool isLabel = false;
	public bool IsLabel {
		get {
			return isLabel;
		}
		set {
			isLabel = value;
			textMesh.color = value? Color.white : Color.gray;
			boxShape.renderer.enabled = !value;
		}
	}

	string text = "";
	public string Text {
		get {
			return text;
		}
		set {
			text = value;
			textMesh.text = value;
		}
	}

	// Use this for initialization
	void Awake () {

		GameObject textGO = transform.Find ("Text").gameObject;
		textMesh = textGO.GetComponent<TextMesh> ();
		textMesh.text = " ";

		boxShape = transform.Find ("BoxShape");
	}

	new public bool enabled {

		set {
			if (value == true) {
				GetComponentInChildren<Highlightable>().Highlighted = Highlightable.HighlightDegree.None;
				LeanTween.moveLocal(
					gameObject,
					EnablePosition,
					0.5625f)
					.setEase (LeanTweenType.easeInOutCubic);
				LeanTween.scale(
					gameObject,
					EnableScale,
					0.5625f)
					.setEase (LeanTweenType.easeInOutCubic);
			} else {
				LeanTween.moveLocal(
					gameObject,
					DisablePosition,
					0.125f)
					.setEase (LeanTweenType.easeInOutCubic);
				LeanTween.scale(
					gameObject,
					DisableScale,
					0.125f)
					.setEase (LeanTweenType.easeInOutCubic);
			}
			base.enabled = value;
		}

		get {
			return base.enabled;
		}

	}

	public void SwellAndDisable() {
		LeanTween.scale (
			gameObject,
			SwollenScale,
			0.25f)
			.setEase (LeanTweenType.easeInOutCubic);
		
		LeanTween.moveLocal(
			gameObject,
			DisablePosition,
			0.125f)
			.setEase (LeanTweenType.easeInOutCubic)
			.setDelay (0.25f);

		LeanTween.scale (
			gameObject,
			DisableScale,
			0.125f)
			.setEase (LeanTweenType.easeInOutCubic)
			.setDelay (0.25f);
	}

}
