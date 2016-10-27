using UnityEngine;
using BehaviorTrees;
using System.Collections;
using System.Collections.Generic;

public class PlanState {

	List<CausalLink> cls = new List<CausalLink>();
	List<Affordance> affs = new List<Affordance> ();
	List<OrderingConstraint> ocs = new List<OrderingConstraint> ();
	string note = "";

	public PlanState (List<CausalLink> cls, List<Affordance> affs, List<OrderingConstraint> ocs) {

		this.cls = cls;
		this.affs = affs;
		this.ocs = ocs;
	}

	public List<CausalLink> GetCausalLinks () {

		return cls;
	}

	public List<Affordance> GetAffordance () {

		return affs;
	}

	public List<OrderingConstraint> GetOrderingConstraints () {

		return ocs;
	}

	public void SetNote (string str) {

		note = str;
	}

	public string GetSummary () {

		return note;
	}
}
