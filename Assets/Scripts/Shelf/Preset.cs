using UnityEngine;
using System.Collections;

public class Preset {

	public enum PresetType {
		Empty,
		Static,
		Animated,
		NewPresetPlaceHolder,
	}

	public Pose Pose { get; private set; }
	public string Title { get; private set; }
	public string Author { get; private set; }
	public PresetType Type { get; private set; }

	public Preset(PresetType type = PresetType.Empty, Pose pose = null, string title = "", string author = "") {
		Type = type;
		Pose = pose;
		Title = title;
		Author = author;
	}

}
