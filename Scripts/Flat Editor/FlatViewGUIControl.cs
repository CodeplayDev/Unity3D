using UnityEngine;
using System.Collections;
using System.IO;

public class FlatViewGUIControl : MonoBehaviour
{
	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public GameObject camManager;
	public GUISkin skin;

	public bool isDemo = false;
	public bool backToMap = true;

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************

	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;

	private float screenHeightRatio;
	private float screenWidthRatio;

	private DirectoryInfo dir;

	private FileInfo[] fis;

	private bool mode3D = false;
	private bool modePaint = false;
	private bool modeFurniture = false;

	private bool view_Beds = true;
	private bool view_Chairs = false;
	private bool view_Tables = false;
	private bool view_Lightings = false;
	private bool view_Wall = true;

	private Touch touch;
	private int touchCount;

	private Vector2 scrollPosition = Vector2.zero;

   //**********************************
   //*                                *
   //*     Variables for ALL Menu     *
   //*                                *
   //**********************************

	public Vector2 buttonMargin;
   public Vector2 buttonSize;
   public Vector2 menuMargin;
	public Vector2 menuSize;
   public Vector2 screenMargin;

	//**********************************
	//*                                *
	//*     Variables for BED Menu     *
	//*                                *
	//**********************************

	private ArrayList names_bed;
	private ArrayList textureList_bed;

	private float listWidth_bed;

	private int itemCnt_bed;

	//************************************
	//*                                  *
	//*     Variables for CHAIR Menu     *
	//*                                  *
	//************************************
	
	private ArrayList names_chair;
	private ArrayList textureList_chair;
	
	private float listWidth_chair;
	
	private int itemCnt_chair;
	
	//************************************
	//*                                  *
	//*     Variables for TABLE Menu     *
	//*                                  *
	//************************************
	
	private ArrayList names_table;
	private ArrayList textureList_table;
	
	private float listWidth_table;
	
	private int itemCnt_table;
	
	//***************************************
	//*                                     *
	//*     Variables for LIGHTING Menu     *
	//*                                     *
	//***************************************
	
	private ArrayList names_lighting;
	private ArrayList textureList_lighting;
	
	private float listWidth_lighting;
	
	private int itemCnt_lighting;
	
	//****************************************
	//*                                      *
	//*     Variables for WALLPAPER Menu     *
	//*                                      *
	//****************************************
	
	private ArrayList names_wall;
	private ArrayList textureList_wall;
	private Rect[] iconRect_wall;

	private float listWidth_wall;
	
	private int itemCnt_wall;

	private Texture2D selectedWallTexture;

	//*************************************
	//*                                   *
	//*     Ray for dragging detection    *
	//*                                   *
	//*************************************

	private Ray rayToFloor;
	private Ray rayToWall;

	private RaycastHit rayToFloorHit;
	private RaycastHit rayToWallHit;

	//************************************
	//*                                  *
	//*    Targets for drag-and-drop     *
	//*                                  *
	//************************************

	private GameObject targetWall;

	//*****************************
	//*                           *
	//*     Testing variables     *
	//*                           *
	//*****************************

	public Texture2D testWallpaper;

	private bool isDraggingWall = false;
	
	//*********************
	//*                   *
	//*     Functions     *
	//*                   *
	//*********************

	private void Start()
	{
		screenHeightRatio = Screen.height / BGHeight;
		screenWidthRatio = Screen.width / BGWidth;

		if(!isDemo){
			//Initialize the setup details for Item Menus
			SetupMenuBed();
			SetupMenuChair();
			SetupMenuLighting();
			SetupMenuTable();
			SetupMenuWallpaper();
		}
	}

