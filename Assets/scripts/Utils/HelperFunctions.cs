using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HelperFunctions{

	//Retturns the SmartObjects of a type objType in the scene
	public static List<T> GetSmartObjectsOfType<T>(System.Type objType) {

		List<T> objs = new List<T> ();
		objs = GameObject.FindObjectsOfType (objType) as List<T>;
		return objs;
	}
}
