﻿using UnityEngine;
using System.Collections;
using BehaviorTrees;

public class SmartPlace :  SmartObject{

	public SmartPlace(string name) {
		this.name = "\"" + name + "\"";
		Constants.PBT_Trace = Constants.PBT_Trace + "A place " + this.name + " / Exists\n";
	}
}