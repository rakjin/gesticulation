using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private List<Poser> slots = new List<Poser>();
	private PresetDataSource presets = new PresetDataSource();
	private int index = 0;

	// Use this for initialization
	void Awake () {

		if (puppetPrefab == null) {
			Debug.LogError("puppetPrefab required");

		} else {
			presets.Push (new Preset(Preset.PresetType.Static, null, "Default", "Noname"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.PresetPoseHaveAQuestion(), "선생님 질문있어요", "김고삼"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.PresetPoseOTL(), "OTL", "곰"));
			presets.Push (new Preset(Preset.PresetType.Static, Pose.PresetPoseThinker(), "생각하는 사람", "Rodin"));
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
			LeanTween.moveLocal(slots[i].gameObject, newPosition, .5f).setEase(LeanTweenType.easeInOutCubic);
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
	
}
