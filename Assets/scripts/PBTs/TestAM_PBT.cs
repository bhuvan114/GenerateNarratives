using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;
using BehaviorTrees;

public class TestAM_PBT : MonoBehaviour {

	public SmartCharacter daniel;
	public SmartCharacter robert;
	public SmartPlace meetPlace1;
	public SmartPlace meetPlace2;


	// Write your PBT in this funtion. The last node of the PBT should always be this.TerminateTree()
	public Node BuildTree() {


		//Actions
		GoTo danGoes = new GoTo(daniel, meetPlace1);
		GoTo robGoes = new GoTo (robert, meetPlace2);


		//PBT here
		return new Sequence (danGoes.PBT (), robGoes.PBT ());
	}


}
