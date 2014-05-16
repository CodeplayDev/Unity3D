/// <summary>
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Text;
using System;

public class MainMenu : MonoBehaviour {
	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public GameObject loadingPrefab;
	public GameObject sceneMon;

	public GUISkin mainMeunSkin;

	public Texture2D mainBG;
	public Texture2D mainLogo;

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************

	private AsyncOperation AO;

	private float dWidth = 1024.0f;
	private float dHeight = 768.0f;
	private int currentMenu = 0;	//Controls which page of the menu currently displayed

	//*********************
	//*     			  *
	//*     Functions     *
	//*     			  *
	//*********************

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	private void OnGUI()
	{
		//Placing the GUI with Matrix4x4 at position Vector(0,0,0), with NO rotation and scale accordingly to the 1024x768
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(Screen.width / dWidth, Screen.height / dHeight, 1));

		//Draw the Background
		GUI.DrawTexture(new Rect(0,0, 1024,768), mainBG,ScaleMode.StretchToFill);

		//Draw the Main Logo
		GUI.DrawTexture(new Rect(0,0, 1024,768), mainBG,ScaleMode.StretchToFill);

		//Setup the main menu (GUI) skin
		if(mainMeunSkin != null)
			GUI.skin = mainMeunSkin;

		Menu_Top();

