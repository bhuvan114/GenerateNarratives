using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class DrawSword : Affordance {

	public DrawSword (SmartCharacter afdnt, SmartSword afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		name = affordant.name + " / draws / " + affordee.name;

		effects.Add (new Condition (affordee.relName, "drawn", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return new LeafInvoke (
			() => this.AddSummaryToTrace ());
	}
}
