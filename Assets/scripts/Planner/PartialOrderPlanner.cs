using UnityEngine;
using BehaviorTrees;
using System;
using System.Collections;
using System.Collections.Generic;

public class PartialOrderPlanner {

	List<CausalLink> cls = new List<CausalLink>();
	List<Affordance> affs = new List<Affordance> ();
	List<OrderingConstraint> ocs = new List<OrderingConstraint> ();
	int planDepth = 0;

	public PartialOrderPlanner(PlanState planState) {

		SetPlanState (planState);
	}

	public void SetPlanState (PlanState planState) {

		this.cls = planState.GetCausalLinks ();
		this.affs = planState.GetAffordance ();
		this.ocs = planState.GetOrderingConstraints ();
	}

	public List<PlanState> planForInconsistency (CausalLink inConLink) {
		List<PlanState> admissiblePlanStates = new List<PlanState> ();
		Stack<Affordance> affStack = new Stack<Affordance> ();
		Stack<List<CausalLink>> clStack = new Stack<List<CausalLink>> ();
		Stack<List<OrderingConstraint>> ocStack = new Stack<List<OrderingConstraint>> ();

		do {
		} while(affStack.Count != 0);

		return admissiblePlanStates;
	}
}
