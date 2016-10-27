using UnityEngine;
using BehaviorTrees;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class NarrativePredictor {


	List<CausalLink> cls;
	List<CausalLink> inconsistencies;
	List<OrderingConstraint> orderingConstraints = new List<OrderingConstraint> ();
	List<Affordance> affordances = new List<Affordance> ();
	List<Affordance> availableAffordances = new List<Affordance> ();



	void SetUp (List<EventMemory> startMems) {

		foreach (EventMemory mem in startMems) {
			Affordance aff = mem.GetAffordance ();
			affordances.Add (aff);
			OrderingConstraint oc = new OrderingConstraint (affordances [0], aff);
			if (!orderingConstraints.Contains (oc)) {
				orderingConstraints.Add (oc);
			}
		}

		for (int i = 0; i < startMems.Count (); i++) {
			for (int j = 0; j < startMems.Count (); i++) {
				if (startMems [i].GetEndTime () < startMems [j].GetStartTime ()) {
					OrderingConstraint oc = new OrderingConstraint (affordances [i+1], affordances [j+1]);
					if (!orderingConstraints.Contains (oc))
						orderingConstraints.Add (oc);
				}
			}
		}
	}

	public NarrativePredictor (List<EventMemory> startMemories, List<CausalLink> cls, List<CausalLink> inCons, Affordance startAff) {

		this.cls = cls;
		this.inconsistencies = inCons;
		affordances.Add (startAff);
		SetUp (startMemories);
	}
}
