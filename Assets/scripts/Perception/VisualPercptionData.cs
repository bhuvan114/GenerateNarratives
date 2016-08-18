using UnityEngine;
using System.Collections;

[System.Serializable]
public class VisualPercptionData {

	public SerializableVector3 actor1_position, actor2_position;

	public VisualPercptionData (Vector3 actor1_pos, Vector3 actor2_pos) {

		actor1_position = actor1_pos;
		actor2_position = actor2_pos;
	}

	public VisualPercptionData (VisualPercptionData viData) {

		actor1_position = viData.GetActorOnePosition ();
		actor2_position = viData.GetActorTwoPosition ();
	}

	public Vector3 GetActorOnePosition() {
	
		return actor1_position;
	}

	public Vector3 GetActorTwoPosition() {

		return actor2_position;
	}

	public string DisplayVisualPerceptionData () {

		return "{ 'act1' : " + actor1_position.ToString () + "; 'act2' : " + actor2_position.ToString () + " }";
	}
}
