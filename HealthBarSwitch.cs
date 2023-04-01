using UnityEngine;
using System.Collections;

public class HealthBarSwitch : MonoBehaviour {
	public Unit parent;
	public SelectUnits rosterManager;
	public GameObject healthBar;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (rosterManager.unitsInScene.Contains(parent)) {
			healthBar.SetActive (true);
		} 
		else {
			healthBar.SetActive (false);
		}
	}
}
