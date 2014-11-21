using UnityEngine;
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

		LeanTween.moveLocal (Root.transform.gameObject, pose.RootPosition, duration);
		LeanTween.rotateLocal (Root.transform.gameObject, pose.RootRotation.eulerAngles, duration);

		LeanTween.rotateLocal(Chest.transform.gameObject, pose.Chest.eulerAngles, duration);
		LeanTween.rotateLocal(Head.transform.gameObject, pose.Head.eulerAngles, duration);

		LeanTween.rotateLocal(ThighL.transform.gameObject, pose.ThighL.eulerAngles, duration);
		LeanTween.rotateLocal(ThighR.transform.gameObject, pose.ThighR.eulerAngles, duration);
		LeanTween.rotateLocal(ShinL.transform.gameObject, pose.ShinL.eulerAngles, duration);
		LeanTween.rotateLocal(ShinR.transform.gameObject, pose.ShinR.eulerAngles, duration);
		LeanTween.rotateLocal(FootL.transform.gameObject, pose.FootL.eulerAngles, duration);
		LeanTween.rotateLocal(FootR.transform.gameObject, pose.FootR.eulerAngles, duration);

		LeanTween.rotateLocal(ShoulderL.transform.gameObject, pose.ShoulderL.eulerAngles, duration);
		LeanTween.rotateLocal(ShoulderR.transform.gameObject, pose.ShoulderR.eulerAngles, duration);
		LeanTween.rotateLocal(UpperArmL.transform.gameObject, pose.UpperArmL.eulerAngles, duration);
		LeanTween.rotateLocal(UpperArmR.transform.gameObject, pose.UpperArmR.eulerAngles, duration);
		LeanTween.rotateLocal(ForearmL.transform.gameObject, pose.ForearmL.eulerAngles, duration);
		LeanTween.rotateLocal(ForearmR.transform.gameObject, pose.ForearmR.eulerAngles, duration);
		LeanTween.rotateLocal(HandL.transform.gameObject, pose.HandL.eulerAngles, duration);
		LeanTween.rotateLocal(HandR.transform.gameObject, pose.HandR.eulerAngles, duration);
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
			ApplyPose (pose, interval);
			yield return new WaitForSeconds(interval + 0.00048828125f);
		}

		isPlayingAnimation = false;
	}

	public void StopMotion() {
		isPlayingAnimation = false;
	}

}

