using UnityEngine;
using System.Collections;

public class Part : MonoBehaviour {

	public CharacterJoint joint { get; private set; }

	void Awake () {
		joint = GetComponent<CharacterJoint> ();
	}
}
