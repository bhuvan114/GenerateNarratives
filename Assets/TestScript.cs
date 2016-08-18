using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TestScript : MonoBehaviour {

	//bool temp = false;
	// Use this for initialization
	void Start () {
		string filePath = Constants.traceFilesPath + "Dan.bin";
		if (File.Exists (filePath)) {
			List<EventMemory> salesman;
			//deserialize
			using (Stream stream = File.Open(filePath, FileMode.Open))
			{
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

				salesman = (List<EventMemory>)bformatter.Deserialize(stream);
			}
			foreach(EventMemory mem in salesman)
				mem.PrintTallMemory ();

		} /*else {
			List<MemoryEvent> mem = new List<MemoryEvent> ();
			Vector3 t1 = GameObject.Find ("obj1").transform.position;
			Vector3 t2 = GameObject.Find ("obj2").transform.position;
			VisualPercptionData viData = new VisualPercptionData (t1, t2);
			PerceptionInfo perInfo = new PerceptionInfo ();
			perInfo.SetVisionData (viData);
			MemoryEvent memEve = new MemoryEvent (1.0f, "msg_1", "one", "two");
			memEve.SetPerceptionInfo (perInfo);
			mem.Add(memEve);
			mem.Add(new MemoryEvent (2.0f, "msg_2", "one", "two"));
			using (Stream stream = File.Open(filePath, FileMode.Create))
			{
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

				bformatter.Serialize(stream, mem);
			}
			Debug.Log ("File created");
		}*/
	}
	
	// Update is called once per frame
	void Update () {


	}
}
