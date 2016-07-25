using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TreeSharpPlus;

public class GameController : MonoBehaviour {

	private BehaviorAgent behaviorAgent;
	private CameraController cameraController;
	private GameObject holder;

	public GameObject gameMenu;

	// Use this for initialization
	void Start () {
	
		behaviorAgent = null;
		Time.timeScale = 0f;
		cameraController = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Constants.StartSimulation) {

			Time.timeScale = 1f;
			gameMenu.SetActive (false);
			behaviorAgent = new BehaviorAgent (this.BuildTree ());
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
			Constants.StartSimulation = false;
		}

		if (Constants.MergeMemories) {

			cameraController.EnableCameraMoveControls ();
			holder = cameraController.cameraHolder;
			Time.timeScale = 1f;
			gameMenu.SetActive (false);
		}
	
	}

	public Node BuildTree() {

		//Get your PBT here
		Node PBT = this.gameObject.GetComponent<TestAM_PBT> ().BuildTree ();
		return new Sequence (PBT, this.TerminateTree ());
	}

	public Node SaveTraces () {

		return new LeafInvoke (
			() => MessageBus.SaveTraces ());
	}

	public Node ResetApplication () {

		return new LeafInvoke (
			() => SceneManager.LoadScene (0));
	}

	public Node TerminateTree () {

		return new Sequence (this.SaveTraces (), new LeafWait (1000), this.ResetApplication ());
	}
}
