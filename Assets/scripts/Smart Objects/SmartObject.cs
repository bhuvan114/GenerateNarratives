using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTrees {
	
	public class SmartObject /*: MonoBehaviour*/ {

		public string name = "Smart Object";
		public string relName = "";

		// Returns all the supported affordances types of the character
		public List<System.Type> GetSupportedAffordances() {

			List<System.Type> affTypes = new List<System.Type> ();
			System.Reflection.FieldInfo[] members = this.GetType ().GetFields ();
			foreach (System.Reflection.FieldInfo member in members) {
				if (member.FieldType.BaseType == typeof(BehaviorTrees.Affordance)) {
					affTypes.Add(member.FieldType);
				}
			}
			return affTypes;
		}
	}
}