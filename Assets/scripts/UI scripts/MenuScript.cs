using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TreeSharpPlus;

public class MenuScript : MonoBehaviour {

	public GameObject mergePane;
	bool memoriesMerged = false;

	public void RunSimulation () {

		Constants.StartSimulation = true;
	}

	public void Animate (Node treeRoot) {

		BehaviorAgent behaviorAgent = new BehaviorAgent (treeRoot);
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();

		mergePane.SetActive (false);
	}

	public void ShowStartState () {

		Vector3 oc = new Vector3 (0, 0, 0);
		Debug.Log ("Bleh" + oc.ToString());
		Node treeRoot = MergeTraceHelper.SetupSimulationEmvironment ();

		GameObject mergeButton = GameObject.Find ("MergeContinue");
		Button button = mergeButton.GetComponent<Button> ();
		button.onClick.RemoveAllListeners ();
		button.onClick.AddListener (delegate {
			this.Animate (treeRoot);
		});

		//mergePane.SetActive (false);
	}

	public void MergeMemories () {

		Constants.MergeMemories = true;

	}

	public void DisplayMergeMemories () {

		if (!memoriesMerged) {
			Debug.Log ("Old bleh");
			MergeTraceHelper.MergeMemories ();
			memoriesMerged = true;
		}
		GameObject mergeButton = GameObject.Find ("MergeContinue");
		Button button = mergeButton.GetComponent<Button> ();
		button.onClick.RemoveAllListeners ();
		button.onClick.AddListener (delegate {this.ShowStartState ();});
	}

	public void ResetSelect () {

		Constants.MergeMemories = true;
		memoriesMerged = false;
		mergePane.SetActive (false);
	}
}
