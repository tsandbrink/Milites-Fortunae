using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseLoadingScreen : MonoBehaviour {

	Canvas loadScreen;
	Text loadText;
	public Canvas GeneralUI;
	public Canvas ContinueButtonCanvas;

	void Start(){
		loadScreen = DontDestroyLoadScreen.thisCanvas;
		loadScreen.sortingOrder = 2;
		ContinueButtonCanvas.sortingOrder = 3;
		GeneralUI.sortingOrder = 1;
		loadText = DontDestroyLoadText.loadingText;
		loadText.gameObject.SetActive (false);
	}

	public void closeLoadScreen(){
		loadScreen.gameObject.SetActive (false);
		gameObject.SetActive (false);
		ContinueButtonCanvas.gameObject.SetActive (false);
	}
}
