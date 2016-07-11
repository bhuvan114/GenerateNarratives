using System.Collections;
using System.Collections.Generic;

public static class MessageBus {

	static string trace = "";
	static List<TraceMessage> traceMsgs = new List<TraceMessage>();
	static bool unread = false;

	public static void PublishMessage(string msg, string actorName1, string actorName2) {
		
		traceMsgs.Add (new TraceMessage (msg, actorName1, actorName2));
		trace = trace + msg;
		unread = true;
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
		unread = false;
	}
}