	private void Update()
	{
		//Initialize the Rays for drag-and-drop detection
		rayToFloor = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
		rayToWall = Camera.main.ScreenPointToRay (Input.mousePosition);

		//Controls the dragging of the menus
		if(Input.touchCount > 0) {
			touch = Input.touches[0];
			touchCount = Input.touchCount;

			bool tInsideMenu = IsTouchInsideMenu(touch.position);		//Check if the touch is within the menu area
		
			if(touch.phase == TouchPhase.Moved && tInsideMenu)
			{
				scrollPosition.x += touch.deltaPosition.x;
			}
		}

		//
	}

	private void OnGUI ()
	{
		GUI.skin = skin;

		GUI.matrix = Matrix4x4.TRS (new Vector3 (0, 0, 0), Quaternion.identity, new Vector3 (screenWidthRatio, screenHeightRatio, 1));

		if(targetWall != null)
			GUI.Label(new Rect(0, 50, 300, 30), "TargetWall: " + targetWall.name);

		//Back button
		if(GUI.Button(new Rect(1800, 100, 200, 200), "", "Back"))
		{
			if(backToMap){
				//Load Map scene
				Application.LoadLevel("Map_Floor");
			} else {
				//Load Main Menu Scene
				Application.LoadLevel ("MainMenu");
			}
		}

		if (mode3D) {
			//************************************
			//*                                  *
			//*     Activate the 3D view GUI     *
			//*                                  *
			//************************************

			if (GUI.Button (new Rect (1800, 350, 200, 200), "", "2DBtn")) {
				mode3D = false;
				camManager.GetComponent<CameraManager>().Activate2DCamera();
			}
		} else {
			//************************************
			//*                                  *
			//*     Activate the 2D view GUI     *
			//*                                  *
			//************************************

			if (GUI.Button (new Rect (1800, 350, 200, 200), "", "3DBtn")) {
				mode3D = true;
				camManager.GetComponent<CameraManager>().Activate3DCamera();
			}
		}

		if (GUI.Button (new Rect (1800, 600, 200, 200), "", "TrashBtn")) {
			GameObject[] trashObj = GameObject.FindGameObjectsWithTag("Furniture_toBeDel");

			for(int i = 0; i < trashObj.Length; i++)
			{
				Destroy(trashObj[i]);
			}
		}

		//*************************************************
		//*                                               *
		//*     EDIT ONLY allow when NOT in demo flat     *
		//*                                               *
		//*************************************************
		if(!isDemo){
			//The Paint button
			if(GUI.Button(new Rect(50, 100, 200, 200), "", "PaintBtn"))
			{
				modeFurniture = false;
				modePaint = !modePaint;
				ResetAllCategoryView();
				view_Wall = true;
			}
			
			//The Furniture button
			if(GUI.Button(new Rect(50, 350, 200, 200), "", "FurnitureBtn"))
			{
				modePaint = false;
				modeFurniture = !modeFurniture;
				ResetAllCategoryView();
				view_Beds = true;
			}
						
			//************************
			//*                      *
			//*    In Paint Mode     *
			//*                      *
			//************************
			if(modePaint){

				//Create category buttons
				if(GUI.Button(new Rect(0, 1140, 300, 100), "Wallpaper", "CategoryBtn"))
				{
					ResetAllCategoryView();
					view_Wall = true;
				}

				//Create the PAINT Item Menus
				CreateMenuWallpaper();


				if(Event.current.type == EventType.mouseUp) {
					targetWall = null;
					selectedWallTexture = null;
				}
				
				if(targetWall != null && selectedWallTexture != null)
					targetWall.transform.renderer.material.mainTexture = selectedWallTexture;

				if(Event.current.type == EventType.mouseDrag) {
					if(Physics.Raycast(rayToWall, out rayToWallHit, 100, 1 << LayerMask.NameToLayer("wall"))){
						targetWall = rayToWallHit.collider.gameObject;
					}
				}

				if(touchCount > 0) {
					if(touch.phase == TouchPhase.Moved) {
						if(Physics.Raycast(rayToWall, out rayToWallHit, 100, 1 << LayerMask.NameToLayer("wall"))){
							targetWall = rayToWallHit.collider.gameObject;
						}
					}

					if(touch.phase == TouchPhase.Ended) {
						targetWall = null;
						selectedWallTexture = null;
					}

					if(selectedWallTexture != null) 
						GUI.DrawTexture(new Rect(touch.position.x, Screen.height - touch.position.y, 300f, 300f), selectedWallTexture);
				}

			}

			//****************************
			//*                          *
			//*    In Furniture Mode     *
			//*                          *
			//****************************
			if(modeFurniture){

				//Create category buttons
				if(GUI.Button(new Rect(0, 1140, 300, 100), "Beds", "CategoryBtn"))
				{
					ResetAllCategoryView();
					view_Beds = true;
				}

				if(GUI.Button(new Rect(300, 1140, 300, 100), "Chairs", "CategoryBtn"))
				{
					ResetAllCategoryView();
					view_Chairs = true;
				}
				
				if(GUI.Button(new Rect(600, 1140, 300, 100), "Tables", "CategoryBtn"))
				{
					ResetAllCategoryView();
					view_Tables = true;
				}

				if(GUI.Button(new Rect(900, 1140, 300, 100), "Lights", "CategoryBtn"))
				{
					ResetAllCategoryView();
					view_Lightings = true;
				}

				//Draw the FURNITURE Item Menus
				CreateMenuBed();
				CreateMenuChair();
				CreateMenuLighting();
				CreateMenuTable();

			}
		}
	}

