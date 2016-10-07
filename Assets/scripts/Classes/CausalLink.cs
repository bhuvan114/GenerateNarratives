using UnityEngine;
using System.Collections;

namespace BehaviorTrees {
	public class CausalLink : System.IEquatable<CausalLink> {

		Affordance aff1, aff2;
		Condition cond;

		public Affordance GetAffordanceOne () {

			return aff1;
		}

		public Affordance GetAffordanceTwo () {

			return aff2;
		}

		public Condition GetCondition () {

			return cond;
		}

		public CausalLink (Affordance aff1, Condition cond, Affordance aff2) {

			this.aff1 = aff1;
			this.aff2 = aff2;
			this.cond = cond;
		}

		public string asString () {

			string str = aff1.asString () + " + " + cond.asString () + " + " + aff2.asString ();
			return str;
		}

		public bool Equals (CausalLink cl) {

			return (aff1.Equals (cl.GetAffordanceOne ()) && aff2.Equals (cl.GetAffordanceTwo ()) && cond.Equals (cl.GetCondition ()));
		}
	}
}
