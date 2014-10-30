using UnityEngine;
using System.Collections;

public class Shelf : MonoBehaviour {

	const int slotsNum = 3;
	virtual public int SlotsNum {
		get {
			return slotsNum;
		}
	}

	// Use this for initialization
	void Start () {

		FillSlots ();
	
	}

	virtual public Vector3 GetSlotPosition(int slotNum) {

		return Vector3.zero;

	}


	void FillSlots() {

		for (int i = 0; i < SlotsNum; i++) {

			Debug.Log (i);

		}

	}

}
