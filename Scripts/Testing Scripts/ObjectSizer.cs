using UnityEngine;
using System.Collections;

public class ObjectSizer : MonoBehaviour {
	//****************************
	//*                          *
	//*     Public Variables     *
	//*                          *
	//****************************

	public bool showHeight = true;
	public bool showWidth = true;
	public bool showDepth = true;

	//private float objHeight;
	//private float objWidth;

	// Use this for initialization
	void Start () {
	}

	void Update () {
		if(showHeight)
			print ("Object [" + this.gameObject.name + "] Height is = " + transform.renderer.bounds.extents.y);
		
		if(showWidth)
			print ("Object [" + this.gameObject.name + "]  Width is = " + transform.renderer.bounds.extents.x);
		
		if(showDepth)
			print ("Object [" + this.gameObject.name + "]  Depth is = " + transform.renderer.bounds.extents.z);
	}
}
