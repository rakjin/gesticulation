using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour {

	public enum HighlightDegree {
		None,
		Half,
		Full,
		Pale,
	}
	
	protected readonly Color normalColor = new Color (0.5f, 0.5f, 0.5f);
	protected readonly Color halfHighlightedColor = new Color (0.5f, 0.875f, 0.875f);
	protected readonly Color highlightedColor = new Color (0.125f, 1, 0);
	protected readonly Color paleColor = new Color (0.375f, 0.625f, 1);


	#region Highlight
	
	private HighlightDegree highlighted = HighlightDegree.None;
	virtual public HighlightDegree Highlighted {
		
		set {
			if (highlighted == value) {
				return;
			}
			
			highlighted = value;
			Color tint = normalColor;
			if (highlighted == HighlightDegree.Half) {
				tint = halfHighlightedColor;
			} else if (highlighted == HighlightDegree.Full) {
				tint = highlightedColor;
			} else if (highlighted == HighlightDegree.Pale) {
				tint = paleColor;
			}
			LeanTween.color (gameObject, tint, 0.125f);
		}
		
		get {
			return highlighted;
		}
		
	}
	
	#endregion
}
