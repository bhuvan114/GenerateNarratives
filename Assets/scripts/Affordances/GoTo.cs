using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class GoTo : Affordance {

	Vector3 pos;

	public GoTo (SmartCharacter afdnt, SmartPlace afdee) {

		pos = afdee.GetPosition ();
		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		base.initialize ();

		//effects.Add (new Condition (affordant.name, Constants.ConditionType.AT, true));
		effects.Add (new Location (affordant.name, affordee.name));
		//Debug.Log ("Goto pos : " + pos.ToString () + " - " + affordant.name);
		root = this.PBT ();
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		return (new Sequence(this.PublisEventStartMsg(), this.affordant.gameObject.GetComponent<BehaviorMecanim> ().Node_GoTo (pos), this.UpdateAndPublishEndMsg()));
	}
}
