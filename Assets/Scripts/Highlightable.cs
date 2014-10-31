using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour {

	public enum HighlightDegree {
		None,
		Half,
		Full,
		Pale,
	}
	
	virtual protected Color normalColor { get { return new Color (0.3125f, 0.625f, 0.46875f); } }
	virtual protected Color halfHighlightedColor { get { return new Color (0.46875f, 0.875f, 0.625f); } }
	virtual protected Color highlightedColor { get { return new Color (0.125f, 0.5f, 0.25f); } }
	virtual protected Color paleColor { get { return new Color (0.375f, 0.625f, 1); } }


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
