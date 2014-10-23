using UnityEngine;
using System.Collections;

public class Poser : MonoBehaviour {

	public Part Root { get; private set; }
	public Part Chest { get; private set; }
	public Part Head { get; private set; }

	// Use this for initialization
	void Start () {
		ReadParts ();

		Chest.transform.localEulerAngles = new Vector3 (-40, 0, 0);
		Head.transform.localEulerAngles = new Vector3 (40, 0, 0);
	}

	void ReadParts () {
		Root = ReadPart ("puppet");
		Chest = ReadPart ("puppet/Chest");
		Head = ReadPart ("puppet/Chest/Neck/Head");
	}

	Part ReadPart (string partName) {
		Part part = new Part(transform.Find (partName));
		if (part.transform == null) {
			Debug.LogError("part(" + partName + ") not found");
		}
		return part;
	}

}

