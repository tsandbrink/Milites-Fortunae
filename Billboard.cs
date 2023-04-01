using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	public Camera main_Camera;
	// Use this for initialization


	// Update is called once per frame
	void Update () {
		transform.LookAt(transform.position + main_Camera.transform.rotation *Vector3.back,
		               main_Camera.transform.rotation * Vector3.up);
	}
}
