using UnityEngine;
using System.Collections;

public class Shelf : MonoBehaviour {

	public Transform puppetPrefab;

	const int slotsNum = 3;
	virtual public int SlotsNum {
		get {
			return slotsNum;
		}
	}

	virtual public int CenterSlot {
		get {
			return 1;
		}
	}

	// Use this for initialization
	void Start () {

		if (puppetPrefab == null) {
			Debug.LogError("puppetPrefab required");
		} else {
			FillSlots ();
		}
	
	}

	virtual public Vector3 GetSlotPosition(int slotNum) {

		return Vector3.zero;

	}


	void FillSlots() {

		for (int i = 0; i < SlotsNum; i++) {

			Transform puppet = (Transform)Instantiate(puppetPrefab, GetSlotPosition(i), Quaternion.identity);
			Poser poser = puppet.gameObject.GetComponent<Poser>();
			poser.Highlighted = Part.HighlightDegree.Pale;
			if (i % 2 == 0) {
				poser.Visible = false;
			}

		}

	}

}
