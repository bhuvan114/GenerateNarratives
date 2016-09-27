using UnityEngine;
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

	void RemoveDuplicateEndEvents () {

		for (int i = 0; i < endEventMemories.Count; i++) {
			foreach (EventMemory startEventMemory in startEventMemories) {
				if (endEventMemories [i].Completes (startEventMemory)) {
					endEventMemories.RemoveAt (i);
					i--;
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
		RemoveDuplicateEndEvents ();
	}
}
