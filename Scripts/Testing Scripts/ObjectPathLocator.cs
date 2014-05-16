using UnityEngine;
using System.Collections;
using System.IO;

public class ObjectPathLocator : MonoBehaviour {

	public Texture2D testTexture;

	public GUISkin skin;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = skin;

		GUI.Label (new Rect(50, 10, 200, 20), "   Path: ");

	}
}
