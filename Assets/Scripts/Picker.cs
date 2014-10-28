using UnityEngine;
using System.Collections;

using Leap;

public class Picker : MonoBehaviour {

	FingerModel thumb;
	FingerModel index;

	// Use this for initialization
	void Start () {

		HandModel handModel = GetComponent<HandModel> ();
		thumb = handModel.fingers [0];
		index = handModel.fingers [1];
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (thumb.GetTipPosition (), 0.125f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (index.GetTipPosition (), 0.125f);
	}

	void OnDestroy() {
		Debug.Log ("I'm Dying!");
	}
}
