using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	//****************************
	//*  		   			     *
	//*     Public variables     *
	//*  						 *
	//****************************

	public bool monitorOn = false;
	public bool mouseOn = false;
	public bool versionOn = true;

	public GUISkin skin;

	public string verNum = "version";

	//*****************************
	//*  						  *
	//*     Private variables     *
	//*  						  *
	//*****************************
	
	private bool backToMap = true;

	private Camera mainCam;

	private string consoleMsg;
	private string districtName;
	private string estateName;
	private string floorName;
	private string flatName;
	private string mapTextureName;
	private string xmlPath;

	private float hitX = 0;
	private float hitY = 0;
	private float hitZ = 0;

	private RaycastHit rayHit;
	private RaycastHit camRayHit;

	private Ray ray;
	private Ray camRay;

	//*********************
	//*     			  *
	//*     Functions     *
	//*     			  *
	//*********************

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		districtName = "";
		estateName = "";
		floorName = "";
	}

	private void OnGUI(){
		GUI.skin = skin;

		if(monitorOn){
			GUI.Label (new Rect(50, 10, 200, 20), "   Scene: " + Application.loadedLevelName);
			GUI.Label (new Rect(50, 30, 200, 20), "District: " + districtName);
			GUI.Label (new Rect(50, 50, 200, 20), "  Estate: " + estateName);
			GUI.Label (new Rect(50, 70, 200, 20), "   Floor: " + floorName);
			GUI.Label (new Rect(50, 90, 200, 20), "    Flat: " + flatName);

			GUI.Label (new Rect(50, 120, 1200, 20), " XMLPath: " + xmlPath);
			GUI.Label (new Rect(50, 150, 1200, 20), " MapText: " + mapTextureName);

			GUI.Label (new Rect(50, 180, 1200, 20), " Console: " + consoleMsg);
		}

		if(mouseOn)
		{
			GUI.Label (new Rect(400, 10, 200, 20), "Mouse_X: " + Input.mousePosition.x);
			GUI.Label (new Rect(400, 30, 200, 20), "Mouse_Y: " + Input.mousePosition.y);
			GUI.Label (new Rect(400, 50, 200, 20), "Mouse_Z: " + Input.mousePosition.z);
			GUI.Label (new Rect(400, 70, 200, 20), "  Hit_X: " + hitX);
			GUI.Label (new Rect(400, 90, 200, 20), "  Hit_Y: " + hitY);
			GUI.Label (new Rect(400, 110, 200, 20), "  Hit_Z: " + hitZ);

			ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
			camRay = Camera.main.ScreenPointToRay(Camera.main.transform.position);

			if(Physics.Raycast(ray, out rayHit, 100))
			{
				hitX = rayHit.point.x;
				hitY = rayHit.point.y;
				hitZ = rayHit.point.z;
			}
		}

		if(versionOn)
		{
			GUI.Label (new Rect(20, 440, 200, 20), verNum);
		}
	}

	private void Update()
	{
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue);
		Debug.DrawRay (camRay.origin, camRay.direction * 10, Color.yellow);
	}

	//****************************
	//*                          *
	//*     Getter functions     *
	//*                          *
	//****************************

	public string GetDistrictName()
	{
		return districtName;
	}
	
	public string GetEstateName()
	{
		return estateName;
	}
	
	public string GetFloorName()
	{
		return floorName;
	}
	
	public string GetFlatName()
	{
		return flatName;
	}

	public bool isBackToMap()
	{
		return backToMap;
	}

	//****************************
	//*                          *
	//*     Setter functions     *
	//*                          *
	//****************************

	public void SetDistrictName(string name)
	{
		districtName = name;
	}

	public void SetEstateName(string name)
	{
		estateName = name;
	}
	
	public void SetFloorName(string name)
	{
		floorName = name;
	}

	public void SetFlatName(string name)
	{
		flatName = name;
	}

	public void SetMapTextureName(string name)
	{
		mapTextureName = name;
	}

	public void SetConsoleMsg(string msg)
	{
		consoleMsg = msg;
	}

	public void SetXMLPath(string path)
	{
		xmlPath = path;
	}

	public void GoBackToMain()
	{
		backToMap = false;
	}
	
	public void GoBackToMa()
	{
		backToMap = true;
	}

	//***************************
	//*                         * 
	//*     Other functions     *
	//*                         *
	//***************************

	/// <summary>
	/// Loads the HDA scene.
	/// </summary>
	/// <param name="cScene">Current scene.</param>
	/// <param name="nScene">New scene.</param>
	public void LoadHDAScene(string cScene, string nScene)
	{
		if(Application.GetStreamProgressForLevel(cScene) == 1 && Application.GetStreamProgressForLevel(nScene) == 1){
			Application.LoadLevel(nScene);
		}
	}
}
