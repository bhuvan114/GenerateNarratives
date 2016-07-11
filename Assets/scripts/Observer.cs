using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Observer : MonoBehaviour {

	string observerTrace = "";

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		if (MessageBus.HasUnreadMsgs ()) {


		}
	}


	/* Returns the names of all SmartObjects in the Field Of View 
	 * of the observer.
	 * SIDE NOTES : All the Smart Objects in the scene will have a
	 * 'SmartObject' tag attaced to them, and variable of SmartObject
	 * type has a 'name' field. This function should return the names
	 * of all the objects with tag 'SmartObject' in its field of view.
	*/
	//TODO : To be implemented (@Kuan)
	List<string> GetObjectsInFieldOfView () {

		List<string> objNames = new List<string> ();

		return objNames;
	}
}
