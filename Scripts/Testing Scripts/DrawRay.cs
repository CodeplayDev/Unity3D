using UnityEngine;
using System.Collections;

public class DrawRay : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, transform.forward, Color.red);
	}
}
