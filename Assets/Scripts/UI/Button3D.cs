using UnityEngine;
using System.Collections;

public class Button3D : MonoBehaviour {

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
}
