using UnityEngine;
using System.Collections;

public class Poser : MonoBehaviour {

	public Transform Root { get; private set; }

	private Transform chest;
	private Transform head;

	public Quaternion Chest { get { return chest.rotation; } }
	public Quaternion Head { get { return head.rotation; } }


	// Use this for initialization
	void Start () {

		ReadRootPart ();
		ReadJoints ();
	
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

