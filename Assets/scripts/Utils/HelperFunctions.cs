﻿using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
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
		string[] traceFiles = System.IO.Directory.GetFiles (Constants.traceFilesPath, "*.bin");
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

		Constants.agentMemories = new Dictionary<string, List<EventMemory>> ();
		List<EventMemory> mems;
		foreach (string agent in Constants.objsMergeMemories) {
			mems = new List<EventMemory> ();
			string agentFilePath = Constants.traceFilesPath + agent + ".bin";
			//deserialize
			using (System.IO.Stream stream = System.IO.File.Open(agentFilePath, System.IO.FileMode.Open))
			{
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

				mems = (List<EventMemory>)bformatter.Deserialize(stream);
			}
			//foreach (string line in System.IO.File.ReadAllLines (agentFilePath))
			//	tMsgs.Add (HelperFunctions.ConvertToTraceMsg (line));
			Constants.agentMemories.Add (agent, mems);
		}
	}

	public static string GetAgentStoriesAsString () {

		string txt = "";
		foreach (string key in Constants.agentMemories.Keys) {
			txt = txt + key + " : \n";
			foreach (EventMemory memEve in Constants.agentMemories[key])
				txt = txt + " - " + memEve.GetMessageWithMemoryType () + "\n";
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

	public static string GetAgentMemoriesAsString (List<EventMemory> tMems) {

		string txt = "";
		foreach (EventMemory tMem in tMems) {
			txt = txt + " - " + tMem.GetMessage () + "(" + tMem.GetStartTime () + "," + tMem.GetEndTime () + ")\n";
		}
		return txt;
	}

	public static Condition ConvertStringToCondition (string condStr) {

		Condition cond;


		string[] words = condStr.Split (new char[] { ' ' });
		string actorOne = words [0];
		Constants.ConditionType condType = (Constants.ConditionType)System.Enum.Parse (typeof(Constants.ConditionType), words [1]);
		if (words.Length > 4) {

			//Dave AT (2.0, 0.1, 10.2) True;
			int strtIndx, endIndx, len;
			strtIndx = condStr.IndexOf ('(');
			endIndx = condStr.IndexOf (')');
			len = endIndx - strtIndx + 1;
			Vector3 pos = ConvertStringToVector3 (condStr.Substring (strtIndx, len));
			if (condType == Constants.ConditionType.AT) {
				cond = new Location (actorOne, pos);
			} else {
				cond = new Condition (actorOne, condType, pos.ToString (), System.Convert.ToBoolean (words [words.Length - 1]));
			}

			//Debug.LogError ("In str to cond : " + cond.asString ());

		} else if (words.Length == 4) {
			
			string relActor = words [2];
			if (condType == Constants.ConditionType.AT)
				cond =  new Location (actorOne, relActor);
			else
				cond = new Condition (actorOne, condType, relActor, System.Convert.ToBoolean (words [3]));
		} else {

			cond = new Condition (actorOne, condType, System.Convert.ToBoolean (words [2]));
		}
		//Debug.Log ("Convert : " + condStr + " - " + cond.asString ());
		return cond;
	}

	public static List<Condition> GetStateFromStateString (string stateStr) {

		List<Condition> conds = new List<Condition> ();
		if (stateStr [0] == '+')
			stateStr = stateStr.Substring (1);
		if (stateStr.Length > 0) {
			string[] condStrs = stateStr.Split (new char[] { ';' });
			for (int i = 0; i < condStrs.Length; i++) {
				if (!condStrs [i].Contains (";") && condStrs [i].Length > 0)
					conds.Add (ConvertStringToCondition (condStrs [i]));
			}
		}
		return conds;
	}

	public static void CreateSmartObjectAndGameObjectMap () {

		Constants.smartObjToGameObjMap = new Dictionary<string, string> ();
		Constants.gameObjToSmartObjMap = new Dictionary<string, string> ();

		IEnumerable<System.Type> objTypes = System.Reflection.Assembly.GetExecutingAssembly ().GetTypes ()
			.Where (t => t.BaseType != null && t.BaseType == typeof(BehaviorTrees.SmartObject));

		foreach (System.Type objType in objTypes) {

			SmartObject[] smtObjs = GameObject.FindObjectsOfType (objType) as SmartObject[];
			List<string> objNames = new List<string> ();
			foreach (SmartObject obj in smtObjs) {
				objNames.Add (obj.name);
				Constants.smartObjToGameObjMap.Add (obj.name, obj.gameObject.name);
				Constants.gameObjToSmartObjMap.Add (obj.gameObject.name, obj.name);
			}
			Constants.objectsOfTypeMap.Add (objType, objNames);
		}
	}

	public static Affordance GetAffordanceFromString (string affStr) {

		string[] words = affStr.Split (new char[] { ' ' });
		GameObject actorOne = GameObject.Find (Constants.smartObjToGameObjMap [words [0]]);
		GameObject actorTwo = GameObject.Find (Constants.smartObjToGameObjMap [words [2]]);
		Constants.AFF_TAGS affTag = (Constants.AFF_TAGS)System.Enum.Parse (typeof(Constants.AFF_TAGS), words [1]);
		System.Type affType = Constants.affordanceMap [affTag];

		ConstructorInfo[] constrts = affType.GetConstructors ();

		if ((actorOne != null) && (actorTwo != null)) {
			foreach (ConstructorInfo constrt in constrts) {
				ParameterInfo[] pInfo = affType.GetConstructors () [0].GetParameters ();
				if ((actorOne.GetComponent (pInfo [0].ParameterType) != null) && (actorTwo.GetComponent (pInfo [1].ParameterType) != null)) {
					Affordance aff = (Affordance)System.Activator.CreateInstance (Constants.affordanceMap [affTag], actorOne.GetComponent (pInfo [0].ParameterType), actorTwo.GetComponent (pInfo [1].ParameterType));
					return aff;
				}
			}
		}
		throw new System.Exception ("No affordance '" + words[1] + "' with the given characters!!");
	}

	public static string GetConditionsAsString (List<Condition> conds) {

		string txt = "";
		foreach (Condition cond in conds)
			txt = txt + cond.asString() + "\n";

		return txt;
	}

	public static float GetTimeToTravel (Vector3 start, Vector3 destination) {

		NavMeshPath path = new NavMeshPath ();
		if (NavMesh.CalculatePath (start, destination, NavMesh.AllAreas, path)) {

			if (path.corners.Length < 2)
				return 0;
			Vector3 previousCorner = path.corners[0];
			float lengthSoFar = 0.0F;
			int i = 1;
			while (i < path.corners.Length) {
				Vector3 currentCorner = path.corners[i];
				lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
				previousCorner = currentCorner;
				i++;
			}
			Debug.LogError (lengthSoFar + " / " + Constants.agentSpeed);
			return (lengthSoFar / Constants.agentSpeed);
		} else {
			return 0;
		}
	}

	public static Vector3 ConvertStringToVector3(string positionString) {

		Vector3 pos = new Vector3 (0, 0, 0);
		string[] words = positionString.Split (new char[]{ ',' });
		pos.x = System.Convert.ToSingle (words [0].Substring (1));
		pos.y = System.Convert.ToSingle (words [1].Substring (1));
		pos.z = System.Convert.ToSingle (words [2].Substring (1, words[2].Length-2));
		return pos;
	}
}