	private void ResetAllCategoryView()
	{
		view_Beds = false;
		view_Chairs = false;
		view_Tables = false;
		view_Lightings = false;
		view_Wall = false;
	}

	//*****************************************************************
	//*                                                               *
	//*     Visualisation functions for Furniture and Paint Menus     *
	//*                                                               *
	//*****************************************************************

	private void CreateMenuBed()
	{
		//*************************************
		//*     Generate the list of BEDS     *
		//*************************************
		
		if(view_Beds){
			scrollPosition = GUI.BeginScrollView (new Rect (0,1236,2048,300), scrollPosition, new Rect (0, 0, listWidth_bed, 280));
			
			for(int i = 0; i < itemCnt_bed; i++)
			{
				if(GUI.Button(new Rect(300f * i, 0, 300, 300), (Texture2D)textureList_bed[i]))
				{
					rayToFloor = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
					
					//Vector3 offY = new Vector3(0, testPrefab.renderer.bounds.extents.y * 0.5f, 0);
					
					if(Physics.Raycast(rayToFloor, out rayToFloorHit, 100))
					{
						//Instantiate(testPrefab, rayHit.point + offY, Quaternion.identity);
					}
				}
			}
			
			// End the scroll view that we began above.
			GUI.EndScrollView ();
		}
	}

	private void CreateMenuChair()
	{
		//***************************************
		//*     Generate the list of CHAIRS     *
		//***************************************
		
		if(view_Chairs){
			scrollPosition = GUI.BeginScrollView (new Rect (0,1236,2048,300), scrollPosition, new Rect (0, 0, listWidth_chair, 280));
			
			for(int i = 0; i < itemCnt_chair; i++)
			{
				if(GUI.Button(new Rect(300f * i, 0, 300, 300), (Texture2D)textureList_chair[i]))
				{
					GameObject instance = Instantiate(Resources.Load("Furniture Models/Chairs/" + names_chair[i])) as GameObject;
					instance.AddComponent<FurnitureObjectManager>();
					instance.transform.rotation = Quaternion.identity;
					
					instance.AddComponent<Rigidbody>();
					instance.rigidbody.freezeRotation = true;
					instance.rigidbody.isKinematic = true;

					instance.AddComponent<BoxCollider>();
					
					Vector3 offY = new Vector3(0, instance.renderer.bounds.extents.y * 0.2f, 0);
					
					if(Physics.Raycast(rayToFloor, out rayToFloorHit, 100))
					{
						instance.transform.position = rayToFloorHit.point;
					}
				}
			}
			
			// End the scroll view that we began above.
			GUI.EndScrollView ();
		}
	}

