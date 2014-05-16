using UnityEngine;
using System.Collections;

public class DrawDistrictMap : MonoBehaviour {

	public string previousScene;

	public Texture2D mapTexture;
	public Texture2D[] estateList;

	public GUISkin skin;

	//Sub class that defines the estate icon on the district map
	public class Estate
	{
		public GUIStyle guiStyle;
		public bool enabled;
		public Rect position;
		public string estateMapXmlPath;
		public string estateName;
	}
	//Array to store the list of estate icons for the district map
	public Estate[] estates;


	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		GUI.skin = skin;
		GUI.matrix = Matrix4x4.TRS (new Vector3 (0, 0, 0), Quaternion.identity, new Vector3 (Screen.width / BGWidth, Screen.height / BGHeight, 1));
		GUI.DrawTexture (new Rect (0, 0, mapTexture.width, mapTexture.height), mapTexture, ScaleMode.StretchToFill);
		
		//***********************
		//*                     *
		//*     Back button     *
		//*                     *
		//***********************
		if(GUI.Button(new Rect(1700, 100, 200, 200), "", "Back"))
		{
			Application.LoadLevel("MainMap");
		}
		
		//Put the estate icons in place; the icons details are prepared from LoadEstatesXml
		int estatesCount = estates.Length;
		for (int i = 0; i < estatesCount; i++) {
			Estate estate = estates [i];
			GUI.enabled = estate.enabled;
			
			//Set the estate icons as buttons; by pressing will zoom into corresponding estate map
			if (GUI.Button (estate.position, "", estate.guiStyle)) {
				//LoadEstateMap(estate.estateName);
			}
		}
		
	}

}
