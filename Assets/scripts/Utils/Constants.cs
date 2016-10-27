﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Constants {

	public static float agentSpeed = 2.0f;
	public static string PBT_Trace = "";
	public static string traceFilesPath = Application.dataPath + "/traces/";
	public static bool StartSimulation = false;
	public static bool MergeMemories = false;
	public static bool ShowMergePane = false;
	public static List<string> objsWithMemories = new List<string> ();
	public static List<string> objsMergeMemories = new List<string> ();
	public static Dictionary<string, List<EventMemory>> agentMemories = new Dictionary<string, List<EventMemory>> ();

	public enum AFF_TAGS {
		walks_to,
		looks_at,
		meets
	};

	public static Dictionary<AFF_TAGS, System.Type> affordanceMap = new Dictionary<AFF_TAGS, System.Type> {

		{ AFF_TAGS.walks_to, typeof(GoTo) },
		{ AFF_TAGS.looks_at, typeof(LookAt) },
		{ AFF_TAGS.meets, typeof(Meet) },
	};

	public enum ConditionType {
		AT,
		IS_ALIVE
	};

	public static List<string> visualizableAttributes = new List<string> {
		ConditionType.AT.ToString ()
	};

	public static Dictionary<System.Type, List<string>> objectsOfTypeMap = new Dictionary<Type, List<string>> ();
	public static Dictionary<string, string> smartObjToGameObjMap = new Dictionary<string, string> ();
	public static Dictionary<string, string> gameObjToSmartObjMap = new Dictionary<string, string> ();
}
