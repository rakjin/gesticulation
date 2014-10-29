using UnityEngine;
using System.Collections;

public class Button3D : MonoBehaviour {

	TextMesh textMesh;

	// Use this for initialization
	void Start () {

		GameObject textGO = transform.Find ("Text").gameObject;
		textMesh = textGO.GetComponent<TextMesh> ();
		textMesh.text = " ";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
