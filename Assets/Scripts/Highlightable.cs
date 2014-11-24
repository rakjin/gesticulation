using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour {

	public enum HighlightDegree {
		None,
		Half,
		Full,
		Pale,
	}
	
	virtual protected Color normalColor { get { return new Color (80/256f, 140/256f, 160/256f); } }
	virtual protected Color halfHighlightedColor { get { return new Color (130/256f, 190/256f, 210/256f); } }
	virtual protected Color highlightedColor { get { return new Color (20/256f, 80/256f, 100/256f); } }
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

	void OnMouseUpAsButton() {
		GameController.Instance.OnPickStateChanged (
			GameController.PickState.Hovering,
			GameController.PickState.Picking,
			null,
			gameObject);
	}
	
	#endregion
}
