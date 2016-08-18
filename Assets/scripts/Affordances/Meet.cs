using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using BehaviorTrees;

public class Meet : Affordance {

	Val<Vector3> pos;
	float dist = 2;

	public Meet (SmartCharacter afdnt, SmartCharacter afdee) {

		affordant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		base.initialize ();
		pos = Val.V (() => affordee.gameObject.transform.position);
		effects.Add (new Location (affordant.name, affordee.name));
		root = this.PBT ();
	}

	/*public void UpdateEffects () {
		
		effects.Add (new Location (affordant.name, affordant.gameObject.transform.position));
	}

	public Node PopulateEffects () {

		return new LeafInvoke (
			() => this.UpdateEffects ());
	}*/

	public Node PBT(){

		//TODO : If required, animation code has to be written here
		return (new Sequence(this.PublisEventStartMsg(), this.affordant.gameObject.GetComponent<BehaviorMecanim> ().Node_GoToUpToRadius (pos, dist), this.UpdateAndPublishEndMsg()));
	}

}
