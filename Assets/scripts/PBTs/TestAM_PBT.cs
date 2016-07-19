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

	private BehaviorAgent behaviorAgent;
	private List<Observer> observers;
	// Use this for initialization
	void Start () {

		PopulateObserversInScene ();
		behaviorAgent = new BehaviorAgent (this.BuildTree ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PopulateObserversInScene () {

		Object[] objs = GameObject.FindObjectsOfType (typeof(Observer));
	}

	public Node BuildTree() {


		//Actions
		GoTo danGoes = new GoTo(daniel, meetPlace1);
		GoTo robGoes = new GoTo (robert, meetPlace2);


		//PBT here
		return new Sequence (danGoes.PBT (), robGoes.PBT (), this.TerminateTree());
	}

	public Node TerminateTree () {

		return new LeafInvoke (
			() => MessageBus.SaveTraces ());
	}
}