	private void CreateMenuLighting()
	{
		//******************************************
		//*     Generate the list of LIGHTINGS     *
		//******************************************
		
		if(view_Lightings){
			scrollPosition = GUI.BeginScrollView (new Rect (0,1236,2048,300), scrollPosition, new Rect (0, 0, listWidth_lighting, 280));
			
			for(int i = 0; i < itemCnt_lighting; i++)
			{
				if(GUI.Button(new Rect(300f * i, 0, 300, 300), (Texture2D)textureList_lighting[i]))
				{
					rayToFloor = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
					
					//Vector3 offY = new Vector3(0, testPrefab.renderer.bounds.extents.y * 0.5f, 0);
					
					if(Physics.Raycast(rayToFloor, out rayToFloorHit, 100))
					{
						//Instantiate(testPrefab, rayHit.point + offY, Quaternion.identity);
					}
				}
			}
			
			// End the scroll view that we began above.
			GUI.EndScrollView ();
		}
	}

	private void CreateMenuTable()
	{
		//***************************************
		//*     Generate the list of TABLES     *
		//***************************************
		
		if(view_Tables){
			scrollPosition = GUI.BeginScrollView (new Rect (0,1236,2048,300), scrollPosition, new Rect (0, 0, listWidth_table, 280));
			
			for(int i = 0; i < itemCnt_table; i++)
			{
				if(GUI.Button(new Rect(300f * i, 0, 300, 300), (Texture2D)textureList_table[i]))
				{
					rayToFloor = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
					
					GameObject instance = Instantiate(Resources.Load("Furniture Models/Tables/Table01")) as GameObject;
					instance.transform.rotation = Quaternion.identity;
					
					Vector3 offY = new Vector3(0, instance.renderer.bounds.extents.y * 0.5f, 0);
					
					if(Physics.Raycast(rayToFloor, out rayToFloorHit, 100))
					{
						//Instantiate(testPrefab, rayHit.point + offY, Quaternion.identity);
						instance.transform.position = rayToFloorHit.point + offY;
						
					}
				}
			}
			
			// End the scroll view that we began above.
			GUI.EndScrollView ();
		}
	}

	private void CreateMenuWallpaper()
	{
		//*******************************************
		//*     Generate the list of WALLPAPERS     *
		//*******************************************
		
		if(view_Wall){
			//scrollPosition = GUI.BeginScrollView (new Rect (0,1236,2048,300), scrollPosition, new Rect (0, 0, listWidth_wall, 280));
			scrollPosition = GUI.BeginScrollView (new Rect (screenMargin.x, screenMargin.y, menuSize.x, menuSize.y), scrollPosition, new Rect (0, 0, listWidth_wall, buttonSize.x-(buttonMargin.y * 2f)));

			for(int i = 0; i < itemCnt_wall; i++)
			{
				iconRect_wall[i] = new Rect(300f * i, 0, buttonSize.x, buttonSize.y);

				GUI.DrawTexture(iconRect_wall[i], (Texture2D)textureList_wall[i]);

				if(iconRect_wall[i].Contains(Event.current.mousePosition))
					selectedWallTexture = (Texture2D)textureList_wall[i];

				if(touchCount > 0) {
					if(iconRect_wall[i].Contains(touch.position)) {
						selectedWallTexture = (Texture2D)textureList_wall[i];
					}
				}

			}
			
			// End the scroll view that we began above.
			GUI.EndScrollView ();
		}

		if(Event.current.type == EventType.mouseUp)
			selectedWallTexture = null;

	}


	//*********************************************************
	//*                                                       *
	//*     Setup Functions for Furniture and Paint Menus     *
	//*                                                       *
	//*********************************************************

