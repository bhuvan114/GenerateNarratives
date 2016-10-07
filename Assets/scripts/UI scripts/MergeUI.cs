using UnityEngine;
using BehaviorTrees;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MergeUI : MonoBehaviour {

	public enum MenuScreens {
		AGENT_MEMORIES,
		CAT_MEMORIES,
		ESTIMATED_MEMORIES,
		START_STATE,
		NARRATIVE_CONSISTENCY,
		RESET
	}

	public MenuScreens screen = MenuScreens.AGENT_MEMORIES;
	public GameObject mergePane;
	MergeMemories memories;
	NarrativeValidator validator;

	void DisplayAllMemmories () {

		memories = new MergeMemories (Constants.agentMemories);	
		string txt = "All Memories : \n";
		txt = txt + HelperFunctions.GetAgentMemoriesAsString (memories.GetAllMemories ());
		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = txt;
	}

	void DisplayStartAndEndMemories () {
			
		memories.CategorizeStartAndEndEvents ();
		string text = "CATEGORIZED : \nStartEvents : \n";
		text = text + HelperFunctions.GetAgentMemoriesAsString (memories.GetStartEventMemories ());
		text = text + "End events : \n";
		text = text + HelperFunctions.GetAgentMemoriesAsString (memories.GetEndEventMemories ());
		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = text;
	}

	void DisplayEstimatedTimes () {

		memories.EstimateMissingTimes ();
		string text = "ESTIMATED :\nStartEvents : \n";
		text = text + HelperFunctions.GetAgentMemoriesAsString (memories.GetStartEventMemories ());
		text = text + "End events : \n";
		text = text + HelperFunctions.GetAgentMemoriesAsString (memories.GetEndEventMemories ());
		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = text;
	}

	void DisplayStartState () {

		validator = new NarrativeValidator (memories.GetStartEventMemories (), memories.GetEndEventMemories ());
		List<Condition> startState = validator.GetStartState ();

		string txt = "Possible Start State : \n";
		foreach(Condition cond in startState)
			txt = txt + "- " + cond.asString() + "\n";
		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = txt;
	}

	void DisplayNarrativeConsistency() {

		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		string txt = memoriesTxt.text;
		if (validator.IsNarrativeConsistent ()) {
			txt = txt + "NARRATIVE IS CONSISTENT!!\n";
		} else {
			List<CausalLink> incls = validator.GetInconsistencies ();
			txt = txt + "NARRATIVE INCONSISTENT (" + incls.Count + ")\n";

			foreach (CausalLink cl in incls)
				txt = txt + "- " + cl.asString () + "\n";
		}
		memoriesTxt.text = txt;
	}

	public void Next () {

		switch (screen) {
		case MenuScreens.AGENT_MEMORIES:
			DisplayAllMemmories ();
			screen = MenuScreens.CAT_MEMORIES;
			break;
		case MenuScreens.CAT_MEMORIES:
			DisplayStartAndEndMemories ();
			screen = MenuScreens.ESTIMATED_MEMORIES;
			break;
		case MenuScreens.ESTIMATED_MEMORIES:
			DisplayEstimatedTimes ();
			screen = MenuScreens.START_STATE;
			break;
		case MenuScreens.START_STATE:
			DisplayStartState ();
			screen = MenuScreens.NARRATIVE_CONSISTENCY;
			break;
		case MenuScreens.NARRATIVE_CONSISTENCY:
			DisplayNarrativeConsistency ();
			screen = MenuScreens.RESET;
			break;
		default:
			ResetSelect ();
			break;
		}
	}

	public void ResetSelect () {

		Constants.MergeMemories = true;
		screen = MenuScreens.AGENT_MEMORIES;
		mergePane.SetActive (false);
	}
}
