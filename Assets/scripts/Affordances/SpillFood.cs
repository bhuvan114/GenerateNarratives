using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class SpillFood : Affordance {

	public SpillFood (SmartCharacter afdnt, SmartCharacter afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		name = affordant.name + " / spill_food / " + affordee.name;

		effects.Add (new Condition (affordee.name, "angry", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return new LeafInvoke (
			() => this.AddSummaryToTrace ());
	}
}
