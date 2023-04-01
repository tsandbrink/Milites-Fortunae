using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroyLoadText : MonoBehaviour {

	public static Text loadingText;

	void Awake(){
		loadingText = gameObject.GetComponent<Text> ();
		DontDestroyOnLoad (transform.gameObject);
	}

}
