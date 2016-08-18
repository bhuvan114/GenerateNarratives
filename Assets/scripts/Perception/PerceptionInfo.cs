using UnityEngine;
using System.Collections;

[System.Serializable]
public class PerceptionInfo {

	VisualPercptionData visionData;
	bool hasVisionData = false;

	public PerceptionInfo () { }

	public PerceptionInfo (PerceptionInfo perInfo) {

		if (perInfo.HasVisionPerception ()) {
			hasVisionData = true;
			visionData = new VisualPercptionData (perInfo.GetVisionData ());
		}
	}

	public VisualPercptionData GetVisionData() {
		
		return visionData;
	}

	public bool HasVisionPerception () {

		return hasVisionData;
	}

	public void SetVisionData (VisualPercptionData viData) {

		visionData = viData;
		hasVisionData = true;
	}

	public string DisplayPerceptionInfo () {

		string info = "[\n";
		if (hasVisionData)
			info = info + "Vision Info : " + visionData.DisplayVisualPerceptionData() + ",\n";
		info = info + "]";
		return info;
	}
}
