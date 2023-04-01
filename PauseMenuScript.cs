using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

	RawImage pauseMenu;
	Canvas quitMenu;

	void Start () {
		pauseMenu = gameObject.GetComponent<RawImage> ();
		pauseMenu.enabled = false;
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(false);
		transform.GetChild(3).gameObject.SetActive(false);
		transform.GetChild(4).gameObject.SetActive(false);
		transform.GetChild(5).gameObject.SetActive(false);
		transform.GetChild(6).gameObject.SetActive(false);
	}

	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			pauseMenu.enabled = true;
			transform.GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).gameObject.SetActive(true);
			transform.GetChild(2).gameObject.SetActive(true);
			transform.GetChild(3).gameObject.SetActive(true);
			transform.GetChild(4).gameObject.SetActive(true);
			transform.GetChild(5).gameObject.SetActive(true);
			transform.GetChild(6).gameObject.SetActive(true);
		}
	}

	public void BackToTitleScreen(){
		Application.LoadLevel (1);
	}

	public void closePanel(){
		pauseMenu.enabled = false;
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(false);
		transform.GetChild(3).gameObject.SetActive(false);
		transform.GetChild(4).gameObject.SetActive(false);
		transform.GetChild(5).gameObject.SetActive(false);
		transform.GetChild(6).gameObject.SetActive(false);
	}
		
}
