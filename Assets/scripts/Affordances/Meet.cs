using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using BehaviorTrees;

public class Meet : Affordance {

	Vector3 pos;
	float dist = 1;

	public Meet (SmartCharacter afdnt, SmartCharacter afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		base.initialize ();

		//effects.Add (new Condition (affordant.name, Constants.ConditionType.AT, true));
		//effects.Add (new Location (affordant.name, affordee.name));
		//Debug.Log ("Meet pos : " + pos.ToString () + " - " + affordant.name);
		root = this.PBT ();
	}

	public void UpdatePositionAndEffects () {

		pos = affordee.gameObject.transform.position;
		effects.Add (new Location (affordant.name, pos));
	}

	public Node PopulateDestinationPosition () {

		return new LeafInvoke (
			() => this.UpdatePositionAndEffects ());
	}

	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return (new Sequence(this.affordant.gameObject.GetComponent<BehaviorMecanim> ().Node_GoToUpToRadius (pos, dist), this.UpdateAndPublish()));
	}

}
