using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccPowDef : MonoBehaviour {

	public Unit[] playerUnits;


	// Use this for initialization

	
	// Update is called once per frame

	void Start(){
		foreach (Unit u in playerUnits) {
			u.powImage.gameObject.SetActive (false);
			u.defImage.gameObject.SetActive (false);
		}
	}

	public void accuracy(){
		foreach (Unit u in playerUnits) {
			if (u.isSelected == 1 && u.gameObject.activeInHierarchy == true && u.acc == 0
				&& SoldierAttack.hasAttacked == 0
				&& StandardAttack.hasAttacked == 0
				&& PlayerDualSwordsAttack.hasAttacked == 0) {
				u.thisUnitStats.accuracy += 2;
				if (u.pow == 1) {
					u.thisUnitStats.attack -= 1;
				} else if (u.def == 1) {
					u.thisUnitStats.defense -= 1;
				}
				u.acc = 1;
				u.pow = 0;
				u.def = 0;
				u.accImage.gameObject.SetActive (true);
				u.powImage.gameObject.SetActive (false);
				u.defImage.gameObject.SetActive (false);
			}
		}
	}

	public void power(){
		foreach (Unit u in playerUnits) {
			if (u.isSelected == 1 && u.gameObject.activeInHierarchy == true && u.pow == 0
				&& SoldierAttack.hasAttacked == 0
				&& StandardAttack.hasAttacked == 0
				&& PlayerDualSwordsAttack.hasAttacked == 0) {
				if (u.acc == 1) {
					u.thisUnitStats.accuracy -= 2;
					u.thisUnitStats.attack += 1;
				} else if (u.def == 1) {
					u.thisUnitStats.attack += 1;
					u.thisUnitStats.defense -= 1;
				}
				u.acc = 0;
				u.pow = 1;
				u.def = 0;
				u.accImage.gameObject.SetActive (false);
				u.powImage.gameObject.SetActive (true);
				u.defImage.gameObject.SetActive (false);
			}
		}
	}

	public void defense(){
		foreach (Unit u in playerUnits) {
			if (u.isSelected == 1 && u.gameObject.activeInHierarchy == true && u.def == 0
				&& SoldierAttack.hasAttacked == 0
				&& StandardAttack.hasAttacked == 0
				&& PlayerDualSwordsAttack.hasAttacked == 0) {
				if (u.acc == 1) {
					u.thisUnitStats.accuracy -= 2;
				} else if (u.pow == 1) {
					u.thisUnitStats.attack -= 1;
				}
				u.thisUnitStats.defense += 1;
				u.acc = 0;
				u.pow = 0;
				u.def = 1;
				u.accImage.gameObject.SetActive (false);
				u.powImage.gameObject.SetActive (false);
				u.defImage.gameObject.SetActive (true);
			}
		}
	}
}
