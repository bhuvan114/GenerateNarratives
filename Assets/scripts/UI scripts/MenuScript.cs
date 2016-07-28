using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void RunSimulation () {

		Constants.StartSimulation = true;
	}

	public void MergeMemories () {

		Constants.MergeMemories = true;
	}

	public void DisplayMergeMemories () {

		MergeTraceHelper.MergeMemories ();
	}
}
