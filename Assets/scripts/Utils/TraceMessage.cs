using System.Collections;

public class TraceMessage{

	string msg, actor1, actor2;

	public string GetMessage() {

		return msg;
	}

	public bool HasActorInMsg(string actorName) {

		if (actorName.Equals (actor1) || actorName.Equals (actor2))
			return true;
		else
			return false;
	}

	public TraceMessage (string tMsg, string actorOneName, string actorTwoName) {
		msg = tMsg;
		actor1 = actorOneName;
		actor2 = actorTwoName;
	}
}
