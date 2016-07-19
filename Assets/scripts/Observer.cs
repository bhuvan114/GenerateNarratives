using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Observer : MonoBehaviour {

	List<string> observerTrace = new List<string>();
	string obsName;
	bool msgRead = false;

	Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
	Quaternion stepAngle = Quaternion.AngleAxis(4, Vector3.up);

	// Use this for initialization
	void Start () {

		obsName = this.gameObject.GetComponent<SmartCharacter> ().name;
	}
	
	// Update is called once per frame
	void Update () {

		if (msgRead) {
			MessageBus.ResetMsgBus ();
			msgRead = false;
		}

		if (MessageBus.HasUnreadMsgs ()) {
			
			List<string> msgs = MessageBus.GetMsgsInMsgBus ();
			msgs = GetMsgsForObserver (msgs);
			foreach(string msg in msgs)
				observerTrace.Add (msg);
			msgRead = true;
		}



		if (MessageBus.killSignal) {
			SaveObserverTrace ();
			msgRead = true;
		}

		//DetectObjectsInFOV ();
	}


	List<string> GetMsgsForObserver (List<string> msgs) {

		//Debug.Log ("Name : " + obsName);

		List<string> objs = GetObjectsInFieldOfView ();
		List<string> observedMsgs = new List<string>();
		foreach (string msg in msgs) {
			int indx = msg.IndexOf (obsName);
			if (indx != -1) {
				string newMsg = msg;
				if (indx == 0) {
					
					newMsg = "I" + msg.Substring(obsName.Length);
				} else {
					newMsg = msg.Substring (0, indx) + "me";
				}
				observedMsgs.Add (newMsg);

			} else {
				//go through all observers
				List<string> objsInFOV = GetObjectsInFieldOfView();
				foreach (string obj in objsInFOV) {
					if ((msg.IndexOf (obj) != -1) && (!observedMsgs.Contains (msg))) {
						observedMsgs.Add (msg);
					}
				}
			}
		}
		return observedMsgs;
	}

	/* Returns the names of all SmartObjects in the Field Of View 
	 * of the observer.
	 * SIDE NOTES : All the Smart Objects in the scene will have a
	 * 'SmartObject' tag attaced to them, and variable of SmartObject
	 * type has a 'name' field. This function should return the names
	 * of all the objects with tag 'SmartObject' in its field of view.
	*/
	List<string> GetObjectsInFieldOfView () {
		
		RaycastHit hit;
		List<string> objs = new List<string> ();
		var angle = transform.rotation * startingAngle;
		var direction = angle * Vector3.forward;
		var pos =  transform.position;
		pos.y = 1.5f;
		for(var i = 0; i < 30; i++)
		{
			if (Physics.Raycast (pos, direction, out hit, 30)) {
				Debug.LogWarning ("Rayhit - " + hit.collider.name + " -for- " + obsName);
				Debug.DrawLine (pos, hit.collider.transform.position, Color.red);
				string objName = hit.collider.GetComponentInParent<SmartCharacter> ().name;
				if (!objs.Contains (objName))
					objs.Add (objName);
			} else {
				Debug.DrawLine (pos, pos + direction * 30, Color.green);
			}
			direction = stepAngle * direction;
		}

		return objs;
	}

	public void SaveObserverTrace () {

		Debug.Log ("Observer " + obsName + " : ");
		foreach (string msg in observerTrace)
			Debug.Log (msg);
	}
}
