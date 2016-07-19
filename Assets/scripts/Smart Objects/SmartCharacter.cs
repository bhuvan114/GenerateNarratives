using UnityEngine;
using System.Collections;
using BehaviorTrees;

public class SmartCharacter : SmartObject{

	GoTo goTo;

	public SmartCharacter(string name) {
		this.name = "\"" + name + "\"";
		Constants.PBT_Trace = Constants.PBT_Trace + "A man " + this.name + " / Exists\n";
	}

	public bool Equals(SmartCharacter obj) {
		
		if (name == obj.name)
			return true;

		return false;
	}
}
