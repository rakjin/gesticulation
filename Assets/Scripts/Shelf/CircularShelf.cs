using UnityEngine;
using System.Collections;

public class CircularShelf : Shelf {


	const float radius = 15;
	const int startDeg = 270;
	const int endDeg = 90;
	const int slotsNum = 11;
	override public int SlotsNum {
		get {
			return slotsNum;
		}
	}

	const float degPerStep = ((float)(startDeg - endDeg)) / ((float)(slotsNum - 1));
	const int centerSlot = slotsNum/2;
	

	void OnDrawGizmos() {

		Gizmos.color = Color.red;

		for (int i = 0; i < slotsNum; i++) {
			Gizmos.DrawWireSphere (GetSlotPosition (i), 0.5f);
		}

	}


	override public Vector3 GetSlotPosition(int slotNum) {

		float y = 0;

		float x = Mathf.Sin (Mathf.Deg2Rad  * (startDeg + (slotNum * degPerStep))) * radius;
		float z = -Mathf.Cos (Mathf.Deg2Rad  * (startDeg + (slotNum * degPerStep))) * radius;

		z *= 0.5f;
		z += transform.position.z;

		if (slotNum == centerSlot) {
			z = 0;
		}

		return new Vector3(x, y, z);
	}

}
