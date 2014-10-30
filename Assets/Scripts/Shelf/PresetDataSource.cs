using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

}
