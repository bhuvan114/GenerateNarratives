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

		SmartCharacter[] agents = GameObject.FindObjectsOfType<SmartCharacter> ();
		SmartPlace[] places = GameObject.FindObjectsOfType<SmartPlace> ();
		List<Node> nodes = new List<Node> ();

		int j;
		for (int i = 0; i < agents.Length; i++) {
			j = Random.Range (1, 100) % agents.Length;
			LookAt looksAff = new LookAt (agents [i], places [j]);
			GoTo goesAff = new GoTo (agents [i], places [i]);

			nodes.Add (looksAff.PBT ());
			nodes.Add (goesAff.PBT ());
		}
		//Actions
		//GoTo danGoes = new GoTo(daniel, meetPlace1);
		//GoTo robGoes = new GoTo (robert, meetPlace2);


		//PBT here
		return new SequenceShuffle (nodes.ToArray());
	}


}
