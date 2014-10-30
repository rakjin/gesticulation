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

			Instantiate(puppetPrefab, GetSlotPosition(i), Quaternion.identity);

		}

	}

}
