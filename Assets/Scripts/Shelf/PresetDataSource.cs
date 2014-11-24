using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PresetDataSource {

	private List<Preset> presets = new List<Preset>();
	private Preset empty = new Preset(Preset.PresetType.Empty);

	public PresetDataSource() {
	}

	public Preset Get(int index) {
		if (index < 0) {
			return empty;
		}

		if (index >= presets.Count) {
			return empty;
		}

		return presets[index];
	}

	public void SetAt(Preset preset, int index) {
		presets [index] = preset;
	}

	public void RemoveAt(int index) {
		presets.RemoveAt (index);
	}

	public int Count {
		get {
			return presets.Count;
		}
	}

	public void Push(Preset item) {
		presets.Add (item);
	}

	public void InsertBeforeLast(Preset item) {
		presets.Insert (presets.Count - 1, item);
	}


	#region JSON
	
	public JSONArray Serialize() {
		JSONArray json = new JSONArray ();
		foreach(Preset preset in presets) {
			if (preset.Type == Preset.PresetType.Static || preset.Type == Preset.PresetType.Animated) {
				json.Add(preset.Serialize());
			}
		}
		return json;
	}
	
	#endregion

}
