﻿using UnityEngine;
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

	public Pose GetCurrentPose() {
		Pose pose = new Pose ();

		pose.RootPosition = Root.transform.localPosition;
		pose.RootRotation = Root.transform.localRotation;

		pose.Chest = Chest.transform.localRotation;
		pose.Head = Head.transform.localRotation;

		pose.ThighL = ThighL.transform.localRotation;
		pose.ThighR = ThighR.transform.localRotation;
		pose.ShinL = ShinL.transform.localRotation;
		pose.ShinR = ShinR.transform.localRotation;
		pose.FootL = FootL.transform.localRotation;
		pose.FootR = FootR.transform.localRotation;

		pose.ShoulderL = ShoulderL.transform.localRotation;
		pose.ShoulderR = ShoulderR.transform.localRotation;
		pose.UpperArmL = UpperArmL.transform.localRotation;
		pose.UpperArmR = UpperArmR.transform.localRotation;
		pose.ForearmL = ForearmL.transform.localRotation;
		pose.ForearmR = ForearmR.transform.localRotation;
		pose.HandL = HandL.transform.localRotation;
		pose.HandR = HandR.transform.localRotation;

		return pose;
	}

	public void Pose(Pose pose) {
		Root.transform.localPosition = pose.RootPosition;
		Root.transform.localRotation = pose.RootRotation;

		Chest.transform.localRotation = pose.Chest;
		Head.transform.localRotation = pose.Head;

		ThighL.transform.localRotation = pose.ThighL;
		ThighR.transform.localRotation = pose.ThighR;
		ShinL.transform.localRotation = pose.ShinL;
		ShinR.transform.localRotation = pose.ShinR;
		FootL.transform.localRotation = pose.FootL;
		FootR.transform.localRotation = pose.FootR;

		ShoulderL.transform.localRotation = pose.ShoulderL;
		ShoulderR.transform.localRotation = pose.ShoulderR;
		UpperArmL.transform.localRotation = pose.UpperArmL;
		UpperArmR.transform.localRotation = pose.UpperArmR;
		ForearmL.transform.localRotation = pose.ForearmL;
		ForearmR.transform.localRotation = pose.ForearmR;
		HandL.transform.localRotation = pose.HandL;
		HandR.transform.localRotation = pose.HandR;
	}

}