		//Change page within the MainTitle Scene
		switch(currentMenu){
		case 0:	//TOP Main Menu; initialized
			GUI.DrawTexture(new Rect((1024.0f * 0.5f) - (380f * 0.5f), (768f * 0.4f) - (144 * 0.5f), 380f, 144f), mainLogo, ScaleMode.StretchToFill);
			break;

		case 1:	//Home Design page
			HomeDesignMenuActivated();
			break;
		
		case 2:	//Shopping page
			ShoppingMenuActivated();
			break;

		case 3:	//Home Design > New Plan
			CreateNewPlanActivated();
			break;

		case 4: // Home Design > Demo Flat
			DemoFlatActivated();
			break;

		default:
			break;
		}
	}

	/// <summary>
	/// Loads the flat.
	/// </summary>
	/// <returns>The flat.</returns>
	IEnumerator LoadFlat(string sceneName)
	{
		Instantiate(loadingPrefab, new Vector3(0.5f, 0.5f, 0f), Quaternion.identity);

		AO = Application.LoadLevelAsync(sceneName);
		//AO.allowSceneActivation = false;

		while(!AO.isDone)
		{
			print (AO.progress + " [" + AO.isDone + "]");
			
			yield return null;
		}


	}

	/// <summary>
	/// Sets the main logo to visible and generate the TOP menu buttons
	///  Left button: Home Designer
	/// Right button: Shopping
	/// 
	/// By clicking the buttons, alters the currentMenu to redraw the GUI and display a new set of menu buttons.
	/// </summary>
	private void Menu_Top(){

		float btnWidth = 140f;
		float btnHeight = 170f;
		float btnGap = 82f;

		//Create the HomeDesign Button
		float btnX = (dWidth * 0.5f) - btnWidth - btnGap;
		float btnY = 558f;

		if(GUI.Button(new Rect(btnX , btnY, btnWidth, btnHeight),"", "HomeDesignBTN")){
			currentMenu = 1;
		}
		
		//Create the Shopping Button
		btnX = (dWidth * 0.5f) + btnGap;

		if(GUI.Button(new Rect(btnX	, btnY, btnWidth, btnHeight),"", "ShoppingBTN")){
			currentMenu = 2;	
		}	
	}

	/// <summary>
	/// 
	/// </summary>
	void HomeDesignMenuActivated(){

		float titleW = 252.0f;
		float titleH = 30.0f;
		float titleX = dWidth * 0.5f - titleW * 0.25f;
		float titleY = 50f;

		//Create the title "Design" for the page
		GUI.Label (new Rect(titleX , titleY, titleW, titleH), "Design", "FontTitle");

		//Change pages when "New" button is pressed
		if(GUI.Button(new Rect(270,95,100,100),"", "NewPlanBTN")){
			currentMenu = 3;
		}

		GUI.Label (new Rect(390,130,65,180), "New - ", "FontBold");
		GUI.Label (new Rect(455,130,355,180), "Create New Plan", "Font");

		//Change pages when "Demo Flat" button is pressed
		if(GUI.Button(new Rect(270,210,100,100),"", "OpenPlanBTN")){
			currentMenu = 4;
		}
		GUI.Label (new Rect(390,255,77,180), "Demo Flat", "FontBold");
		
		//Change pages when "Open" button is pressed
		if(GUI.Button(new Rect(270,325,100,100),"", "OpenPlanBTN")){
			//GameObject fileBrowser = new GameObject();
			//fileBrowser.name = "File Browser GameObject";
			//fileBrowser.AddComponent<FileBrowser>();
			//fileBrowser.GetComponent<FileBrowser>().mainMeunSkin = mainMeunSkin;
			//fileBrowser.GetComponent<FileBrowser>().planEditorSkin = planEditorSkin;
			//fileBrowser.GetComponent<FileBrowser>().mainMenuBg = mainMenuBg;
			//fileBrowser.GetComponent<FileBrowser>().furnitureIcon = furnitureIcon;
			//fileBrowser.GetComponent<FileBrowser>().LoadEditedPlanMenu();
			//Destroy(gameObject);
		}
		GUI.Label (new Rect(390,370,77,180), "Open - ", "FontBold");
		GUI.Label (new Rect(467,370,343,180), "Open previous created plans &", "Font");
		GUI.Label (new Rect(467,390,343,180), "edit them", "Font");
	}
	
	/// <summary>
	/// Called when user selected to start a "New" plan at the "Home Design" page
	/// Presets the plan activated.
	/// </summary>
	void CreateNewPlanActivated(){

		float titleW = 252.0f;
		float titleH = 30.0f;
		float titleX = dWidth * 0.5f - titleW * 0.65f;
		float titleY = 50f;

		GUI.Label (new Rect(titleX, titleY, titleW, titleH), "Create New Plan", "FontTitle");

		//Handles action when user pressed "Download New Plan"
		if(GUI.Button(new Rect(270,135,100,100),"", "NewPlanBTN")){

			//Load the scene "MainMap"
			sceneMon.GetComponent<SceneManager>().LoadHDAScene("MainMenu", "MainMap");
		}
		GUI.Label (new Rect(390,170,65,180), "Download new plan", "FontBold");

		//Handles action when user pressed "Use downloaded plans"
		if(GUI.Button(new Rect(270,265,100,100),"", "OpenPlanBTN")){
			currentMenu = 4;
		}
		GUI.Label (new Rect(390,310,77,180), "Use downloaded plans", "FontBold");
	}

	/// <summary>
	/// Demos the flat activated.
	/// </summary>
	void DemoFlatActivated(){
		float titleW = 252.0f;
		float titleH = 30.0f;
		float titleX = dWidth * 0.56f - titleW * 0.65f;
		float titleY = 50f;

		GUI.Label (new Rect(titleX, titleY, titleW, titleH), "Demo Plan", "FontTitle");

		if(GUI.Button(new Rect((1024.0f * 0.5f) - (370f * 0.5f), 150, 370, 50), "", "RR3_BTN"))
		{
			StartCoroutine(LoadFlat("RR3"));
	
			gameObject.SetActive(false);
		}

	}
	

	void ShoppingMenuActivated(){

		GUI.Label (new Rect(435,50,180,30), "Shopping", "FontTitle");
		if(GUI.Button(new Rect(270,135,100,100),"", "CatalogBTN")){
			Application.LoadLevel("catalog");
		}
		GUI.Label (new Rect(390,170,100,180), "Catalog - ", "FontBold");
		GUI.Label (new Rect(490,170,320,180), "Read our latest catalog", "Font");
		
		if(GUI.Button(new Rect(270,265,100,100),"", "ShoppingListBTN")){
			//
		}
		GUI.Label (new Rect(390,310,150,180), "Shopping List - ", "FontBold");
		GUI.Label (new Rect(540,310,343,180), "Open previous created", "Font");
		GUI.Label (new Rect(540,330,343,180), "Shopping Lists", "Font");
	}
}