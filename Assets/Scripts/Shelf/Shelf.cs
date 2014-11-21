﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shelf : MonoBehaviour {
	public delegate void TweenComplete();
	public event TweenComplete OnFlipComplete;

	public const float FLIP_DURATION = 0.5f;

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

	private List<Poser> slots = new List<Poser>();
	private PresetDataSource presets = new PresetDataSource();
	private int index = 0;

	// Use this for initialization
	void Awake () {

		OnFlipComplete += () => {};

		if (puppetPrefab == null) {
			Debug.LogError("puppetPrefab required");

		} else {
			presets.Push (new Preset(Preset.PresetType.Static, Pose.PresetPoseThinker(), "생각하는 사람", "로뎅"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.PresetPoseOTL(), "OTL - 좌절금지", "한송이"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.PresetPoseFashionKing(), "패션왕", "주원"));
			presets.Push (new Preset(Preset.PresetType.NewPresetPlaceHolder));
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
			slots.Add (poser);

			int indexRelativeToDataSource = i - (SlotsNum/2);
			Preset preset = presets.Get (indexRelativeToDataSource);

			poser.ApplyPreset(preset);

		}

	}

	public bool FlipLeft() {
		return Flip (toLeft:true);
	}
	
	public bool FlipRight() {
		return Flip (toLeft:false);
	}

	public bool Flip(bool toLeft = true) {

		int desiredIndex = (toLeft? index+1 : index-1);
		if (0 > desiredIndex || desiredIndex >= presets.Count) {
			return false;
		}

		Poser currentPoser = CurrentPoser ();
		Preset currentPreset = CurrentPreset ();
		if (currentPreset.Pose != null) {
			currentPoser.ApplyPose (currentPreset.Pose, 0.25f);
		}

		index = desiredIndex;

		int tweenSlotBeginIndex = 0;
		int tweenSlotEndIndex = 0;
		int lastSlotIndex = SlotsNum - 1;

		if (toLeft) {
			tweenSlotBeginIndex = 1;
			tweenSlotEndIndex = lastSlotIndex;
		} else {
			tweenSlotBeginIndex = 0;
			tweenSlotEndIndex = lastSlotIndex - 1;
		}

		for (int i = tweenSlotBeginIndex; i <= tweenSlotEndIndex; i++) {
			int newIndex = i + (toLeft? -1 : +1);
			Vector3 newPosition = GetSlotPosition(newIndex);
			LTDescr tween = LeanTween.moveLocal(slots[i].gameObject, newPosition, FLIP_DURATION).setEase(LeanTweenType.easeInOutCubic);
			if (i == CenterSlot) {
				tween.setOnComplete(OnCenterSlotFlipComplete);
			}
		}
		
		int poppingSlotIndex = (toLeft? 0 : lastSlotIndex);
		Poser reusedSlot = slots[poppingSlotIndex];
		slots.RemoveAt (poppingSlotIndex);

		int halfSlotsNum = SlotsNum / 2;
		int endPointPresetIndex = index + (toLeft? halfSlotsNum : -halfSlotsNum);
		reusedSlot.ApplyPreset(presets.Get(endPointPresetIndex));

		int pushingSlotIndex = (toLeft? lastSlotIndex : 0);
		slots.Insert (pushingSlotIndex, reusedSlot);

		reusedSlot.transform.localPosition = GetSlotPosition (pushingSlotIndex);

		return true;
	}

	public Preset CurrentPreset() {
		return presets.Get(index);
	}

	public Poser CurrentPoser() {
		return slots[CenterSlot];
	}

	public void InsertPresetBeforeLast(Preset preset) {
		presets.InsertBeforeLast (preset);

		Poser lastSlot = slots [(SlotsNum / 2) + 1];
		lastSlot.ApplyPreset (presets.Get (presets.Count - 1));
	}

	void OnCenterSlotFlipComplete() {
		OnFlipComplete ();
	}
	
}
