using UnityEngine;
using System.Collections;

public class Poser : MonoBehaviour {

	public Part Root { get; private set; }

	public Part Chest { get; private set; }
	public Part Head { get; private set; }

	public Part ThighL { get; private set; }
	public Part ThighR { get; private set; }
	public Part ShinL { get; private set; }
	public Part ShinR { get; private set; }
	public Part FootL { get; private set; }
	public Part FootR { get; private set; }

	public Part ShoulderL { get; private set; }
	public Part ShoulderR { get; private set; }
	public Part UpperArmL { get; private set; }
	public Part UpperArmR { get; private set; }
	public Part ForearmL { get; private set; }
	public Part ForearmR { get; private set; }
	public Part HandL { get; private set; }
	public Part HandR { get; private set; }

	// Use this for initialization
	void Start () {
		ReadParts ();
	}

	void ReadParts () {
		Root = ReadPart ("puppet");

		Chest = ReadPart ("puppet/Chest");
		Head = ReadPart ("puppet/Chest/Neck/Head");

		ThighL = ReadPart ("puppet/Thigh_L");
		ThighR = ReadPart ("puppet/Thigh_R");
		ShinL = ReadPart ("puppet/Thigh_L/Shin_L");
		ShinR = ReadPart ("puppet/Thigh_R/Shin_R");
		FootL = ReadPart ("puppet/Thigh_L/Shin_L/Foot_L");
		FootR = ReadPart ("puppet/Thigh_R/Shin_R/Foot_R");

		ShoulderL = ReadPart ("puppet/Chest/Shoulder_L");
		ShoulderR = ReadPart ("puppet/Chest/Shoulder_R");
		UpperArmL = ReadPart ("puppet/Chest/Shoulder_L/UpperArm_L");
		UpperArmR = ReadPart ("puppet/Chest/Shoulder_R/UpperArm_R");
		ForearmL = ReadPart ("puppet/Chest/Shoulder_L/UpperArm_L/Forearm_L");
		ForearmR = ReadPart ("puppet/Chest/Shoulder_R/UpperArm_R/Forearm_R");
		HandL = ReadPart ("puppet/Chest/Shoulder_L/UpperArm_L/Forearm_L/Hand_L");
		HandR = ReadPart ("puppet/Chest/Shoulder_R/UpperArm_R/Forearm_R/Hand_R");
	}

	Part ReadPart (string partName) {
		Part part = new Part(transform.Find (partName));
		if (part.transform == null) {
			Debug.LogError("part(" + partName + ") not found");
		}
		return part;
	}

}

