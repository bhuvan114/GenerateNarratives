using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorTrees;

[System.Serializable]
public class EventMemory : System.IEquatable<EventMemory> {

	//Affordance aff;
	PerceptionInfo perceptionData;
	string msg, actorOne, actorTwo;
	float startTime = -1, endTime = -1;

	// Optional Fields
	List<Condition> actorOneState = new List<Condition>();
	List<Condition> actorTwoState = new List<Condition>();
	bool hasStateInformation = false;
	bool isStartEvent = false, isTimeEstimated = false, isEstimatedNenory = false;

	void SetUpValues (float msgTime, string msg, string actOne, string actTwo, PerceptionInfo perData, bool isStartEvent) {

		this.msg = msg;
		if (isStartEvent)
			this.startTime = msgTime;
		else
			this.endTime = msgTime;
		this.actorOne = actOne;
		this.actorTwo = actTwo;
		this.perceptionData = perData;
		this.isStartEvent = isStartEvent;
	}

	public EventMemory (float msgTime, string msg, string actOne, string actTwo, bool isStartEvent) {

		SetUpValues (msgTime, msg, actOne, actTwo, new PerceptionInfo(), isStartEvent);
	}

	public EventMemory (EventMemory memEve) {

		PerceptionInfo perInfo = new PerceptionInfo (memEve.GetPerceptionData ());
		SetUpValues (memEve.GetTimeStamp (),
			memEve.GetMessage (),
			memEve.GetActorOneName (),
			memEve.GetActorTwoName (),
			perInfo,
			memEve.IsStartEvent ());
	}

	public void ConvertToThridPerson (string actorName) {

		if (msg.IndexOf ("I ") == 0) {
			msg = actorName + msg.Substring (1);
			actorOne = actorName;
		}

		if (msg.Substring (msg.Length - 2,2).Equals("me")) {
			msg = msg.Substring (0, msg.Length - 2) + actorName;
			actorTwo = actorName;
		}
	}

	public bool ConvertToFirstPerson (string actorName) {

		int indx = msg.IndexOf (actorName);
		if (indx != -1) {
			string newMsg = msg;
			if (indx != (msg.Length - actorName.Length)) {
				int strtPoint = indx + actorName.Length;
				newMsg = msg.Substring (0, indx) + "I" + msg.Substring(strtPoint);
			} else {
				newMsg = msg.Substring (0, indx) + "me";
			}
			msg = newMsg;
			return true;
		} else {
			return false;
		}
	}

	public EventMemory CreateCounterPartEstimate () {

		EventMemory estMem = new EventMemory (GetTimeStamp (), GetMessage (), GetActorOneName (), GetActorTwoName (), !IsStartEvent ());
		estMem.SetIsEstimatedMemory ();
		estMem.SetStartTime (GetStartTime ());
		estMem.SetEndTime (GetEndTime ());
		return estMem;
	}

	// All setters here
	public void SetPerceptionInfo (PerceptionInfo perData) {

		this.perceptionData = perData;
	}

	public void SetActorOneState (List<Condition> state) {

		actorOneState = state;
		hasStateInformation = true;
	}

	public void SetActorTwoState (List<Condition> state) {

		actorTwoState = state;
		hasStateInformation = true;
	}

	public void SetStartTime (float time, float minTime = 0f) {

		Debug.Log ("Setstart time for '" + msg + "' : " + time);
		startTime = (time < minTime) ? minTime : time;
	}

	public void SetEndTime (float time) {

		endTime = time;
	}

	public void SetTime (float time, float minTime = 0f) {

		if (isStartEvent)
			SetStartTime (time, minTime);
		else
			SetEndTime (time);
	}

	public void SetEstimatedTime (float time, float minTime = 0f) {

		if (!isStartEvent)
			SetStartTime (time, minTime);
		else
			SetEndTime (time);
		isTimeEstimated = true;
	}

	protected void SetIsEstimatedMemory () {

		isEstimatedNenory = true;
	}

	// All Getters here
	public bool IsStartEvent () {

		return isStartEvent;
	}

	public bool IsTimeEstimated () {

		return isTimeEstimated;
	}

	public bool IsEstimatedMemory () {

		return isEstimatedNenory;
	}

	public string GetMessage () {

		return msg;
	}

	public Constants.AFF_TAGS GetEventTag () {

		string[] words = msg.Split (new char [] { ' ' });
		Constants.AFF_TAGS tag = (Constants.AFF_TAGS)System.Enum.Parse (typeof(Constants.AFF_TAGS), words [1]);
		return tag;
	}

	public string GetMessageWithMemoryType () {

		if (isStartEvent)
			return (msg + " (start)");
		else
			return (msg + " (end)");
	}

	public float GetTimeStamp () {

		float time = isStartEvent ? GetStartTime () : GetEndTime ();
		return time;
	}

	public string GetActorOneName () {

		return actorOne;
	}

	public string GetActorTwoName () {

		return actorTwo;
	}

	public PerceptionInfo GetPerceptionData () {

		return perceptionData;
	}

	public bool HasStateInformation () {

		return hasStateInformation;
	}

	public List<Condition> GetActorOneState () {

		return actorOneState;
	}

	public List<Condition> GetActorTwoState () {

		return actorTwoState;
	}

	public float GetStartTime () {

		return startTime;
	}

	public float GetEndTime () {

		return endTime;
	}

	public Affordance GetAffordance () {

		//return (aff == null) ? HelperFunctions.GetAffordanceFromString (GetMessage ()) : aff;
		return HelperFunctions.GetAffordanceFromString (GetMessage ());
	}

	// Get memory time and message as string
	public string GetShortMemory () {

		return GetTimeStamp ().ToString () + " " + msg;
	}

	public bool Equals (EventMemory obj)
	{
		if (GetTimeStamp ().Equals (obj.GetTimeStamp ()) && msg.Equals (obj.GetMessage ()) && isStartEvent.Equals (obj.IsStartEvent ()))
			return true;
		else
			return false;
	}

	public bool Completes (EventMemory obj)
	{
		if (msg.Equals (obj.GetMessage ()) && !isStartEvent.Equals (obj.IsStartEvent ()))
			return true;
		else
			return false;
	}

	// Print all the details of the Memory event
	public void PrintTallMemory () {

		Debug.Log ("{\n" +
			" msg : " + msg + ",\n" +
			"start time : " + startTime.ToString() + ",\n" +
			"end time : " + endTime.ToString() + ",\n" +
			"isStartEvent : " + isStartEvent.ToString() + ",\n" +
			"actor one : " + actorOne + ",\n" +
			"actor two : " + actorTwo + ",\n" +
			"actor_1 state : [" + HelperFunctions.GetConditionsAsString(actorOneState) + "],\n" +
			"actor_2 state : [" + HelperFunctions.GetConditionsAsString(actorTwoState) + "],\n" +
			"perception data : " + perceptionData.DisplayPerceptionInfo() + ",\n" +
			"}");
	}
}

[System.Serializable]
public class EventEndMemory : EventMemory {

	public EventEndMemory (float msgTime, string msg, string actOne, string actTwo) : base (msgTime, msg, actOne, actTwo, false) {
	}
}

[System.Serializable]
public class EventStartMemory : EventMemory {

	public EventStartMemory (float msgTime, string msg, string actOne, string actTwo) : base (msgTime, msg, actOne, actTwo, true) {
	}
}