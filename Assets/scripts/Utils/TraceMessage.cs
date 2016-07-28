using System.Collections;
using System.Collections.Generic;

/* Messages that are present in the  Message bus are of TraceMessage
 * type. It has the names of actor1 and actor2, and the message.
 */

public class TraceMessage {

	string msg;
	float time;

	public string GetMessage() {

		return msg;
	}

	public float GetTime() {

		return time;
	}

	public bool HappensDuring (TraceMessage tMsg) {

		return (this.time == tMsg.GetTime ()) ? true : false;
	}

	public bool HappensBefore (TraceMessage tMsg) {

		return (this.time < tMsg.GetTime ()) ? true : false;
	}

	public void ConvertToThridPerson (string actorName) {

		if (msg.IndexOf ("I") != -1) {
			msg = actorName + msg.Substring (1);
		}
	}

	public string asString() {

		return time + " " + msg;
	}

	public TraceMessage (float msgTime, string msg) {
		this.msg = msg;
		this.time = msgTime;
	}

	public bool Equals(TraceMessage tMsg) {
		
		if (tMsg == null)
			return false;
		if ((this.GetTime() == tMsg.GetTime()) && (this.GetMessage().Equals(tMsg.GetMessage()))) {
			return true;
		} else {
			return false;
		}
	}
}
