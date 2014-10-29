using UnityEngine;
using System.Collections;

public class CircularShelf : MonoBehaviour {


	const float radius = 15;
	const int startDeg = 270;
	const int endDeg = 90;
	const int slotsNum = 11;

	const float degPerStep = ((float)(startDeg - endDeg)) / ((float)(slotsNum - 1));
	const int centerSlot = slotsNum/2;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {

		Gizmos.color = Color.red;

		for (int i = 0; i < slotsNum; i++) {
			Gizmos.DrawWireSphere (GetSlotPosition (i), 1);
		}

	}


	Vector3 GetSlotPosition(int slotNum) {

		float y = 0;

		float x = Mathf.Sin (Mathf.Deg2Rad  * (startDeg + (slotNum * degPerStep))) * radius;
		float z = -Mathf.Cos (Mathf.Deg2Rad  * (startDeg + (slotNum * degPerStep))) * radius;

		z += transform.position.z;

		return new Vector3(x, y, z);
	}

}
