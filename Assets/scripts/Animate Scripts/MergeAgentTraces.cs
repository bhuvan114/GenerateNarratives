using UnityEngine;
using System.Collections;

public class MergeAgentTraces : MonoBehaviour {

	public void GetAgentsWithTrace () {

		string[] traceFiles = System.IO.Directory.GetFiles (Constants.traceFilesPath, "*.txt");
		foreach (string file in traceFiles)
			Debug.Log (file);
	}
}
