using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EscapeScript : MonoBehaviour {

	public Unit[] Units;
	public GameObject[] escapeCubes;
	public Button EscapeButton;
	public Button DontEscapeButton;
	public Text EscapeText;
	public static int x;

	// Use this for initialization
	void Start () {
		EscapeButton.gameObject.SetActive (false);
		DontEscapeButton.gameObject.SetActive (false);
		EscapeText.gameObject.SetActive (false);
		x = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK ||
		    BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE 
			&& x == 0) {
			foreach (Unit u in Units) {
				if (checkForCanEscape (u) == true && u.isSelected == 1) {
					EscapeButton.gameObject.SetActive (true);
					DontEscapeButton.gameObject.SetActive (true);
					EscapeText.gameObject.SetActive (true);
				}
			}
		}
	}

	public void Escape(){
		foreach (Unit u in Units) {
			if (checkForCanEscape (u) == true && u.isSelected == 1) {
				u.thisUnitStats.health = 0;
				u.thisUnitStats.die ();
			}
		}
		EscapeButton.gameObject.SetActive (false);
		DontEscapeButton.gameObject.SetActive (false);
		EscapeText.gameObject.SetActive (false);
		x++;
	}

	public void DontEscape(){
		EscapeButton.gameObject.SetActive (false);
		DontEscapeButton.gameObject.SetActive (false);
		EscapeText.gameObject.SetActive (false);
		x++;
	}

	bool checkForCanEscape(Unit u){
		int x = 0;

			foreach (GameObject g in escapeCubes) {
				if (u.transform.position.x == g.transform.position.x
				    && u.transform.position.z == g.transform.position.z
				    && u.gameObject.activeInHierarchy == true
				    && u.health > 0) {
					x++;
				}
			}

		if (x > 0) {
			return true;
		} else {
			return false;
		}
	}
}
