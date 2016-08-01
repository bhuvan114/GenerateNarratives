using UnityEngine;
using System.Collections;
using BehaviorTrees;
using TreeSharpPlus;

public class LookAt : Affordance {

	Vector3 pos;

	public LookAt (SmartCharacter afdnt, SmartPlace afdee) {

		pos = afdee.GetPosition ();
		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		base.initialize ();
	}

	//PBT for GoTo affordace goes here
	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return (new Sequence(this.affordant.gameObject.GetComponent<BehaviorMecanim> ().Node_OrientTowards(pos), this.UpdateAndPublish()));
	}
}
