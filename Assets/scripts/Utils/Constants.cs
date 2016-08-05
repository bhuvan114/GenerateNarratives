using UnityEngine;
using System;
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
	public static Dictionary<string, List<TraceMessage>> agentMemories = new Dictionary<string, List<TraceMessage>> ();

	public enum AFF_TAGS {
		walks_to,
		looks_at
	};

	public static Dictionary<AFF_TAGS, System.Type> affordanceMap = new Dictionary<AFF_TAGS, System.Type> {

		{ AFF_TAGS.walks_to, typeof(GoTo) },
		{ AFF_TAGS.looks_at, typeof(LookAt) }
	};

	public enum ConditionType {
		AT,
		IS_ALIVE
	};

	public static List<string> visualizableAttributes = new List<string> {
		ConditionType.AT.ToString ()
	};

	public static Dictionary<string, string> smartObjToGameObjMap = new Dictionary<string, string>();
}
