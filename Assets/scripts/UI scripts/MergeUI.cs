using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MergeUI : MonoBehaviour {

	public enum MenuScreens {
		AGENT_MEMORIES,
		CHRONO_MEMORIES
	}

	public MenuScreens screen = MenuScreens.AGENT_MEMORIES;
	public GameObject mergePane;
	MergeMemories memories;

	void DisplayMemories () {

		memories = new MergeMemories (Constants.agentMemories);
		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = HelperFunctions.GetAgentMemoriesAsString (memories.GetAllMemories ());
	}

	void DisplayStartAndEndMemories () {

		memories.CategorizeStartAndEndEvents ();
		string text = "StartEvents : \n";
		text = text + HelperFunctions.GetAgentMemoriesAsString (memories.GetStartEventMemories ());
		text = text + "End events : \n";
		text = text + HelperFunctions.GetAgentMemoriesAsString (memories.GetEndEventMemories ());
		Text memoriesTxt = GameObject.Find ("Memories").GetComponent<Text>();
		memoriesTxt.text = text;
	}

	public void Next () {

		switch (screen) {
		case MenuScreens.AGENT_MEMORIES:
			DisplayMemories ();
			screen = MenuScreens.CHRONO_MEMORIES;
			break;
		case MenuScreens.CHRONO_MEMORIES:
			DisplayStartAndEndMemories ();
			break;
		default:
			break;
		}
	}

	public void ResetSelect () {

		Constants.MergeMemories = true;
		screen = MenuScreens.AGENT_MEMORIES;
		mergePane.SetActive (false);
	}
}
