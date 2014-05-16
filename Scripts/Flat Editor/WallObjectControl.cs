using UnityEngine;
using System.Collections;

public class WallObjectControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void DeselectAllWall()
	{
		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall_selected");

		for(int i = 0; i< walls.Length; i++)
		{
			walls[i].tag = "Wall_normal";
		}
	}

	private void OnMouseDown()
	{
		//print ("Wall = " + transform.name);

		if(this.tag.Equals("Wall_normal")){
			DeselectAllWall();
			this.transform.tag = "Wall_selected";
		} else {
			this.transform.tag = "Wall_normal";
		}
	}

}
