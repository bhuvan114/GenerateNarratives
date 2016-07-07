using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTrees {
	public class Affordance {

		public string name = "Action";
		public SmartObject affordant;
		public SmartObject affordee;
		protected List<Condition> preconditions = new List<Condition>();
		protected List<Condition> effects = new List<Condition>();

		public string asString () {

			return this.name + "\n";
		}

		public string actionSummary () {

			string summary = this.asString ();
			foreach (Condition cond in effects) {
				summary = summary + cond.asString ();
			}
			return summary;	
		}

		public void AddSummaryToTrace () {

			Constants.PBT_Trace = Constants.PBT_Trace + this.actionSummary ();
		}
	}
}
