using UnityEngine;
using BehaviorTrees;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MergeMemories {

	List<EventMemory> memories = new List<EventMemory> ();

	List<EventMemory> startEventMemories = new List<EventMemory> ();
	List<EventMemory> endEventMemories = new List<EventMemory> ();
	List<EventMemory> estimatedStartEventMemories = new List<EventMemory> ();

	Dictionary<string, List<EventMemory>> ConvertAllMemoriesTo3P (Dictionary<string, List<EventMemory>> agentMemories) {
	
		foreach (string key in agentMemories.Keys) {
			for (int i = 0; i < agentMemories [key].Count; i++) {
				agentMemories [key] [i].ConvertToThridPerson (key);
			}
		}
		return agentMemories;
	}

	void PairCompleteEvents () {

		foreach (EventMemory startEventMemory in startEventMemories) {
			foreach (EventMemory endEventMemory in endEventMemories) {
				if (startEventMemory.Completes (endEventMemory)) {
					endEventMemory.SetStartTime (startEventMemory.GetTimeStamp ());
					startEventMemory.SetEndTime (endEventMemory.GetTimeStamp ());
					break;
				}
			}
		}
	}

	void CatagorizeEventMemories () {

		foreach (EventMemory memory in memories)
			if (memory.IsStartEvent ())
				startEventMemories.Add (memory);
			else
				endEventMemories.Add (memory);
	}

	void EstimateAndPopulateEndTimesOfEvents () {

		foreach (EventMemory memory in startEventMemories) {
			if (memory.GetEndTime () == -1) {
				float timeToFinish;
				switch (memory.GetEventTag ()) {
				case Constants.AFF_TAGS.looks_at:
					timeToFinish = 0.1f;
					memory.SetEstimatedTime (memory.GetTimeStamp () + timeToFinish);
					break;
				case Constants.AFF_TAGS.walks_to:
					timeToFinish = HelperFunctions.GetTimeToTravel (memory.GetPerceptionData ().GetVisionData ().actor1_position, memory.GetPerceptionData ().GetVisionData ().actor2_position);
					Debug.LogError ("Time to finish : " + timeToFinish);
					memory.SetEstimatedTime (memory.GetTimeStamp () + timeToFinish);
					break;
				case Constants.AFF_TAGS.meets:
					timeToFinish = HelperFunctions.GetTimeToTravel (memory.GetPerceptionData ().GetVisionData ().actor1_position, memory.GetPerceptionData ().GetVisionData ().actor2_position);
					Debug.LogError ("Time to finish : " + timeToFinish);
					memory.SetEstimatedTime (memory.GetTimeStamp () + timeToFinish);
					break;
				default :
					break;
				}
				endEventMemories.Add (memory.CreateCounterPartEstimate ());
			}
		}
	}

	Vector3 GetLastKnownPositionOfActor(string actorName, List<EventMemory> memories, int memIndx, out float minStartTime) {

		minStartTime = 0f;
		for (int i = memIndx - 1; i >= 0; i--) {

			if (actorName.Equals (memories [i].GetActorOneName ())) {
				minStartTime = memories [i].GetTimeStamp ();
				return memories [i].GetPerceptionData ().GetVisionData ().actor1_position;
			} else if (actorName.Equals (memories [i].GetActorTwoName ())) {
				minStartTime = memories [i].GetTimeStamp ();
				return memories [i].GetPerceptionData ().GetVisionData ().actor2_position;
			}
		}
		Debug.Log (actorName);
		return GameObject.Find (Constants.smartObjToGameObjMap [actorName]).transform.position;
	}

	void EstimateAndPopulateStartTimesOfEvents () {

		foreach (EventMemory memory in endEventMemories) {
			if (memory.GetStartTime () == -1) {
				float timeToFinish, minStartTime;
				Vector3 actorOnePos;
				switch (memory.GetEventTag ()) {
				case Constants.AFF_TAGS.looks_at:
					timeToFinish = 0.1f;
					memory.SetEstimatedTime (memory.GetTimeStamp () - timeToFinish);
					break;
				case Constants.AFF_TAGS.walks_to:
					actorOnePos = GetLastKnownPositionOfActor (memory.GetActorOneName (), endEventMemories, endEventMemories.IndexOf (memory), out minStartTime);
					timeToFinish = HelperFunctions.GetTimeToTravel (actorOnePos, memory.GetPerceptionData ().GetVisionData ().actor2_position);
					Debug.LogError ("Time to finish : " + timeToFinish);
					memory.SetEstimatedTime (memory.GetTimeStamp () - timeToFinish, minStartTime);
					break;
				case Constants.AFF_TAGS.meets:
					actorOnePos = GetLastKnownPositionOfActor (memory.GetActorOneName (), endEventMemories, endEventMemories.IndexOf (memory), out minStartTime);
					timeToFinish = HelperFunctions.GetTimeToTravel (actorOnePos, memory.GetPerceptionData ().GetVisionData ().actor2_position);
					Debug.LogError ("Time to finish : " + timeToFinish);
					memory.SetEstimatedTime (memory.GetTimeStamp () - timeToFinish, minStartTime);
					break;
				default :
					break;
				}
				startEventMemories.Add (memory.CreateCounterPartEstimate ());
			}
		}
	}

	public MergeMemories (Dictionary<string, List<EventMemory>> agentMemories) {

		agentMemories = ConvertAllMemoriesTo3P (agentMemories);
		foreach (string key in agentMemories.Keys) {
			foreach (EventMemory memory in agentMemories[key]) {
				if (!memories.Contains (memory)) {
					memories.Add (memory);
				}
			}
		}
	}

	public List<EventMemory> GetAllMemories () {

		return memories;
	}

	public List<EventMemory> GetStartEventMemories () {

		return startEventMemories;
	}

	public List<EventMemory> GetEndEventMemories () {

		return endEventMemories;
	}

	public void CategorizeStartAndEndEvents () {

		CatagorizeEventMemories ();
		PairCompleteEvents ();
	}


	public void EstimateMissingTimes () {

		startEventMemories = startEventMemories.OrderBy (mems => mems.GetTimeStamp ()).ToList ();
		endEventMemories = endEventMemories.OrderBy (mems => mems.GetTimeStamp ()).ToList ();

		EstimateAndPopulateEndTimesOfEvents ();
		EstimateAndPopulateStartTimesOfEvents ();

	}

	public string CheckNarrativeCompleteness () {

		foreach (EventMemory memory in startEventMemories)
			endEventMemories.Add (memory);
		endEventMemories = endEventMemories.OrderBy (mems => mems.GetEndTime ()).ToList ();

		return "";
	}
}
