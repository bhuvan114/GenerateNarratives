using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BehaviorTrees;
using TreeSharpPlus;

public static class MergeTraceHelper {

	static List<EventMemory> chronoNarrative = new List<EventMemory>();

	/*public void GetAgentsWithTrace () {

		string[] traceFiles = System.IO.Directory.GetFiles (Constants.traceFilesPath, "*.txt");
		foreach (string file in traceFiles)
			Debug.Log (file);
	}*/

	public static void MergeMemories () {

		ConvertAllMemoriesTo3P ();
		chronoNarrative = ChronologicalMerge ();


		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = HelperFunctions.GetAgentMemoriesAsString (chronoNarrative);
	}

	public static void ConvertAllMemoriesTo3P () {

		foreach (string key in Constants.agentMemories.Keys) {
			for (int i = 0; i < Constants.agentMemories [key].Count; i++) {
				Constants.agentMemories [key] [i].ConvertToThridPerson (key);
			}
		}
	}

	public static List<EventMemory> ChronologicalMerge () {

		List<EventMemory> orderedMems = new List<EventMemory> ();
		foreach (string key in Constants.agentMemories.Keys) {
			foreach (EventMemory memEve in Constants.agentMemories[key]) {
				if (!orderedMems.Any (tMem => tMem.GetShortMemory () == memEve.GetShortMemory ())) {
					orderedMems.Add (memEve);
				}
			}
		}
		return orderedMems.OrderBy (tMems => tMems.GetTimeStamp ()).ToList ();
	}

	public static Node SetupSimulationEmvironment () {

		List<string> objsInKnowledge = new List<string> ();
		List<Affordance> affs = new List<Affordance> ();

		//Create Affordances & populate agents in knowledge
		foreach (EventMemory msg in chronoNarrative) {
			affs.Add (HelperFunctions.GetAffordanceFromString (msg.GetMessage ()));
			if (!objsInKnowledge.Contains (msg.GetActorOneName ()))
				objsInKnowledge.Add (msg.GetActorOneName ());
			if (!objsInKnowledge.Contains (msg.GetActorTwoName ()))
				objsInKnowledge.Add (msg.GetActorTwoName ());
		}

		//Get the startState
		List<Condition> startState = new List<Condition>();
		bool isValid = ValidateNarrativeAndReturnStartState (affs, out startState);

		//Logs
		Debug.LogError (isValid.ToString ());
		foreach (Condition con in startState)
			Debug.Log (con.asString ());

		//Visualize Start State
		foreach (Condition cond in startState) {
			if (!objsInKnowledge.Contains (cond.getActor ()))
				objsInKnowledge.Add (cond.getActor ());
			if (!string.IsNullOrEmpty (cond.getRealtedActor ()) && !objsInKnowledge.Contains (cond.getRealtedActor ()))
				objsInKnowledge.Add (cond.getRealtedActor ());
		}

		Node treeRoot = null;
		if (isValid) {

			Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
			memoriesTxt.text = memoriesTxt.text + "\nStart State : \n" + HelperFunctions.GetConditionsAsString (startState);

			VisualizeStartState (startState, objsInKnowledge);
			treeRoot = BuildNarrativeTree (affs);

			//BehaviorAgent behaviorAgent = new BehaviorAgent (treeRoot);
			//BehaviorManager.Instance.Register (behaviorAgent);
			//behaviorAgent.StartBehavior ();
		}

		return treeRoot;
	}

	static Node BuildNarrativeTree (List<Affordance> affs) {
		
		Node[] pbt = affs.Select (x => x.GetPBTRoot ()).ToArray ();
		return new Sequence (pbt);

	}

	static void VisualizeStartState (List<Condition> startState, List<string> objs) {

		//foreach (string obj in objs)
		//	Debug.LogWarning (obj);

		foreach (string objName in Constants.smartObjToGameObjMap.Keys) {
			if (!objs.Contains (objName))
				GameObject.Find (Constants.smartObjToGameObjMap [objName]).SetActive (false);
		}

		foreach (Condition state in startState) {
			Visualize.UpdateSceneWithState (state);
		}
	}

