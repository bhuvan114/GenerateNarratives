using UnityEngine;
using System.Collections;
using BehaviorTrees;

public class SmartCharacter : SmartObject{

	public SmartCharacter(string name) {
		this.name = "\"" + name + "\"";
		Constants.PBT_Trace = Constants.PBT_Trace + "A man " + this.name + " / Exists\n";
	}
}
