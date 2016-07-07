using UnityEngine;
using System.Collections;
using BehaviorTrees;

public class SmartSword : SmartObject {

	private SmartCharacter owner;

	public SmartSword (string name) {
		this.name = "\"" + name + "\"";
		Constants.PBT_Trace = Constants.PBT_Trace + "An object " + this.name + " / Exists\n";

	}

	public SmartSword (string name, SmartCharacter owner) {
		this.name =  name;

		Constants.PBT_Trace = Constants.PBT_Trace + owner.name + " has-a / " + name + "\n";
		this.relName = name + " -- of -- " + owner.name;
	}
}
