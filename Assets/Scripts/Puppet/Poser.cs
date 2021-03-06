﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poser : MonoBehaviour {

	public Part Root { get; private set; }

	public Part Chest { get; private set; }
	public Part Neck { get; private set; }
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

	private bool isPlayingAnimation = false;


	private LTDescr descrRootPosition;
	private LTDescr descrRootRotation;

	private LTDescr descrChest;
	private LTDescr descrNeck;
	private LTDescr descrHead;

	private LTDescr descrThighL;
	private LTDescr descrThighR;
	private LTDescr descrShinL;
	private LTDescr descrShinR;
	private LTDescr descrFootL;
	private LTDescr descrFootR;

	private LTDescr descrShoulderL;
	private LTDescr descrShoulderR;
	private LTDescr descrUpperArmL;
	private LTDescr descrUpperArmR;
	private LTDescr descrForearmL;
	private LTDescr descrForearmR;
	private LTDescr descrHandL;
	private LTDescr descrHandR;

	// Use this for initialization
	void Awake () {
		ReadParts ();
	}

	void ReadParts () {
		Root = ReadPart ("puppet");

		Chest = ReadPart ("puppet/Chest");
		Neck = ReadPart ("puppet/Chest/Neck");
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
		Part part = transform.Find (partName).gameObject.AddComponent<Part> ();
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

	public void ApplyPose(Pose pose) {
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

	public void ApplyPose(Pose pose, float duration) {

		descrRootPosition = LeanTween.moveLocal (Root.transform.gameObject, pose.RootPosition, duration);
		descrRootRotation = LeanTween.rotateLocal (Root.transform.gameObject, pose.RootRotation.eulerAngles, duration);

		descrChest = LeanTween.rotateLocal(Chest.transform.gameObject, pose.Chest.eulerAngles, duration);
		descrHead = LeanTween.rotateLocal(Head.transform.gameObject, pose.Head.eulerAngles, duration);

		descrThighL = LeanTween.rotateLocal(ThighL.transform.gameObject, pose.ThighL.eulerAngles, duration);
		descrThighR = LeanTween.rotateLocal(ThighR.transform.gameObject, pose.ThighR.eulerAngles, duration);
		descrShinL = LeanTween.rotateLocal(ShinL.transform.gameObject, pose.ShinL.eulerAngles, duration);
		descrShinR = LeanTween.rotateLocal(ShinR.transform.gameObject, pose.ShinR.eulerAngles, duration);
		descrFootL = LeanTween.rotateLocal(FootL.transform.gameObject, pose.FootL.eulerAngles, duration);
		descrFootR = LeanTween.rotateLocal(FootR.transform.gameObject, pose.FootR.eulerAngles, duration);

		descrShoulderL = LeanTween.rotateLocal(ShoulderL.transform.gameObject, pose.ShoulderL.eulerAngles, duration);
		descrShoulderR = LeanTween.rotateLocal(ShoulderR.transform.gameObject, pose.ShoulderR.eulerAngles, duration);
		descrUpperArmL = LeanTween.rotateLocal(UpperArmL.transform.gameObject, pose.UpperArmL.eulerAngles, duration);
		descrUpperArmR = LeanTween.rotateLocal(UpperArmR.transform.gameObject, pose.UpperArmR.eulerAngles, duration);
		descrForearmL = LeanTween.rotateLocal(ForearmL.transform.gameObject, pose.ForearmL.eulerAngles, duration);
		descrForearmR = LeanTween.rotateLocal(ForearmR.transform.gameObject, pose.ForearmR.eulerAngles, duration);
		descrHandL = LeanTween.rotateLocal(HandL.transform.gameObject, pose.HandL.eulerAngles, duration);
		descrHandR = LeanTween.rotateLocal(HandR.transform.gameObject, pose.HandR.eulerAngles, duration);
	}

	public Part.HighlightDegree Highlighted {
		set {
			Root.Highlighted = value;
			Chest.Highlighted = value;
			Neck.Highlighted = value;
			Head.Highlighted = value;
			ThighL.Highlighted = value;
			ThighR.Highlighted = value;
			ShinL.Highlighted = value;
			ShinR.Highlighted = value;
			FootL.Highlighted = value;
			FootR.Highlighted = value;
			ShoulderL.Highlighted = value;
			ShoulderR.Highlighted = value;
			UpperArmL.Highlighted = value;
			UpperArmR.Highlighted = value;
			ForearmL.Highlighted = value;
			ForearmR.Highlighted = value;
			HandL.Highlighted = value;
			HandR.Highlighted = value;
		}
	}

	public bool EditEnabled {
		set {
			Root.EditEnabled = value;
			Chest.EditEnabled = value;
			Neck.EditEnabled = value;
			Head.EditEnabled = value;
			ThighL.EditEnabled = value;
			ThighR.EditEnabled = value;
			ShinL.EditEnabled = value;
			ShinR.EditEnabled = value;
			FootL.EditEnabled = value;
			FootR.EditEnabled = value;
			ShoulderL.EditEnabled = value;
			ShoulderR.EditEnabled = value;
			UpperArmL.EditEnabled = value;
			UpperArmR.EditEnabled = value;
			ForearmL.EditEnabled = value;
			ForearmR.EditEnabled = value;
			HandL.EditEnabled = value;
			HandR.EditEnabled = value;
		}
	}

	private bool visible = true;
	public bool Visible {

		get {
			return visible;
		}

		set {
			if (visible == value) {
				return;
			} else {
				visible = value;
				transform.localScale = visible ? Vector3.one : Vector3.zero;
			}
		}
	}

	public void DisconnectFromRigidbody() {
		Root.DisconnectFromRigidbody ();
		Chest.DisconnectFromRigidbody ();
		Head.DisconnectFromRigidbody ();
		ThighL.DisconnectFromRigidbody ();
		ThighR.DisconnectFromRigidbody ();
		ShinL.DisconnectFromRigidbody ();
		ShinR.DisconnectFromRigidbody ();
		FootL.DisconnectFromRigidbody ();
		FootR.DisconnectFromRigidbody ();
		ShoulderL.DisconnectFromRigidbody ();
		ShoulderR.DisconnectFromRigidbody ();
		UpperArmL.DisconnectFromRigidbody ();
		UpperArmR.DisconnectFromRigidbody ();
		ForearmL.DisconnectFromRigidbody ();
		ForearmR.DisconnectFromRigidbody ();
		HandL.DisconnectFromRigidbody ();
		HandR.DisconnectFromRigidbody ();
	}

	public void ApplyPreset(Preset preset) {
		Pose pose = preset.Pose;
		if (pose == null) {
			ApplyPose(Pose.DefaultPose());
		} else {
			ApplyPose(pose);
		}

		switch(preset.Type) {

		case Preset.PresetType.Empty:
			EditEnabled = true;
			Highlighted = Part.HighlightDegree.None;
			Visible = false;
			EditEnabled = false;

			break;

		case Preset.PresetType.Static:
		case Preset.PresetType.Animated:
			EditEnabled = true;
			Highlighted = Part.HighlightDegree.None;
			Visible = true;
			EditEnabled = false;
			
			break;
			
		case Preset.PresetType.NewPresetPlaceHolder:
			EditEnabled = true;
			Highlighted = Part.HighlightDegree.Pale;
			Visible = true;
			EditEnabled = false;
			
			break;

		default:
			Debug.LogError("Not implemented");

			break;

		}
	}

	const float intervalCorrection = 0.001953125f;
	public IEnumerator BeginMotion(List<Pose> motion, float interval, bool isForward = true) {

		int from = isForward? 0 : motion.Count-1;
		int to = isForward? motion.Count : -1;
		int inc = isForward? +1 : -1;

		isPlayingAnimation = true;

		for (int i = from; i != to; i += inc) {
			if (isPlayingAnimation == false) {
				yield break;
			}
			Pose pose = motion[i];
			ApplyPose (pose, interval - intervalCorrection);
			yield return new WaitForSeconds(interval - intervalCorrection);
		}

		isPlayingAnimation = false;
	}

	public void StopMotion() {
		isPlayingAnimation = false;

		if (descrRootPosition != null) {
			descrRootPosition.cancel();
			descrRootPosition = null;
		}
		
		if (descrRootRotation != null) {
			descrRootRotation.cancel();
			descrRootRotation = null;
		}
		
		if (descrChest != null) {
			descrChest.cancel();
			descrChest = null;
		}
		
		if (descrNeck != null) {
			descrNeck.cancel();
			descrNeck = null;
		}
		
		if (descrHead != null) {
			descrHead.cancel();
			descrHead = null;
		}
		
		if (descrThighL != null) {
			descrThighL.cancel();
			descrThighL = null;
		}
		
		if (descrThighR != null) {
			descrThighR.cancel();
			descrThighR = null;
		}
		
		if (descrShinL != null) {
			descrShinL.cancel();
			descrShinL = null;
		}
		
		if (descrShinR != null) {
			descrShinR.cancel();
			descrShinR = null;
		}
		
		if (descrFootL != null) {
			descrFootL.cancel();
			descrFootL = null;
		}
		
		if (descrFootR != null) {
			descrFootR.cancel();
			descrFootR = null;
		}
		
		if (descrShoulderL != null) {
			descrShoulderL.cancel();
			descrShoulderL = null;
		}
		
		if (descrShoulderR != null) {
			descrShoulderR.cancel();
			descrShoulderR = null;
		}
		
		if (descrUpperArmL != null) {
			descrUpperArmL.cancel();
			descrUpperArmL = null;
		}
		
		if (descrUpperArmR != null) {
			descrUpperArmR.cancel();
			descrUpperArmR = null;
		}
		
		if (descrForearmL != null) {
			descrForearmL.cancel();
			descrForearmL = null;
		}
		
		if (descrForearmR != null) {
			descrForearmR.cancel();
			descrForearmR = null;
		}
		
		if (descrHandL != null) {
			descrHandL.cancel();
			descrHandL = null;
		}
		
		if (descrHandR != null) {
			descrHandR.cancel();
			descrHandR = null;
		}
	}

}

