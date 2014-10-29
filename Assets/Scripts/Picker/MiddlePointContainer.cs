using UnityEngine;
using System.Collections;

public class MiddlePointContainer : MonoBehaviour {

	Picker picker;
	public void SetPicker(Picker picker) {
		this.picker = picker;
	}
	
	void OnTriggerEnter(Collider other) {
		picker.TriggerEnter (other);
	}

	void OnTriggerExit(Collider other) {
		picker.TriggerExit (other);
	}
}
