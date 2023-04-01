using UnityEngine;
using System.Collections;

public class TurnOffWalkables : MonoBehaviour {

	public GameObject[] Unwalkables;
	public GameObject[] playerUnwalkableCubes;

	// Use this for initialization
	//void Start () {
	
	//}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	public void TurnOffUnwalkables(){
		foreach (GameObject g in Unwalkables) {
			g.SetActive (false);
		}
	}

	public void TurnOnUnwalkables(){
		foreach (GameObject g in Unwalkables) {
			g.SetActive (true);
		}
	}

	public void TurnOffPlayerUnwalkables(){
		foreach (GameObject g in playerUnwalkableCubes) {
			g.SetActive (false);
		}
	}

	public void TurnOnPlayerUnwalkables(){
		foreach (GameObject g in playerUnwalkableCubes) {
			g.SetActive (true);
		}
	}
}
