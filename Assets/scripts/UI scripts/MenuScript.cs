using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public GameObject mergePane;
	bool memoriesMerged = false;

	public void RunSimulation () {

		Constants.StartSimulation = true;
	}

	public void NewMerge () {

		Debug.Log ("Bleh");
		MergeTraceHelper.SetupSimulationEmvironment ();
		mergePane.SetActive (false);
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
		button.onClick.AddListener (delegate {this.NewMerge ();});
	}

	public void ResetSelect () {

		Constants.MergeMemories = true;
		memoriesMerged = false;
		mergePane.SetActive (false);
	}
}
