using UnityEngine;
using BehaviorTrees;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class NarrativeValidator {

	List<EventMemory> startMemories, endMemories;

	List<Condition> startState;
	List<CausalLink> inConsistentLinks = new List<CausalLink> ();
	Affordance startAff;
	List<CausalLink> cls = new List<CausalLink> ();
	List<Tuple<Affordance, Condition>> openConds = new List<Tuple<Affordance, Condition>> ();
	bool isNarrativeConsistent = false;

	void CalculatePossibleStartState () {

		Debug.Log ("Calculate possible start state");

		startState = new List<Condition> ();

		Dictionary<string, EventMemory> firstOccuranceOfObject = new Dictionary<string, EventMemory> ();
		foreach (EventMemory mem in endMemories) {

			if (!mem.IsEstimatedMemory ()) {
				if (firstOccuranceOfObject.ContainsKey (mem.GetActorOneName ())) {
					if (firstOccuranceOfObject [mem.GetActorOneName ()].GetTimeStamp () > mem.GetTimeStamp ()) {
						firstOccuranceOfObject [mem.GetActorOneName ()] = mem;
					}
				} else {
					firstOccuranceOfObject.Add (mem.GetActorOneName (), mem);
				}

				if (firstOccuranceOfObject.ContainsKey (mem.GetActorTwoName ())) {
					if (firstOccuranceOfObject [mem.GetActorTwoName ()].GetTimeStamp () > mem.GetTimeStamp ()) {
						firstOccuranceOfObject [mem.GetActorTwoName ()] = mem;
					}
				} else {
					firstOccuranceOfObject.Add (mem.GetActorTwoName (), mem);
				}
			}
		}

		foreach (EventMemory mem in startMemories) {

			if (!mem.IsEstimatedMemory ()) {
				if (firstOccuranceOfObject.ContainsKey (mem.GetActorOneName ())) {
					if (firstOccuranceOfObject [mem.GetActorOneName ()].GetTimeStamp () > mem.GetTimeStamp ()) {
						firstOccuranceOfObject [mem.GetActorOneName ()] = mem;
					}
				} else {
					firstOccuranceOfObject.Add (mem.GetActorOneName (), mem);
				}

				if (firstOccuranceOfObject.ContainsKey (mem.GetActorTwoName ())) {
					if (firstOccuranceOfObject [mem.GetActorTwoName ()].GetTimeStamp () > mem.GetTimeStamp ()) {
						firstOccuranceOfObject [mem.GetActorTwoName ()] = mem;
					}
				} else {
					firstOccuranceOfObject.Add (mem.GetActorTwoName (), mem);
				}
			}
		}


		foreach (string actor in firstOccuranceOfObject.Keys) {

			if (firstOccuranceOfObject [actor].IsStartEvent ()) {
				if (actor == firstOccuranceOfObject [actor].GetActorOneName ()) {
					foreach (Condition cond in firstOccuranceOfObject[actor].GetActorOneState())
						if (!startState.Contains (cond))
							startState.Add (cond);
				}

				if (actor == firstOccuranceOfObject [actor].GetActorTwoName ()) {
					foreach (Condition cond in firstOccuranceOfObject[actor].GetActorTwoState())
						if (!startState.Contains (cond))
							startState.Add (cond);
				}
			}	
		}
	}

	void AddEffectsToOpenConds (Affordance aff) {

		foreach (Condition effect in aff.GetEffects()) {
			for (int i = 0; i < openConds.Count (); i++) {
				if (effect.Equals (openConds [i].Item2)) {
					openConds.RemoveAt (i);
					i--;
				} else if (effect.IsContradicts (openConds [i].Item2)) {
					openConds.RemoveAt (i);
					i--;
				}
			}
			openConds.Add (new Tuple<Affordance, Condition> (aff, effect));
		}
	}

	void VerifyMemoryStateWithOpenConds (EventMemory mem, Affordance aff) {

		if (!mem.IsEstimatedMemory ()) {
			List<Condition> memState = mem.GetActorOneState ();
			memState = memState.Union (mem.GetActorTwoState ()).ToList ();
			foreach (Condition cond in memState) {
				bool condExists = false;
				foreach (Tuple<Affordance, Condition> openCond in openConds) {
					if (cond.Equals (openCond.Item2)) {
						condExists = true;
						break;
					} else if (cond.IsContradicts (openCond.Item2)) {
						CausalLink cl = new CausalLink (openCond.Item1, cond, aff);
						condExists = true;
						if (!inConsistentLinks.Contains (cl))
							inConsistentLinks.Add (cl);
						break;
					}
				}
				if (!condExists) {
					inConsistentLinks.Add(new CausalLink(startAff, cond, aff));
				}
			}
		}
	}

	void UpdateOpenCondsWithPreConds (Affordance aff) {

		foreach (Condition cond in aff.GetPreConditions()) {
			bool condExists = false;
			for (int i = 0; i < openConds.Count (); i++) {
				if (cond.Equals (openConds [i].Item2)) {
					condExists = true;
					cls.Add (new CausalLink (openConds [i].Item1, openConds [i].Item2, aff));
					break;
				} else if (cond.IsContradicts (openConds [i].Item2)) {
					CausalLink cl = new CausalLink (openConds [i].Item1, cond, aff);
					condExists = true;
					if (!inConsistentLinks.Contains (cl))
						inConsistentLinks.Add (cl);
					break;
				}
			}
			if (!condExists) {
				inConsistentLinks.Add(new CausalLink(startAff, cond, aff));
			}
		}
	}

	void CheckNarrativeConsistency () {

		isNarrativeConsistent = true;

		int startCount, endCount, i = 0, j = 0;
		startCount = startMemories.Count ();
		endCount = endMemories.Count ();
		startAff = new StartState (startState);
		AddEffectsToOpenConds (startAff);
		while (i < startCount && j < endCount) {
			Debug.Log (i.ToString () + " - " + j.ToString ());
			if (startMemories [i].GetTimeStamp () < endMemories [j].GetTimeStamp ()) {
				//CODE
				Debug.Log("start : " + startMemories[i].GetMessage());
				VerifyMemoryStateWithOpenConds(startMemories[i], startMemories[i].GetAffordance());
				UpdateOpenCondsWithPreConds (startMemories [i].GetAffordance ());
				i++;
			} else {
				// CODE
				Debug.Log("end : " + endMemories[j].GetMessage());
				AddEffectsToOpenConds(endMemories[j].GetAffordance());
				j++;
			}
		}

		while (j < endCount) {
			AddEffectsToOpenConds (endMemories [j].GetAffordance ());
			j++;
		}

		if (inConsistentLinks.Count () > 0)
			isNarrativeConsistent = false;
	}

	public bool IsNarrativeConsistent () {

		if (isNarrativeConsistent == false)
			CheckNarrativeConsistency ();
		return isNarrativeConsistent;
	}

	public List<Condition> GetStartState() {

		Debug.Log ("Get start state");
		if (startState == null)
			CalculatePossibleStartState ();

		return startState;
	}

	public List<CausalLink> GetInconsistencies () {

		return inConsistentLinks;
	}

	public NarrativeValidator(List<EventMemory> startMems, List<EventMemory> endMems) {

		startMemories = startMems.OrderBy (tMems => tMems.GetTimeStamp ()).ToList ();
		endMemories = endMems.OrderBy (tMems => tMems.GetTimeStamp ()).ToList ();
	}
}
