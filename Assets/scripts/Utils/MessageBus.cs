using System.Collections;

public static class MessageBus {

	static string trace = "";
	static string traceMsg = "";
	static bool unread = false;
	static string actor1 = "";
	static string actor2 = "";

	public static void PublishMessage(string msg, string actorName1, string actorName2) {

		actor1 = actorName1;
		actor2 = actorName2;
		traceMsg = msg;
		trace = trace + traceMsg;
		unread = true;
	}

	public static bool MsgContainsActor(string actorName) {

		if (actorName.Equals (actor1) || actorName.Equals (actor2))
			return true;
		else
			return false;
		
	}

	public static bool HasUnreadMsg() {

		return unread;
	}

	public static void ResetMsgBus() {
		
		actor1 = "";
		actor2 = "";
		traceMsg = "";
		unread = false;
	}
}