	private void SetupMenuBed()
	{
		//***********************************
		//*     Setup for BED list menu     *
		//***********************************
		textureList_bed = new ArrayList();
		names_bed = new ArrayList();

		Object[] tempList = Resources.LoadAll("GUI/Furniture/Beds");

		foreach(Object obj in tempList) {
			Texture2D nTexture = (Texture2D) Resources.Load("GUI/Furniture/Beds/" + obj.name);
			textureList_bed.Add (nTexture);
			itemCnt_bed++;

			names_bed.Add(obj.name);
		}

		listWidth_bed = itemCnt_bed * 300.0f;
	}

	private void SetupMenuChair()
	{
		//*************************************
		//*     Setup for CHAIR list menu     *
		//*************************************
		textureList_chair = new ArrayList();
		names_chair = new ArrayList();

		Object[] tempList = Resources.LoadAll("GUI/Furniture/Chairs");
		
		foreach(Object obj in tempList) {
			Texture2D nTexture = (Texture2D) Resources.Load("GUI/Furniture/Chairs/" + obj.name);
			textureList_chair.Add (nTexture);
			itemCnt_chair++;

			names_chair.Add (obj.name);
		}

		listWidth_chair = itemCnt_chair * 300.0f;
	}

	private void SetupMenuLighting()
	{
		//****************************************
		//*     Setup for LIGHTING list menu     *
		//****************************************
		textureList_lighting = new ArrayList();
		names_lighting = new ArrayList();

		Object[] tempList = Resources.LoadAll("GUI/Furniture/Lightings");
		
		foreach(Object obj in tempList) {
			Texture2D nTexture = (Texture2D) Resources.Load("GUI/Furniture/Lightings/" + obj.name);
			textureList_lighting.Add (nTexture);
			itemCnt_lighting++;
			
			names_lighting.Add (obj.name);
		}
		

		listWidth_lighting = itemCnt_lighting * 300.0f;
	}

	private void SetupMenuTable()
	{
		//*************************************
		//*     Setup for TABLE list menu     *
		//*************************************
		textureList_table = new ArrayList();
		names_table = new ArrayList();
		
		Object[] tempList = Resources.LoadAll("GUI/Furniture/Tables");
		
		foreach(Object obj in tempList) {
			Texture2D nTexture = (Texture2D) Resources.Load("GUI/Furniture/Tables/" + obj.name);
			textureList_table.Add (nTexture);
			itemCnt_table++;
			
			names_table.Add (obj.name);
		}
		

		listWidth_table = itemCnt_table * 300.0f;
	}

	private void SetupMenuWallpaper() {
		//*****************************************
		//*     Setup for WALLPAPER list menu     *
		//*****************************************
		textureList_wall = new ArrayList();
		names_wall = new ArrayList();
		
		Object[] tempList = Resources.LoadAll("GUI/Furniture/Wallpaper");
		
		foreach(Object obj in tempList) {
			Texture2D nTexture = (Texture2D) Resources.Load("GUI/Furniture/Wallpaper/" + obj.name);
			textureList_wall.Add (nTexture);
			itemCnt_wall++;
			
			names_wall.Add (obj.name);
		}

		listWidth_wall = itemCnt_wall * 300.0f;

		iconRect_wall = new Rect[itemCnt_wall];
	}

	//******************************
	//*                            *
	//*     Dragging functions     *
	//*                            *
	//******************************

	private void DragWallpaper() {

	}

	//********************************
	//*                              *
	//*     Menu Scroll Movement     *
	//*                              *
	//********************************

	private bool IsTouchInsideMenu(Vector2 touchPos) 
	{
		Vector2 screenPos    = new Vector2(touchPos.x, Screen.height - touchPos.y);  // invert y coordinate
		Rect rAdjustedBounds = new Rect(screenMargin.x, screenMargin.y, menuSize.x, menuSize.y);
		
		return rAdjustedBounds.Contains(screenPos);
	}

}
