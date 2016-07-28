using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class MergeTraceHelper {

	/*public void GetAgentsWithTrace () {

		string[] traceFiles = System.IO.Directory.GetFiles (Constants.traceFilesPath, "*.txt");
		foreach (string file in traceFiles)
			Debug.Log (file);
	}*/

	public static void MergeMemories () {

		ConvertAllMemoriesTo3P ();
		List<TraceMessage> orderedMsgs = ChronologicalMerge ();


		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = HelperFunctions.GetTraceMsgsAsString (orderedMsgs);
	}

	public static void ConvertAllMemoriesTo3P () {

		foreach (string key in Constants.agentMemories.Keys) {
			for (int i = 0; i < Constants.agentMemories [key].Count; i++) {
				Constants.agentMemories [key] [i].ConvertToThridPerson (key);
			}
		}
	}

	public static List<TraceMessage> ChronologicalMerge () {

		List<TraceMessage> orderedMsgs = new List<TraceMessage> ();
		foreach (string key in Constants.agentMemories.Keys) {
			foreach (TraceMessage msg in Constants.agentMemories[key]) {
				if (!orderedMsgs.Any(tMsg => tMsg.asString() == msg.asString())) {
					orderedMsgs.Add (msg);
				}
			}
		}
		return orderedMsgs.OrderBy (tMsg => tMsg.GetTime ()).ToList ();
	}
}
