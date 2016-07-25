using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class GameController : MonoBehaviour {

	private BehaviorAgent behaviorAgent;

	// Use this for initialization
	void Start () {
	
		behaviorAgent = new BehaviorAgent (this.BuildTree ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Node BuildTree() {

		//Get your PBT here
		Node PBT = this.gameObject.GetComponent<TestAM_PBT> ().BuildTree ();
		return new Sequence (PBT, this.TerminateTree ());
	}

	public Node TerminateTree () {

		return new LeafInvoke (
			() => MessageBus.SaveTraces ());
	}
}
