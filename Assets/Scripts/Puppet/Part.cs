using UnityEngine;
using System.Collections;

public class Part {

	public Transform transform { get; private set; }
	public CharacterJoint joint { get; private set; }

	public Part(Transform tramsform) {
		this.transform = tramsform;
		this.joint = tramsform.GetComponent<CharacterJoint> ();
	}
}
