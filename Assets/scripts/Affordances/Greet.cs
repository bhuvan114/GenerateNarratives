using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class Greet : Affordance {

	public Greet (SmartCharacter afdnt, SmartCharacter afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		name = affordant.name + " / greets / " + affordee.name;

		effects.Add (new Condition (affordee.name, "happy", true));
		effects.Add (new Condition (affordant.name, "happy", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return new LeafInvoke (
			() => this.AddSummaryToTrace ());
	}
}
