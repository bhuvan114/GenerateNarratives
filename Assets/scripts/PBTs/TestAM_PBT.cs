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

		List<Condition> initState = new List<Condition> ();
		//initState.Add(new Location(agents[0].name, places[0].name));
		//initState.Add(new Location(agents[0].name, places[1].name));
		NSM.SetInitialState (initState);

		int j;
		//GoTo go = new GoTo (agents [1], places [1]);
		Meet meetUp = new Meet (agents [0], agents [1]);
		//nodes.Add (go.PBT ());


		//return new Sequence (nodes.ToArray ());

		for (int i = 0; i < 3/*agents.Length*/; i++) {
			j = Random.Range (1, 100) % agents.Length;
			LookAt looksAff = new LookAt (agents [i], places [j]);
			//GoTo goesAff = new GoTo (agents [i], places [i]);

			nodes.Add (looksAff.PBT ());
			//nodes.Add (goesAff.PBT ());
		}
		GoTo goesAff = new GoTo (agents [1], places [1]);
		nodes.Add (goesAff.PBT ());
		goesAff = new GoTo (agents [3], places [2]);
		nodes.Add (goesAff.PBT ());
		nodes.Add (meetUp.PBT ());
		goesAff = new GoTo (agents [5], places [5]);
		nodes.Add (goesAff.PBT ());
		//goesAff = new GoTo (agents [0], places [3]);
		//nodes.Add (goesAff.PBT ());

		//Actions
		//GoTo danGoes = new GoTo(daniel, meetPlace1);
		//GoTo robGoes = new GoTo (robert, meetPlace2);


		//PBT here
		return new Sequence (nodes.ToArray());
	}


}
