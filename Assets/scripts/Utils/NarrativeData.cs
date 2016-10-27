using UnityEngine;
using BehaviorTrees;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public static class NarrativeData {

	public static Dictionary<string, List<string>> affordancesWithEffect = new Dictionary<string, List<string>> ();
	public static Dictionary<string, List<Affordance>> affordancesOfType = new Dictionary<string, List<Affordance>> ();

	static void populateActionEffectMap () {

		foreach (string affType in affordancesOfType.Keys) {
			foreach (Affordance aff in affordancesOfType[affType]) {
				List<Condition> effects = aff.GetEffects ();
				foreach (Condition effect in effects) {
					if (!affordancesWithEffect.ContainsKey (effect.getConditionName ()))
						affordancesWithEffect.Add (effect.getConditionName (), new List<string> ());
					if (!affordancesWithEffect [effect.getConditionName ()].Contains (affType))
						affordancesWithEffect [effect.getConditionName ()].Add (affType);
				}
			}
		}

		//----------- TO BE REMOVED ------------
		foreach (string condName in affordancesWithEffect.Keys)
			Debug.Log (condName);
		//--------------------------------------
	}

	static void populateActionActionTypeMap () {

		List<System.Type> actionTypes = System.Reflection.Assembly.GetExecutingAssembly ().GetTypes ()
			.Where (t => t.BaseType != null && t.BaseType == typeof(BehaviorTrees.Affordance)).ToList ();
		actionTypes.Remove (typeof(StartState));
		actionTypes.Remove (typeof(EndState));

		Dictionary<string, GameObject> objectNameObjectMap = new Dictionary<string, GameObject> ();
		foreach (string objName in Constants.gameObjToSmartObjMap.Keys)
			objectNameObjectMap.Add (Constants.gameObjToSmartObjMap [objName], GameObject.Find (objName));
		

		foreach (System.Type actionType in actionTypes) {
			ConstructorInfo[] affCons = actionType.GetConstructors ();
			List<Affordance> affs = new List<Affordance> ();
			foreach (ConstructorInfo consInfo in affCons) {
				ParameterInfo[] consPara = consInfo.GetParameters ();

				//----------- TO BE REMOVED ------------
				Debug.Log (actionType.ToString ());
				foreach (ParameterInfo parInf in consPara)
					Debug.Log (parInf.ParameterType.ToString ());
				//--------------------------------------
					
				System.Type typ1 = consPara[0].ParameterType;
				System.Type typ2 = consPara[1].ParameterType;
				if (Constants.objectsOfTypeMap.ContainsKey (typ1) && Constants.objectsOfTypeMap.ContainsKey (typ2)) {
					foreach (string objOneName in Constants.objectsOfTypeMap[typ1]) {
						foreach (string objTwoName in Constants.objectsOfTypeMap[typ2]) {
							if (objOneName != objTwoName) {
								Affordance aff = (Affordance)System.Activator.CreateInstance (actionType,
									                 objectNameObjectMap [objOneName].GetComponent (typ1),
									                 objectNameObjectMap [objTwoName].GetComponent (typ2));
								affs.Add (aff);
							}
						}
					}
				}
			}
			affordancesOfType.Add (actionType.ToString (), affs);
		}
	}

	public static void populateNarrativeData () {

		populateActionActionTypeMap ();
		populateActionEffectMap ();
	}
}
