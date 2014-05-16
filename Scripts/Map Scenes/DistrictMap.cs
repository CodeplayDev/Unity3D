/// <summary>
/// District map.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

public class DistrictMap : MonoBehaviour {

	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public enum MapType
	{
		HongKong = 0,
		Kowloon,
		Lantau,
		NewTerritories
	}

	public GameObject loadingObj;

	public GUISkin skin;

	public MapType type;

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************

	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;
	
	//Sub class that defines the estate icon on the district map
	private class Estate
	{
		public GUIStyle guiStyle;
		public bool enabled;
		public Rect position;
		public string estateMapXmlPath;
		public string estateName;
	}
	//Array to store the list of estate icons for the district map
	private Estate[] estates;

	private GameObject sceneMonitor;

	//The XML files where district texture and details of estates icons on map are stored
	private string[] xmlPaths = new string[] { 
		"XMLs/MapXML/HK_Island_Estates",
		"XMLs/MapXML/Kowloon_Estates",
		"XMLs/MapXML/Lantau_Estates",
		"XMLs/MapXML/NewTerritories_Estates"
	};


	private Texture mapTexture;

	private XmlDocument xmlDoc;
	private XmlNode xmlNode;

	//*********************
	//*     			  *
	//*     Functions     *
	//*     			  *
	//*********************

	// Use this for initialization
	void Start () {
		sceneMonitor = GameObject.Find("SceneMonitor");

		string mapType = sceneMonitor.GetComponent<SceneManager>().GetDistrictName();

		switch(mapType){
		case "HongKong":
			type = MapType.HongKong;
			break;

		case "Kowloon":
			type = MapType.Kowloon;
			break;

		case "Lantau":
			type = MapType.Lantau;
			break;

		case "NewTerritories":
			type = MapType.NewTerritories;
			break;
		}

		LoadEstatesXml (xmlPaths [(int)type]);
		if (skin == null) {
			skin = Resources.Load ("UI_Skin/DistrictMap") as GUISkin;
		}
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
				LoadEstateMap(estate.estateName);
			}
		}

	}

	/// <summary>
	/// Base on the details prepared by LoadEstatesXml, create new GameObject as the 
	/// </summary>
	/// <param name="xmlPath">Xml path.</param>
	void LoadEstateMap (String estateName)
	{
		//Update the MapLevelMonitor to keep a track of the zoom level
		sceneMonitor.GetComponent<SceneManager>().SetEstateName(estateName);

		//Load the scene
		Application.LoadLevel("Map_Estate");
	}


	/// <summary>
	/// Reads the XML file where details of the map is stored.
	/// 1. Read the <district> tag to get the map background details
	/// 2. Read the <estate> tag nodes by nodes to get the estate icons details
	/// 
	/// The details are stored as private class Estate
	/// </summary>
	/// <param name="xmlPath">XML path of the XML file where estate details are stored</param>
	void LoadEstatesXml (string xmlPath)
	{
		sceneMonitor.GetComponent<SceneManager>().SetXMLPath(xmlPath);

		//**********************************************************************
		//*																	   *
		//*     Read the XML for details about individual dsitrict TEXTURE     *
		//*																	   *
		//**********************************************************************
			xmlDoc = new XmlDocument ();
			//xmlDoc.LoadXml (File.ReadAllText (xmlPath));
			TextAsset xmlText = (TextAsset) Resources.Load(xmlPath);
			xmlDoc.LoadXml(xmlText.text);
			xmlNode = xmlDoc.SelectSingleNode ("district");

		mapTexture = (Texture2D) Resources.Load ("Texture/Map/BG_District/" + xmlNode.Attributes.GetNamedItem("texture").Value);

		//******************************************************************************
		//*																	   		   *
		//*     Read the XML for details about individual estates ICONs on the map     *
		//*																	   		   *
		//******************************************************************************

		int estatesCount = xmlNode.ChildNodes.Count;
		estates = new Estate[estatesCount];
		
		for (int i = 0; i < estatesCount; i++) {
			GUIStyle guiStyle = new GUIStyle ();

			string subpath = "Texture/Map/Icon_DistrictMap/";
			string texName = xmlNode.ChildNodes [i].Attributes.GetNamedItem ("texture").Value;
			string[] pos = xmlNode.ChildNodes [i].Attributes.GetNamedItem ("pos").Value.Split (",".ToCharArray ());

			Estate estate = new Estate ();
			estate.estateName = xmlNode.ChildNodes[i].Attributes.GetNamedItem("name").Value;

			//*****************************************************************************
			//*  	        														      *
			//*     Read the XML for details about individual estate TEXTURE (normal)     *
			//*																       		  *
			//*****************************************************************************
			estate.enabled = xmlNode.ChildNodes [i].Attributes.GetNamedItem ("enabled").Value.Equals ("true");
			estate.estateMapXmlPath = xmlNode.ChildNodes [i].Attributes.GetNamedItem ("map").Value;

			Texture2D texture = (Texture2D) Resources.Load (subpath + texName + "_Normal");

			estate.position = new Rect (Convert.ToInt32(pos[0].Trim()), Convert.ToInt32(pos[1].Trim()), texture.width, texture.height);

			guiStyle.normal.background = texture;

			//******************************************************************************
			//*  	        														       *
			//*     Read the XML for details about individual estate TEXTURE (clicked)     *
			//*																       		   *
			//******************************************************************************
			texture = (Texture2D) Resources.Load(subpath + texName + "_Clicked");

			//assign to the GUI style
			guiStyle.active.background = texture;
			guiStyle.focused.background = texture;
			estate.guiStyle = guiStyle;
			estates [i] = estate;
		}

	}
}
