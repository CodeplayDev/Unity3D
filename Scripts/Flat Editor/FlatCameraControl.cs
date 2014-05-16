using UnityEngine;
using System.Collections;

public class FlatCameraControl : MonoBehaviour
{

	//****************************
	//*                          *
	//*     Public Variables     *
	//*                          *
	//****************************

	public float rotateSpeed = 0.04f;
	public float lateralFactor = 0;
	public float lateralForce = 10f;
	public float moveFactor = 0;
	public float moveForce = 10f;
	public float zoomFactor = 0.1f;
	public float zoomForce = 10f;

	public float minPinchSpeed = 5.0F;
	public float varianceInDistances = 5.0f;

	//*****************************
	//*                           *
	//*     Private Variables     *
	//*                           *
	//*****************************

	private bool camMoving = false;
	private bool camMovable = true;

	private float screenHeightRatio;
	private float screenWidthRatio;
	private float BGWidth = 2048.0f;
	private float BGHeight = 1536.0f;

	private Rect cameraRotateRect = new Rect(0,0,2048,1536);

	//Pinch Zoom variables
	private float touchDelta = 0.0F;
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);
	private float speedTouch0 = 0.0F;
	private float speedTouch1 = 0.0F;

	//*******************************
	//*                             *
	//*     Debug use variables     *
	//*                             *
	//*******************************
	
	//*********************
	//*                   *
	//*     Functions     *
	//*                   *
	//*********************

	// Use this for initialization
	void Start ()
	{
		screenWidthRatio = Screen.width / BGWidth;
		screenHeightRatio = Screen.height / BGHeight;
	}

	private void Update ()
	{
		//********************************
		//*                              *
		//*     Touch Camera Control     *
		//*                              *
		//********************************
		RotateCamera ();
		MoveCamera ();
		ZoomCamera();
	}

	void MoveCamera ()
	{
		if(Input.touchCount > 0){
			if(Camera.main.rigidbody.isKinematic)
				Camera.main.rigidbody.isKinematic = false;


			if(camMovable) {
			if(Input.touchCount == 2){

				//**********************
				//*                    *
				//*     Swipe Left     *
				//*                    *
				//**********************

				if(Input.GetTouch(0).deltaPosition.x < -5*screenWidthRatio && cameraRotateRect.Contains(Input.GetTouch(0).position) && Input.GetTouch(1).deltaPosition.x < -5*screenWidthRatio && cameraRotateRect.Contains(Input.GetTouch(1).position)){
					if(lateralFactor > -3){
						lateralFactor -= 0.1f;
						Camera.main.transform.rigidbody.AddRelativeForce(Vector3.right * -lateralForce);
					}
				}
				
				//***********************
				//*                     *
				//*     Swipe Right     *
				//*                     *
				//***********************

				if(Input.GetTouch(0).deltaPosition.x > 5*screenWidthRatio && cameraRotateRect.Contains(Input.GetTouch(0).position) && Input.GetTouch(1).deltaPosition.x > 5*screenWidthRatio && cameraRotateRect.Contains(Input.GetTouch(1).position)){
					if(lateralFactor < 3){
						lateralFactor += 0.1f;
						Camera.main.transform.rigidbody.AddRelativeForce(Vector3.right * lateralForce);
					}
				}
				
				//**************************
				//*                        *
				//*     Swipe Backward     *
				//*                        *
				//**************************

				if(Input.GetTouch(0).deltaPosition.y < -5*screenHeightRatio && cameraRotateRect.Contains(Input.GetTouch(0).position) && Input.GetTouch(1).deltaPosition.y < -5*screenHeightRatio && cameraRotateRect.Contains(Input.GetTouch(1).position)){
					if(moveFactor > -3){
						moveFactor -= 0.1f;
						Camera.main.transform.rigidbody.AddRelativeForce(Vector3.forward * -moveForce);
					}
				}

				//*************************
				//*                       *
				//*     Swipe Forward     *
				//*                       *
				//*************************

				if(Input.GetTouch(0).deltaPosition.y > 5*screenHeightRatio && cameraRotateRect.Contains(Input.GetTouch(0).position) && Input.GetTouch(1).deltaPosition.y > 5*screenHeightRatio && cameraRotateRect.Contains(Input.GetTouch(1).position)){
					if(moveFactor < 3){
						moveFactor += 0.1f;
						Camera.main.transform.rigidbody.AddRelativeForce(Vector3.forward * moveForce);
					}
				}
			}
			}
		} else {
			if(!Camera.main.rigidbody.isKinematic)
				Camera.main.rigidbody.isKinematic = true;
		}
	}

	void RotateCamera ()
	{
		//Single touch rotation
		if (Input.touchCount == 1) {
			if (camMovable) {
				Camera.main.transform.Rotate (-Input.GetTouch (0).deltaPosition.y * rotateSpeed, Input.GetTouch (0).deltaPosition.x * rotateSpeed, 0);
				Camera.main.transform.localRotation = Quaternion.Euler (Camera.main.transform.localRotation.eulerAngles.x, Camera.main.transform.localRotation.eulerAngles.y, 0);
			}
		}
	}

	public void ResumeCamera() {
		camMovable = true;
	}

	public void StopCamera() {
		camMovable = false;
	}

	void ToggleCameraMove() {
		camMovable = !camMovable;
	}

	void ZoomCamera()
	{
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
		{
			if(camMovable) {
				curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
				prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
				touchDelta = curDist.magnitude - prevDist.magnitude;
				speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
				speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
			
			
				//OUT
				if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
				{
					Camera.main.transform.Translate(Vector3.forward * -zoomFactor);
				}
			
				//IN
				if ((touchDelta +varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
				{
					Camera.main.transform.Translate(Vector3.forward * zoomFactor);
				}
			}
		}     
	}
}
