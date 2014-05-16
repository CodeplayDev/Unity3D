using UnityEngine;
using System.Collections;

public class FlatEditor : MonoBehaviour {

	// Update is called once per frame
	void Update () {

		transform.position = Input.mousePosition;

		Physics.Raycast(transform.position, transform.forward);
	}
}
