using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MessageBus {

	static List<string> trace = new List<string> ();
	static List<TraceMessage> traceMsgs = new List<TraceMessage>();
	static List<string> traceMsgsString = new List<string>();
	static bool unread = false;

	public static bool killSignal = false;

	public static void PublishMessage(string msg, string actorName1, string actorName2) {
		
		traceMsgs.Add (new TraceMessage (msg, actorName1, actorName2));
		//trace = trace + msg;
		trace.Add(msg);
		traceMsgsString.Add(msg);
		unread = true;

		Debug.Log ("Message Bus : " + msg);

		//Debug.Log ("Message Bus trace : \n" + trace);
	}

	public static List<string> GetMsgsInMsgBus () {

		return traceMsgsString;
	}

	public static bool MsgsContainsActor(string actorName, out List<string> msgText) {

		msgText = new List<string> ();
		foreach (TraceMessage tMsg in traceMsgs) {
			if (tMsg.HasActorInMsg (actorName))
				msgText.Add (tMsg.GetMessage ());
		}

		if (msgText.Count != 0)
			return true;
		else
			return false;
		
	}

	public static bool HasUnreadMsgs() {

		return unread;
	}

	public static void ResetMsgBus() {
		
		traceMsgs = new List<TraceMessage> ();
		traceMsgsString = new List<string> ();
		unread = false;
		killSignal = false;
	}

	public static void SaveTraces() {

		Debug.Log ("Global trace");
		foreach (string msg in trace)
			Debug.Log (msg);
		killSignal = true;
	}
}
