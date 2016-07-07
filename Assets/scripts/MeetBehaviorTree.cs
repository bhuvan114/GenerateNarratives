using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using BehaviorTrees;

public class MeetBehaviorTree : MonoBehaviour {

	private BehaviorAgent behaviorAgent;

	// Use this for initialization
	void Start () {

		Constants.PBT_Trace = "";
		behaviorAgent = new BehaviorAgent (this.BuildTree ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	
	}

	public void printTrace(){

		Debug.Log (Constants.PBT_Trace);
	}

	public Node printTrace_Node(){
		return new LeafInvoke (
			() => this.printTrace());
	}

	// Behavior Tree here
	public Node BuildTree() {

		//Smart Objects that you are going to use
		SmartCharacter dan = new SmartCharacter("Daniel");
		SmartPlace pizza = new SmartPlace("Pizza Restaurant");
		SmartCharacter john = new SmartCharacter("John");
		SmartSword sword = new SmartSword ("Sword", john);

		//Actions
		GoTo danGoes = new GoTo(dan, pizza);
		GoTo johnGoes = new GoTo(john, pizza);
		Greet danJphn = new Greet (dan, john);
		SpillFood jhnSpill = new SpillFood (john, dan);
		Argue danArgue = new Argue (dan, john);
		DrawSword jhnDraw = new DrawSword (john, sword);
		Stab jhnDan = new Stab (john, dan);

		//PBT here
		return new Sequence (danGoes.PBT (), johnGoes.PBT(), danJphn.PBT(), 
			jhnSpill.PBT(), danArgue.PBT(), jhnDraw.PBT(), jhnDan.PBT(), this.printTrace_Node ());
	}
}
