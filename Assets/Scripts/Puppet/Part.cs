using UnityEngine;
using System.Collections;

public class Part : MonoBehaviour {

	static readonly Color normalColor = new Color (0.5f, 0.5f, 0.5f);
	static readonly Color highlightedColor = new Color (0.125f, 1, 0);

	public CharacterJoint joint { get; private set; }

	void Awake () {
		joint = GetComponent<CharacterJoint> ();
	}

	private bool highlighted = false;
	public bool Highlighted {

		set {
			if (highlighted == value) {
				return;
			}

			highlighted = value;
			Color tint = (highlighted ? highlightedColor : normalColor);
			LeanTween.color (gameObject, tint, 0.125f);
		}

		get {
			return highlighted;
		}

	}
}