	static bool ValidateNarrativeAndReturnStartState(List<Affordance> affs, out List<Condition> systemStartState) {

		systemStartState = new List<Condition> ();
		bool valid = true;

		for (int i = chronoNarrative.Count - 1; i >= 0; i--) {
			// Adding trace to start state
			foreach (Condition cond in chronoNarrative[i].GetActorOneState()) {
				//Debug.Log ("1" + cond.asString ());
				if (!((affs [i].asString ().IndexOf ("meets") != -1) && cond.IsLocation ())) {
					foreach (Condition state in systemStartState) {
						if (cond.IsContradicts (state))
							valid = false;
					}

					if (!systemStartState.Contains (cond)) {
						//Debug.LogError ("+" + cond.asString ());
						systemStartState.Add (cond);
					}
				}
			}

			foreach (Condition cond in chronoNarrative[i].GetActorTwoState()) {
				//Debug.Log ("2" + cond.asString ());
				foreach (Condition state in systemStartState) {
					if (cond.IsContradicts (state))
						valid = false;
				}

				if (!systemStartState.Contains (cond)) {
				//	Debug.LogError ("+" + cond.asString ());
					systemStartState.Add (cond);
				}
			}


			//Verifying with affordance
			//if (affs [i].asString ().IndexOf ("meets")) {
			//	foreach
			//} else {
				foreach (Condition effect in affs[i].GetEffects()) {
					//Debug.Log ("3" + effect.asString ());
					foreach (Condition cond in systemStartState) {
						if (effect.IsContradicts (cond)) {
							valid = false;
						}
					}

					if (systemStartState.Contains (effect)) {
						//	Debug.LogError ("+" + effect.asString ());
						//	systemStartState.Add (effect);
						//} else {
						//	Debug.LogError ("-" + effect.asString ());
						systemStartState.Remove (effect);
					}
				}

				foreach (Condition preCond in affs[i].GetPreConditions()) {
					//Debug.Log ("4" + preCond.asString ());
					for (int j = 0; j < systemStartState.Count; j++) {
						if (preCond.IsContradicts (systemStartState [j])) {
							//	Debug.LogError ("*" + preCond.asString ());
							systemStartState [j] = preCond;
						}
					}

					if (!systemStartState.Contains (preCond)) {
						//Debug.LogError ("+" + preCond.asString ());
						systemStartState.Add (preCond);
					}
				}
			//}
		}
		return valid;
	}

	public static float EstimateTimeTakenForAnEvent (EventMemory memory) {

		float time = 0;
		string[] msg = memory.GetMessage ().Split (new char[]{ ' ' });
		string eventType = msg [1];
		if (eventType.Equals (Constants.AFF_TAGS.meets.ToString ()) || eventType.Equals (Constants.AFF_TAGS.walks_to.ToString ())) {


		}

		return time;
	}

	public static List<Affordance> ConvertEventMemoriesToAffordances (List<EventMemory> memories) {

		List<Affordance> affs = new List<Affordance> ();
		foreach (EventMemory memory in memories)
			affs.Add (HelperFunctions.GetAffordanceFromString (memory.GetMessage ()));
		return affs;
	}

	public static bool ValidateNarrativeCompleteness (List<EventMemory> memories, out List<CausalLink> inCompletes, out List<Condition> startState) {

		bool isComplete = true;
		List<Affordance> affs = ConvertEventMemoriesToAffordances (memories);
		inCompletes = new List<CausalLink> ();
		startState = new List<Condition> ();

		for (int i = memories.Count - 1; i >= 0; i--) {

			foreach (Condition cond in memories[i].GetActorOneState()) {

				for (int j=0; j<startState.Count(); j++) {
					if (cond.Equals (startState [j])) {
						startState.RemoveAt (j);
						break;
					} else if (cond.IsContradicts(startState[j])) {
						startState.RemoveAt (j);
						inCompletes.Add (new CausalLink (affs [i], cond, affs [i + 1]));
						isComplete = false;
						break;
					}
				}
				startState.Add (cond);
			}

			foreach (Condition cond in memories[i].GetActorTwoState()) {

				for (int j=0; j<startState.Count(); j++) {
					if (cond.Equals (startState [j])) {
						startState.RemoveAt (j);
						break;
					} else if (cond.IsContradicts(startState[j])) {
						startState.RemoveAt (j);
						inCompletes.Add (new CausalLink (affs [i], cond, affs [i + 1]));
						break;
					}
				}
				startState.Add (cond);
			}

			// Precondition logic here
		}
		return isComplete;
	}
}
