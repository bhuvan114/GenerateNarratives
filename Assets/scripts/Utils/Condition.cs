﻿using UnityEngine;
using System.Collections;

namespace BehaviorTrees {
	public class Condition {
		string actor;
		string cond;
		bool status;
		bool isInit = false;

		public Condition(string actor, string cond, bool status) {

			this.actor = actor;
			this.cond = cond;
			this.status = status;
		}

		public Condition(string actor, string cond, bool status, bool isInit) {

			this.actor = actor;
			this.cond = cond;
			this.status = status;
			this.isInit = isInit;
		}

		public string asString () {

			if (status == false)
				cond = "not-" + cond;
			if (isInit == true)
				return actor + " \\ is \\ " + cond + "\n";
			else
				return actor + " \\ thus changes \\ " + cond + "\n";
		}
	}
}