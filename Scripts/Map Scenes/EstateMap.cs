/// <summary>
/// Estate map.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

public class EstateMap : MonoBehaviour {
	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public GUISkin skin;

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************

	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;

	private GameObject sceneMonitor;

	//Private class storing details of a FLOOR plan
	private class Plan
	{
		public GUIStyle guiStyle;
		public Rect position;
		public string planName;
	}
	private Plan[] plans;

	private string floorPlanXmlName;
	private string xmlPath;

	private Texture mapTexture;

	//*********************
	//*     			  *
	//*     Functions     *
	//*     			  *
	//*********************
	
	// Use this for initialization
	void Start () {
		sceneMonitor = GameObject.Find("SceneMonitor");

		xmlPath = "XMLs/EstateXML/" + sceneMonitor.GetComponent<SceneManager>().GetEstateName() + "_map";

		//Load the XML file to prepare details needed to draw the map
		LoadEstateMapXml(xmlPath);
		if (skin == null) {
			skin = Resources.Load ("UI_Skin/EstateMap") as GUISkin;
		}
	}
	
	void OnGUI () {
		GUI.skin = skin;
		
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity,new Vector3(Screen.width / BGWidth, Screen.height / BGHeight, 1));
		GUI.DrawTexture(new Rect(0, 0, mapTexture.width, mapTexture.height), mapTexture, ScaleMode.StretchToFill);

		//Back button
		if(GUI.Button(new Rect(1700, 100, 200, 200), "", "Back"))
		{
			Application.LoadLevel("Map_District");
		}

		//Generate the buttons to zoom into different floor plan on the Estate map
		int plansCount = plans.Length;
		for (int i = 0; i < plansCount; i++) {
			Plan plan = plans[i];
			//GUI.enabled = plan.enabled;
			if (GUI.Button(plan.position, "", plan.guiStyle)) {
				LoadFloorPlan(floorPlanXmlName, plan.planName);
			}
		}


	}

	void LoadEstateMapXml(string xmlPath) {
		//***********************************************************************
		//*															 		    *
		//*     Read the XML for details about individual estate BG TEXTURE     *
		//*																	    *
		//***********************************************************************

		sceneMonitor.GetComponent<SceneManager>().SetXMLPath(xmlPath);

		XmlDocument xmlDoc = new XmlDocument();

		try {
		TextAsset xmlText = (TextAsset) Resources.Load (xmlPath);
		xmlDoc.LoadXml(xmlText.text);
		} catch (Exception e) {
			sceneMonitor.GetComponent<SceneManager>().SetConsoleMsg(e.Message);
		}

		XmlNode xmlNode = xmlDoc.SelectSingleNode("estate");

		string path = "Texture/Map/BG_Estate/" + xmlNode.Attributes.GetNamedItem("texture").Value;

		Texture2D texture = (Texture2D) Resources.Load(path);

		floorPlanXmlName = xmlNode.Attributes.GetNamedItem("floor_plan").Value;
		mapTexture = texture;
		
		//*******************************************************************************
		//*																	   		    *
		//*     Read the XML for details about individual building ICONs on the map     *
		//*																	   		    *
		//*******************************************************************************
		
		int planCount = xmlNode.ChildNodes.Count;
		plans = new Plan[planCount];
		
		for (int i = 0; i < planCount; i++) {
			GUIStyle guiStyle = new GUIStyle();

			string texName = xmlNode.ChildNodes[i].Attributes.GetNamedItem("texture").Value;
			string[] pos = xmlNode.ChildNodes[i].Attributes.GetNamedItem("pos").Value.Split(",".ToCharArray());
			string subpath = "Texture/Map/Icon_EstateMap/";

			Plan plan = new Plan();
			plan.planName = xmlNode.ChildNodes[i].Attributes.GetNamedItem("name").Value;

			//*******************************************************************************
			//*																	   		    *
			//*     Read the XML for details about individual BLOCK ICONs in the ESTATE     *
			//*		e.g. A, B, C, D, E or 1, 2, 3, 4, 5...
			//*																	   		    *
			//*******************************************************************************

			path = subpath + texName;
			texture = (Texture2D) Resources.Load(path);
			plan.position = new Rect(Convert.ToInt32(pos[0].Trim()), Convert.ToInt32(pos[1].Trim()), texture.width, texture.height);

			//******************************************************************
			//*          													   *
			//*     Sets up the active and normal style of the BLOCK ICONS     *
			//*          													   *
			//******************************************************************

			guiStyle.normal.background = texture;
			path = subpath + texName +"_Clicked";
			texture = (Texture2D) Resources.Load(path);

			guiStyle.active.background = texture;
			guiStyle.focused.background = texture;
			plan.guiStyle = guiStyle;
			plans[i] = plan;
		}
	}

	/// <summary>
	/// Loads the floor plan.
	/// </summary>
	/// <param name="xmlPath">Xml path.</param>
	/// <param name="planName">Plan name.</param>
	void LoadFloorPlan(string planXmlName, string planName) {

		//Update the MapLevelMonitor to keep a track of the zoom level
		sceneMonitor.GetComponent<SceneManager>().SetFloorName(planName);

		//Load scene
		Application.LoadLevel("Map_Floor");
	}}
