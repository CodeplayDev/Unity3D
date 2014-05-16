/// <summary>
/// Instantiate the flat model and put it at the centre of the FlatScene
/// </summary>

using UnityEngine;
using System.Collections;

public class FlatObjectManager : MonoBehaviour {

	public bool logOn = false;

	private float camZoomFact = 0.35f;

	private GameObject sceneMonitor;

	private string rootPath;
	private string subPath;
	private string flatName;

	// Use this for initialization
	void Start () {
		sceneMonitor = GameObject.Find ("SceneMonitor");

		flatName = sceneMonitor.GetComponent<SceneManager>().GetFlatName();

		GameObject instance = Instantiate(Resources.Load ("Flat Models/" + flatName)) as GameObject;
		instance.transform.position = new Vector3(instance.transform.position.x, 0, instance.transform.position.z);
		instance.transform.rotation = Quaternion.identity;

		float insX = instance.transform.FindChild("main_wall").renderer.bounds.extents.z;
		float insZ = instance.transform.FindChild("main_wall").renderer.bounds.extents.x;
		float insArea = insX * insZ;
		float camSize = insArea * camZoomFact;

		Camera.main.orthographicSize = camSize;

		if(logOn){
			print ("Flat Object Name: " + instance.name);
			print ("New object Width: " + insX);
			print ("New object Depth: " + insZ);
			print ("New Object Area: " + insArea);
			print ("2D Camera Size: " + camSize);
		}

		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall_normal");
		for(int i = 0; i < walls.Length; i++) {
			walls[i].AddComponent<WallObjectControl>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
