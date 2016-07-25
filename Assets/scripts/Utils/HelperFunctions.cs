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

		Constants.agentMemories = new Dictionary<string, string[]> ();
		foreach (string agent in Constants.objsMergeMemories) {
			string agentFilePath = Constants.traceFilesPath + agent + ".txt";
			Constants.agentMemories.Add (agent, System.IO.File.ReadAllLines (agentFilePath));
		}
	}

	public static string GetAgentStoriesAsString () {

		string txt = "";
		foreach (string key in Constants.agentMemories.Keys) {
			txt = txt + key + " : \n";
			foreach (string line in Constants.agentMemories[key])
				txt = txt + " - " + line + "\n";
		}
		return txt;
	}
}
