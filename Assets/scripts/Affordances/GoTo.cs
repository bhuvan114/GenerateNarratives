using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class GoTo : Affordance {

	public GoTo (SmartCharacter afdnt, SmartPlace afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		name = affordant.name + " / walks_to / " + affordee.name;

		effects.Add (new Condition (affordant.name, "reached", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return new LeafInvoke (
			() => this.AddSummaryToTrace ());
	}
}
