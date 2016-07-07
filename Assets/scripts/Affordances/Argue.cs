using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class Argue : Affordance {

	public Argue (SmartCharacter afdnt, SmartCharacter afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		name = affordant.name + " / argues / " + affordee.name;

		effects.Add (new Condition (affordee.name, "angry", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return new LeafInvoke (
			() => this.AddSummaryToTrace ());
	}
}
