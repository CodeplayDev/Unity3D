using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public GameObject camera2D;
	public GameObject camera3D;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//*****************************************
	//*                                       *
	//*     Functions to activate cameras     *
	//*                                       *
	//*****************************************

	public void Activate2DCamera()
	{
		Deactivate3DCamera();
		camera2D.SetActive(true);
	}

	public void Activate3DCamera()
	{
		Deactivate2DCamera();
		camera3D.SetActive(true);
	}
	
	//*******************************************
	//*                                         *
	//*     Functions to deactivate cameras     *
	//*                                         *
	//*******************************************
	
	private void Deactivate2DCamera()
	{
		camera2D.SetActive(false);
	}
	
	private void Deactivate3DCamera()
	{
		camera3D.SetActive(false);
	}
	
}
