using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroyLoadScreen : MonoBehaviour {

	public static Canvas thisCanvas;

	void Awake () {
		thisCanvas = gameObject.GetComponent<Canvas> ();
		DontDestroyOnLoad(transform.gameObject);
	}
}
