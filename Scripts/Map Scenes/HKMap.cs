using UnityEngine;
using System.Collections;

public class HKMap : MonoBehaviour {

	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public GUISkin mainMapSkin;

	public Texture ClickedTexture_HK;
	public Texture ClickedTexture_Kowloon;
	public Texture ClickedTexture_Lantau;
	public Texture ClickedTexture_NT;
	public Texture mapTexture;
	public Texture points;

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************

	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;

	private GameObject sceneMonitor;

	private Texture activeClickedTexture;

	//*********************
	//*     			  *
	//*     Functions     *
	//*     			  *
	//*********************

	// Use this for initialization
	void Start () {
		sceneMonitor = GameObject.Find("SceneMonitor");
	}
	

	void OnGUI () {
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new Vector3(Screen.width / BGWidth, Screen.height / BGHeight, 1));

		//Draw the HK map background
		GUI.DrawTexture(new Rect(0, 0, mapTexture.width, mapTexture.height), mapTexture, ScaleMode.StretchToFill);

		//Assign the GUI skin
		if (mainMapSkin != null)
			GUI.skin = mainMapSkin;

		//***********************
		//*                     *
		//*     Back button     *
		//*                     *
		//***********************
		if(GUI.Button(new Rect(1700, 100, 200, 200), "", "Back"))
		{
			//Load the scene "MainMap"
			if(Application.GetStreamProgressForLevel("MainMenu") == 1){
				//Load the MainMenu scene
				Application.LoadLevel("MainMenu");

				//Destroy the previous SceneMonitor GameObject
				//SceneMonitor is a MainMenu game object, so when going back to main menu
				//Current one has to be destroyed to avoid duplicant
				Destroy(sceneMonitor);

			}
		}

		int btnWidth = GUI.skin.customStyles[0].normal.background.width;
		int btnHeight = GUI.skin.customStyles[0].normal.background.height;

		//Controls the TOUCH (holding down) event for mobile devices
		//This just light up the area without activating the next page
		if (Input.touchCount > 0) {
			if (Input.GetTouch(0).phase == TouchPhase.Began) {
				if (new Rect(1102, 1120, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
					activeClickedTexture = ClickedTexture_HK;
				} else if (new Rect(248, 1107, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
					activeClickedTexture = ClickedTexture_Lantau;
				} else if (new Rect(799, 562, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
					activeClickedTexture = ClickedTexture_NT;
				} else if (new Rect(1050, 877, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
					activeClickedTexture = ClickedTexture_Kowloon;
				}
			} else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) {
				activeClickedTexture = null;
			}
			
			if (activeClickedTexture != null) {
				GUI.DrawTexture(new Rect(0, 0, activeClickedTexture.width, activeClickedTexture.height), activeClickedTexture, ScaleMode.StretchToFill);
			}
		}
		
		//Controls the MOUSEDOWN (holding down) event for PC devices
		//This just light up the area without activating the next page
		if (Event.current.type == EventType.mouseDown) {
			if (new Rect(1102, 1120, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
				activeClickedTexture = ClickedTexture_HK;
			} else if (new Rect(248, 1107, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
				activeClickedTexture = ClickedTexture_Lantau;
			} else if (new Rect(799, 562, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
				activeClickedTexture = ClickedTexture_NT;
			} else if (new Rect(1050, 877, btnWidth, btnHeight).Contains(Event.current.mousePosition)) {
				activeClickedTexture = ClickedTexture_Kowloon;
			}
		} else if (Event.current.type == EventType.mouseUp) {
			activeClickedTexture = null;
		}
		
		
		if (activeClickedTexture != null) {
			GUI.DrawTexture(new Rect(0, 0, activeClickedTexture.width, activeClickedTexture.height), activeClickedTexture, ScaleMode.StretchToFill);
		}

		//Draw the dots on the maps that represent the estates locations
		GUI.DrawTexture(new Rect(0, 0, points.width, points.height), points, ScaleMode.StretchToFill);

		//***********************************************************************************
		//*                                                                                 *
		//*     Actual handling of the button when CLICKED; will activate the next page     *
		//*                                                                                 *
		//***********************************************************************************

		if (GUI.Button(new Rect(1102, 1120, btnWidth, btnHeight),"", "HongKongBtn")) {
			//Update the MapLevelMonitor to keep a track of the zoom level
			sceneMonitor.GetComponent	<SceneManager>().SetDistrictName("HongKong");

			//Load the scene "MainMap"
			Application.LoadLevel("Map_District");
		}
		if (GUI.Button(new Rect(248, 1107, btnWidth, btnHeight),"", "LantauBtn")) {
			//Update the MapLevelMonitor to keep a track of the zoom level
			sceneMonitor.GetComponent	<SceneManager>().SetDistrictName("Lantau");
			
			//Load the scene "MainMap"
			Application.LoadLevel("Map_District");
		}
		if (GUI.Button(new Rect(799, 562, btnWidth, btnHeight),"", "NewTerritoriesBtn")) {
			//Update the MapLevelMonitor to keep a track of the zoom level
			sceneMonitor.GetComponent	<SceneManager>().SetDistrictName("NewTerritories");
			
			//Load the scene "MainMap"
			Application.LoadLevel("Map_District");
		}
		if (GUI.Button(new Rect(1050, 877, btnWidth, btnHeight),"", "KowloonBtn")) {
			//Update the MapLevelMonitor to keep a track of the zoom level
			sceneMonitor.GetComponent	<SceneManager>().SetDistrictName("Kowloon");
			
			//Load the scene "MainMap"
			Application.LoadLevel("Map_District");
		}

	}
}
