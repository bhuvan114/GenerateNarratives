using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

namespace BehaviorTrees {
	public class Affordance {

		public string name = "Action";
		public SmartObject affordant;
		public SmartObject affordee;
		protected List<Condition> preconditions = new List<Condition>();
		protected List<Condition> effects = new List<Condition>();

		public string asString () {

			return this.name;
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

		public void publishMessage () {
			TraceMessage msg = new TraceMessage (this.asString (), affordant.name, affordee.name);
			MessageBus.PublishMessage (this.asString (), affordant.name, affordee.name);
		}

		public Node PublisMsgNode () {

			return new LeafInvoke (
				() => this.publishMessage());
		}

		public bool Equals(Affordance aff) {

			if (aff == null)
				return false;
			else if (this.asString () == aff.asString ())
				return true;
			else
				return false;
		}
	}
}
