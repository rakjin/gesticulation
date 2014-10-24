using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GameObject defaultPuppet = GameObject.Find ("/Puppet");
		Debug.Log (defaultPuppet);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
