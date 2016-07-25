using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Constants {

	public static string PBT_Trace = "";
	public static string traceFilesPath = Application.dataPath + "/traces/";
	public static bool StartSimulation = false;
	public static bool MergeMemories = false;
	public static bool ShowMergePane = false;
	public static List<string> objsWithMemories = new List<string> ();
	public static List<string> objsMergeMemories = new List<string> ();
	public static Dictionary<string, string[]> agentMemories = new Dictionary<string, string[]> ();
}
