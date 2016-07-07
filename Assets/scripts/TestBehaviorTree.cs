using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class TestBehaviorTree : MonoBehaviour {

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

		return new Sequence (this.Node_func1(), this.Node_func2(), this.Node_func3());
	}

	public Node Node_func1 () {

		return new LeafInvoke (
			() => this.func_1());
	}

	public Node Node_func2 () {

		return new LeafInvoke (
			() => this.func_2());
	}

	public Node Node_func3 () {

		return new LeafInvoke (
			() => this.behaviorAgent.StopBehavior());
	}

	public void func_1 () {
		Debug.LogError ("Function 1");
	}

	public void func_2 () {
		Debug.LogError ("Function 2");
	}
}
