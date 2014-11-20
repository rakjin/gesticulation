using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Preset {

	public enum PresetType {
		Empty,
		Static,
		Animated,
		NewPresetPlaceHolder,
	}

	private List<Pose> motion;
	public ReadOnlyCollection<Pose> Motion {
		get {
			if (motion == null) {
				return null;
			}
			return motion.AsReadOnly();
		}
	}
	public Pose Pose {
		get {
			if (motion == null || motion.Count == 0) {
				return null;
			}
			return motion[motion.Count-1];
		}
	}
	public string Title { get; private set; }
	public string Author { get; private set; }
	public PresetType Type { get; private set; }

	public Preset(List<Pose> motion, string title = "", string author = "") {
		Type = PresetType.Animated;
		this.motion = motion;
		Title = title;
		Author = author;
	}
	
	public Preset(PresetType type = PresetType.Empty, Pose pose = null, string title = "", string author = "") {
		Type = type;
		if (pose != null) {
			motion = new List<Pose>();
			motion.Add(pose);
		}
		Title = title;
		Author = author;
	}

}
