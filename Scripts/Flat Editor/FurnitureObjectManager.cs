using UnityEngine;
using System.Collections;

public class FurnitureObjectManager : MonoBehaviour {

	public GUISkin skin;

	//*****************************
	//*                           *
	//*     Private variables     *
	//*                           *
	//*****************************
	private bool isMovable = false;
	private bool isMoving = false;
	private bool isSelected = false;

	private Ray ray;
	private RaycastHit rayHit;

	private Touch touch;
	private int touchCount;

	//***************************
	//*                         *
	//*     From Old Script     *
	//*                         *
	//***************************
	private float targetItemRotation;

	private float screenWidthRatio;
	private float screenHeightRatio;

	private float screenHeight = 768.0f;
	private float screenWidth = 1024.0f;

	//*********************
	//*                   *
	//*     Functions     *
	//*                   *
	//*********************
	
	private void Start() {
		screenHeightRatio = Screen.height / screenHeight;
		screenWidthRatio = Screen.width / screenWidth;
	}

	// Use this for initialization
	private void Update () {
		if (Input.touchCount > 0) {
			touch = Input.touches[0];
			touchCount = Input.touchCount;
		}

		if (isMoving) {
			//Stops the camera from moving while the furniture is being moved around
			Camera.main.GetComponent<FlatCameraControl> ().StopCamera ();

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out rayHit, 100, 1 << LayerMask.NameToLayer ("ground"))) {
				transform.position = rayHit.point + new Vector3 (0, rayHit.point.y, 0);
			}	
		} else { 
			//Resume the camera so it can move
			Camera.main.GetComponent<FlatCameraControl>().ResumeCamera();
		}

	}
	
	private void OnGUI() {
		GUI.skin = (GUISkin) Resources.Load("UI_Skin/PlanEditor");

		if (isMovable && Event.current.type == EventType.mouseDrag) {

		}

		if(isSelected)
			RotationSlider();

	}
	
	private void OnMouseDown()
	{
		//isMoving = true;
		isMovable = true;

		if(isSelected){
			DeselectAllFurniture();
			this.transform.tag = "Furniture_toBeDel";
		} else {
			this.transform.tag = "Furniture_normal";
		}
	}

	private void OnMouseUp()
	{
		isSelected = !isSelected;
		
		isMoving = false;
	}

	/// <summary>
	/// Change the tags of ALL previously SELECTED furniture object back to NORMAL
	/// </summary>
	private void DeselectAllFurniture()
	{
		GameObject[] selectedFurnitures = GameObject.FindGameObjectsWithTag("Furniture_toBeDel");

		for(int i = 0; i < selectedFurnitures.Length; i++)
		{
			selectedFurnitures[i].tag = "Furniture_normal";
		}
	}

	private void RotationSlider(){
		targetItemRotation = transform.localEulerAngles.y;

		//float rectLeft = Camera.main.WorldToScreenPoint (transform.position).x / screenWidthRatio - 90f;
		//float rectTop = 768f - (Camera.main.WorldToScreenPoint (transform.position).y / (screenHeightRatio - 80f));
		float rectLeft = Camera.main.WorldToScreenPoint (transform.position).x - 95f;
		float rectTop = Screen.height - Camera.main.WorldToScreenPoint (transform.position).y - 300f;

		targetItemRotation = GUI.HorizontalSlider(new Rect(rectLeft, rectTop, 190, 40), targetItemRotation, 0.0F, 359.9F, "CustomHorizontalSlider", "CustomSliderThumb");
						
		// To prevent rotating the object while moving
		if (!isMoving) {
			if(targetItemRotation > 30 && targetItemRotation < 60)
				targetItemRotation = 45;
			if(targetItemRotation > 75 && targetItemRotation < 105)
				targetItemRotation = 90;
			if(targetItemRotation > 120 && targetItemRotation < 150)
				targetItemRotation = 135;
			if(targetItemRotation > 165 && targetItemRotation < 195)
				targetItemRotation = 180;
			if(targetItemRotation > 210 && targetItemRotation < 240)
				targetItemRotation = 225;
			if(targetItemRotation > 255 && targetItemRotation < 285)
				targetItemRotation = 270;
			if(targetItemRotation > 300 && targetItemRotation < 330)
				targetItemRotation = 315;
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
			                                                        targetItemRotation, transform.localEulerAngles.z);
		}
	}

}
