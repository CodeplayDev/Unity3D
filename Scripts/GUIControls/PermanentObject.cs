using UnityEngine;
using System.Collections;

public class PermanentObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.root.gameObject);
	}
}
