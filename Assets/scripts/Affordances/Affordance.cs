using UnityEngine;
using System.Linq;
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
		protected string tag = "";

		public string asString () {

			return this.name;
		}

		protected void initialize() {

			tag = Constants.affordanceMap.FirstOrDefault (t => t.Value == this.GetType ()).Key;
			name = affordant.name + " " + tag + " " + affordee.name;
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
			//TraceMessage msg = new TraceMessage (this.asString (), affordant.name, affordee.name);
			string msgWithTime = Time.realtimeSinceStartup.ToString("n2") + " " + this.asString();
			MessageBus.PublishMessage (msgWithTime, affordant.name, affordee.name);
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
