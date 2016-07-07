using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class Stab : Affordance {

	public Stab (SmartCharacter afdnt, SmartCharacter afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		name = affordant.name + " / stabs / " + affordee.name;

		effects.Add (new Condition (affordee.name, "dead", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return new LeafInvoke (
			() => this.AddSummaryToTrace ());
	}
}
