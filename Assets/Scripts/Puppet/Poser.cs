using UnityEngine;
using System.Collections;

public class Poser : MonoBehaviour {

	public Transform Root { get; private set; }

	private Transform chest;
	private Transform head;

	public CharacterJoint Chest { get { return chest.GetComponent<CharacterJoint>(); } }
	public CharacterJoint Head { get { return head.GetComponent<CharacterJoint>(); } }


	// Use this for initialization
	void Start () {

		ReadRootPart ();
		ReadJoints ();

		Chest.rigidbody.AddForce (new Vector3 (0, 0, -30));
		Head.rigidbody.AddForce (new Vector3 (0, 0, 10));
	
	}

	void ReadRootPart () {
		Root = transform.Find ("puppet");
		if (!Root) {
			Debug.LogError("root part not found");
		}
	}

	void ReadJoints () {
		chest = ReadJoint ("puppet/Chest");
		head = ReadJoint ("puppet/Chest/Neck/Head");
	}

	Transform ReadJoint (string name) {
		Transform joint = transform.Find (name);
		if (!joint) {
			Debug.LogError("joint (" + name + ") not found");
		}
		return joint;
	}

}

