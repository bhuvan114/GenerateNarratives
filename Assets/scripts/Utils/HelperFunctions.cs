using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using BehaviorTrees;

public static class HelperFunctions{

	//Retturns the SmartObjects of a type objType in the scene
	public static List<T> GetSmartObjectsOfType<T>(System.Type objType) {

		List<T> objs = new List<T> ();
		objs = GameObject.FindObjectsOfType (objType) as List<T>;
		return objs;
	}

	public static void GenerateNameSmrtObjMap () {

		//List<SmartObject> objs = GameObject.FindGameObjectsWithTag ("SmartObject") as List<SmartObject>;
		//foreach ()
	}

	public static void GetAgentsWithTrace () {

		string[] words;
		Regex regex = new Regex ("(/|\\\\|\\.)+");
		string[] traceFiles = System.IO.Directory.GetFiles (Constants.traceFilesPath, "*.txt");
		foreach (string file in traceFiles) {
			words = regex.Split (file);
			Constants.objsWithMemories.Add(words[words.Length-3]);
		}
	}

	public static string GetAsString(List<string> stringList) {

		string text = "";
		foreach(string sent in stringList)
			text = text + sent + "\n";
		return text;
	}

	public static void PopulateAgentStories () {

		Constants.agentMemories = new Dictionary<string, List<TraceMessage>> ();
		List<TraceMessage> tMsgs;
		foreach (string agent in Constants.objsMergeMemories) {
			tMsgs = new List<TraceMessage> ();
			string agentFilePath = Constants.traceFilesPath + agent + ".txt";
			foreach (string line in System.IO.File.ReadAllLines (agentFilePath))
				tMsgs.Add (HelperFunctions.ConvertToTraceMsg (line));
			Constants.agentMemories.Add (agent, tMsgs);
		}
	}

	public static string GetAgentStoriesAsString () {

		string txt = "";
		foreach (string key in Constants.agentMemories.Keys) {
			txt = txt + key + " : \n";
			foreach (TraceMessage msg in Constants.agentMemories[key])
				txt = txt + " - " + msg.asString() + "\n";
		}
		return txt;
	}

	public static TraceMessage ConvertToTraceMsg (string msg) {

		int spcIndx = msg.IndexOf (" ");
		string timeStr = msg.Substring (0, spcIndx);
		float time = System.Convert.ToSingle (timeStr);
		string msgStr = msg.Substring (spcIndx + 1);
		return new TraceMessage (time, msgStr);
	}

	public static string GetTraceMsgsAsString (List<TraceMessage> tMsgs) {

		string txt = "";
		foreach (TraceMessage tMsg in tMsgs)
			txt = txt + " - " + tMsg.GetMessage () + "\n";
		return txt;
	}
}
