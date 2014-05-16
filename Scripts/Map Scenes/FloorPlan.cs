/// <summary>
/// Floor plan.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

public class FloorPlan : MonoBehaviour {
	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public GUISkin skin;
	public GameObject loadingPrefab;

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************

	private AsyncOperation AO;

	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;

	private GameObject sceneMonitor;

	private string estateName;
	private string planName;
	private string xmlFileName;
	private string xmlPath;

	private Texture mapTexture;
	//	private string floorPlanXmlPath;
	
	private class Flat
	{
		public GUIStyle guiStyle;
		//		public bool enabled;
		public Rect position;
		//		public string flatXmlPath;
		public string flatPlanName;
	}
	
	private Flat[] flats;

	//*********************
	//*     			  *
	//*     Functions     *
	//*     			  *
	//*********************

	IEnumerator LoadFlat()
	{
		AO = Application.LoadLevelAsync("FlatScene");
		AO.allowSceneActivation = false;

		while(!AO.isDone)
		{

			//print (AO.progress + " [" + AO.isDone + "]");

			if(AO.progress >= 0.9f)
			{
				AO.allowSceneActivation = true;
				break;
			}

			//yield return null;
		}

		//AO.allowSceneActivation = true;
		yield return AO;
	}

	// Use this for initialization
	void Start () {

		sceneMonitor = GameObject.Find("SceneMonitor");

		estateName = sceneMonitor.GetComponent<SceneManager>().GetEstateName();
		planName  = sceneMonitor.GetComponent<SceneManager>().GetFloorName();

		xmlPath = "XMLs/FloorXML/";
		xmlFileName = estateName + "_floor_plans";

		LoadFloorPlanXml(xmlPath + xmlFileName, planName);

		if (skin == null) {
			skin = Resources.Load ("UI_Skin/FloorPlan") as GUISkin;
		}
	}

	void OnGUI () {
		GUI.skin = skin;
		
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new Vector3(Screen.width / BGWidth, Screen.height / BGHeight, 1));
		GUI.DrawTexture(new Rect(0, 0, mapTexture.width, mapTexture.height), mapTexture, ScaleMode.StretchToFill);
		
		//Back button
		if(GUI.Button(new Rect(1700, 100, 200, 200), "", "Back")) {
			Application.LoadLevel("Map_Estate");
		}

		// plans
		int flatsCount = flats.Length;
		for (int i = 0; i < flatsCount; i++) {
			Flat flat = flats[i];
			//GUI.enabled = plan.enabled;
			if (GUI.Button(flat.position, "", flat.guiStyle)) {
				//DownloadedPlanMenuActivated();

				//loadingObj.SetActive(true);
				
				//Load the scene "MainMap"
				//if(Application.GetStreamProgressForLevel("FlatScene") == 1){
				sceneMonitor.GetComponent<SceneManager>().SetFlatName(flat.flatPlanName);
				//
				//	Instantiate(loadingPrefab, new Vector3(0.5f, 0.5f, 0f), Quaternion.identity);
				//
				//	StartCoroutine(LoadFlat());
				//
				//	gameObject.SetActive(false);
				//}
			
				Application.LoadLevel("FlatScene");
			}
		}
	}

	/// <summary>
	/// Loads the floor plan xml.
	/// </summary>
	/// <param name="xmlPath">Xml path.</param>
	/// <param name="planName">Plan name.</param>
	void LoadFloorPlanXml(string xmlPath, string planName) {

		//******************************************************************************
		//*                                                                            *
		//*     Parse the XML to findout which floor plan detail set would be used     *
		//*     Plan Name is carried from EstateMap to tell this script which flat     *
		//*                                                                            *
		//******************************************************************************

		XmlDocument xmlDoc = new XmlDocument();
		TextAsset xmlText = (TextAsset) Resources.Load(xmlPath);
		xmlDoc.LoadXml(xmlText.text);
		XmlNode xmlNode = xmlDoc.SelectSingleNode("floor_plans");
		xmlNode = xmlNode.SelectSingleNode("floor_plan" + planName);

		int flatCount = xmlNode.ChildNodes.Count;

		string path = "Texture/Map/BG_Floor/" + estateName + "/" + xmlNode.Attributes.GetNamedItem("texture").Value;
		Texture2D texture;

		//********************************************
		//*                                          *
		//*     Prepare the Floor Plan BG Texture    *
		//*                                          *
		//********************************************

		mapTexture = (Texture2D) Resources.Load(path);
		sceneMonitor.GetComponent<SceneManager>().SetMapTextureName(path);

		flats = new Flat[flatCount];

		//******************************************************************************
		//*                                                                            *
		//*     Start parsing the XML file and add FLAT icons on to the floor plan     *
		//*                                                                            *
		//******************************************************************************

		for (int i = 0; i < flatCount; i++) {
			GUIStyle guiStyle = new GUIStyle();

			string texName = xmlNode.ChildNodes[i].Attributes.GetNamedItem("texture").Value;
			string[] pos = xmlNode.ChildNodes[i].Attributes.GetNamedItem("pos").Value.Split(",".ToCharArray());
			string flatName = xmlNode.ChildNodes[i].Attributes.GetNamedItem ("name").Value;
			string subpath = "Texture/Map/Icon_EstateMap/";

			Flat flat = new Flat();
			path = subpath + texName;

			//**************************
			//*                        *
			//*     Normal Texture     *
			//*                        *
			//**************************

			sceneMonitor.GetComponent<SceneManager>().SetXMLPath(path);
			texture = (Texture2D) Resources.Load(path);
			flat.position = new Rect(Convert.ToInt32(pos[0].Trim()), Convert.ToInt32(pos[1].Trim()), texture.width, texture.height);
			flat.flatPlanName = flatName;
			
			//*************************
			//*                       *
			//*     Hover Texture     *
			//*                       *
			//*************************
			guiStyle.normal.background = texture;
			path = subpath + texName + "_Clicked";
			texture = (Texture2D) Resources.Load(path);

			guiStyle.active.background = texture;
			guiStyle.focused.background = texture;
			flat.guiStyle = guiStyle;
			flats[i] = flat;
		}
	}
}
