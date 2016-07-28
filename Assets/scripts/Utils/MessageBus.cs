using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* All the messages are published here. In this case, when an affordance
 * is executed, the details of the affordance are published in here in form
 * of a Trace message. Any oberver can read from this.
 */
public static class MessageBus {

	// Contains the entire trace of the narrative in god's-eye point
	static List<string> trace = new List<string> ();

	//static List<TraceMessage> traceMsgs = new List<TraceMessage>();
	static List<string> traceMsgsString = new List<string>();
	static bool unread = false;

	public static bool killSignal = false;

	// Function to publish a message. It sets the 'unread' flag to true.
	public static void PublishMessage(string msg, string actorName1, string actorName2) {
		
		//traceMsgs.Add (new TraceMessage (msg, actorName1, actorName2));
		//trace = trace + msg;
		trace.Add(msg);
		traceMsgsString.Add(msg);
		unread = true;

		Debug.Log ("Message Bus : " + msg);

		//Debug.Log ("Message Bus trace : \n" + trace);
	}

	// Returns the new unread message
	public static List<string> GetMsgsInMsgBus () {

		return traceMsgsString;
	}

	// Returns all the messages that contains an actor
	// Returns true if such message exists
	/*public static bool MsgsContainsActor(string actorName, out List<string> msgText) {

		msgText = new List<string> ();
		foreach (TraceMessage tMsg in traceMsgs) {
			if (tMsg.HasActorInMsg (actorName))
				msgText.Add (tMsg.GetMessage ());
		}

		if (msgText.Count != 0)
			return true;
		else
			return false;
		
	}*/

	// Checks if the message bus has any new messaages
	public static bool HasUnreadMsgs() {

		return unread;
	}

	// Resets everything in msg bus except the global trace.
	public static void ResetMsgBus() {
		
		//traceMsgs = new List<TraceMessage> ();
		traceMsgsString = new List<string> ();
		unread = false;
		killSignal = false;
	}

	// Save the Global trace
	public static void SaveTraces() {

		Debug.Log ("Global trace");
		foreach (string msg in trace)
			Debug.Log (msg + "\n");
		killSignal = true;
	}
}
