﻿using UnityEngine;
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

		effects.Add (new Condition (affordant.name, "reached", true));
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return (new Sequence(this.affordant.gameObject.GetComponent<BehaviorMecanim> ().Node_GoTo (pos), this.PublisMsgNode()));
	}
}
