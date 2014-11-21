using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Preset {

	public enum PresetType {
		Empty,
		Static,
		Animated,
		NewPresetPlaceHolder,
	}
	
	public List<Pose> Motion { get; private set; }
	public Pose Pose {
		get {
			if (Motion == null || Motion.Count == 0) {
				return null;
			}
			return Motion[Motion.Count-1];
		}
	}
	public string Title { get; private set; }
	public string Author { get; private set; }
	public PresetType Type { get; private set; }

	public Preset(List<Pose> motion, string title = "", string author = "") {
		Type = PresetType.Animated;
		Motion = motion;
		Title = title;
		Author = author;
	}
	
	public Preset(PresetType type = PresetType.Empty, Pose pose = null, string title = "", string author = "") {
		Type = type;
		if (pose != null) {
			Motion = new List<Pose>();
			Motion.Add(pose);
		}
		Title = title;
		Author = author;
	}


	#region JSON

	public JSONClass Serialize() {
		JSONArray motion = new JSONArray ();
		foreach(Pose pose in Motion) {
			motion.Add (pose.Serialize());
		}

		JSONClass json = new JSONClass ();
		json.Add ("title", new JSONData(Title));
		json.Add ("author", new JSONData(Author));
		json.Add ("motion", motion);
		
		return json;
	}

	#endregion

}
