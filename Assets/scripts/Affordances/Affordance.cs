using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;
using BehaviorTrees;

namespace BehaviorTrees {
	public class Affordance : System.IEquatable<Affordance> {

		public string name = "Action";
		public SmartObject affordant;
		public SmartObject affordee;
		protected List<Condition> preconditions = new List<Condition>();
		protected List<Condition> effects = new List<Condition>();
		protected string tag = "";
		protected Node root = null;
		protected bool isStart = false, isEnd = false;

		public string asString () {

			return this.name;
		}

		protected void initialize() {

			tag = Constants.affordanceMap.FirstOrDefault (t => t.Value == this.GetType ()).Key.ToString ();
			name = affordant.name + " " + tag + " " + affordee.name;
		}

		/*
		public string actionSummary () {

			string summary = this.asString ();
			foreach (Condition cond in effects) {
				summary = summary + cond.asString ();
			}
			return summary;	
		}
*/
		public List<Condition> GetEffects () {

			return effects;
		}

		public List<Condition> GetPreConditions () {

			return preconditions;
		}

		public Node GetPBTRoot () {

			return root;
		}

		public bool IsStart() {

			return isStart;
		}

		public bool IsEnd () {

			return isEnd;
		}

		//TODO : Redundant, Ridiculous, REMOVE!!
		public void AddSummaryToTrace () {

//			Constants.PBT_Trace = Constants.PBT_Trace + this.actionSummary ();
		}

		public void publishStartMessage () {
			//TraceMessage msg = new TraceMessage (this.asString (), affordant.name, affordee.name);
			float time = Time.realtimeSinceStartup;//.ToString("n2");// + " " + this.asString();
			EventStartMemory startMem = new EventStartMemory(time, this.asString(), affordant.name, affordee.name);
			MessageBus.PublishMessage ((EventMemory) startMem);
		}

		public Node PublisEventStartMsg () {

			return new LeafInvoke (
				() => this.publishStartMessage());
		}

		public void publishEndMessage () {
			//TraceMessage msg = new TraceMessage (this.asString (), affordant.name, affordee.name);
			float time = Time.realtimeSinceStartup;//.ToString("n2");// + " " + this.asString();
			EventEndMemory endMem = new EventEndMemory(time, this.asString(), affordant.name, affordee.name);
			MessageBus.PublishMessage ((EventMemory) endMem);
		}

		public Node PublisEndMsgNode () {

			return new LeafInvoke (
				() => this.publishEndMessage());
		}


		public Node UpdateNarrativeStateNode () {

			return new LeafInvoke (
				() => NSM.UpdateNarrativeState (effects));
		}

		public Node UpdateAndPublishEndMsg () {

			return new Sequence (this.UpdateNarrativeStateNode (), this.PublisEndMsgNode ());
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

public class StartState : Affordance {

	public StartState (List<Condition> conds) {
		effects = conds;
		isStart = true;
	}
}

public class EndState : Affordance {

	public EndState (List<Condition> conds) {
		preconditions = conds;
		isEnd = true;
	}
}