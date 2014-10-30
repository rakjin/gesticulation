﻿using UnityEngine;
using System.Collections;

public class Shelf : MonoBehaviour {

	public Transform puppetPrefab;

	const int slotsNum = 3;
	virtual public int SlotsNum {
		get {
			return slotsNum;
		}
	}

	virtual public int CenterSlot {
		get {
			return 1;
		}
	}

	private PresetDataSource presets = new PresetDataSource();

	// Use this for initialization
	void Start () {

		if (puppetPrefab == null) {
			Debug.LogError("puppetPrefab required");

		} else {
			presets.Push (new Preset(Preset.PresetType.Static, Pose.RandomPose00(), "RanPo 00", "Stanton"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.RandomPose01(), "RanPo 01", "Mars"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.RandomPose02(), "RanPo 02", "Greco"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.RandomPose03(), "RanPo 03", "Nguyen"));
			FillSlots ();
		}
	
	}

	virtual public Vector3 GetSlotPosition(int slotNum) {

		return Vector3.zero;

	}


	void FillSlots() {

		for (int i = 0; i < SlotsNum; i++) {

			Transform puppet = (Transform)Instantiate(puppetPrefab, GetSlotPosition(i), Quaternion.identity);
			Poser poser = puppet.gameObject.GetComponent<Poser>();
			poser.Highlighted = Part.HighlightDegree.Pale;
			if (i % 2 == 0) {
				poser.Visible = false;
			}
			if (i == CenterSlot) {
				poser.gameObject.name = "Puppet";
			}

		}

	}

}
