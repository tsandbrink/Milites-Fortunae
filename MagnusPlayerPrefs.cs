using UnityEngine;
using System.Collections;

public class MagnusPlayerPrefs : MonoBehaviour {

    UnitStats Magnus;
    Unit Magnuss;
	// Use this for initialization
	void Awake () {
        Magnus = GetComponent<UnitStats>();
        Magnuss = GetComponent<Unit>();
        PlayerPrefs.SetFloat("MagMaxHealth", Magnus.maxHealth);
        PlayerPrefs.SetFloat("MagDefense", 4 + Magnus.armor.defense);
        PlayerPrefs.SetFloat("MagAccuracy", Magnus.accuracy);
        PlayerPrefs.SetInt("MagAttack", Magnus.attack);
        PlayerPrefs.SetInt("MagConstitution", Magnus.constitution);
        PlayerPrefs.SetInt("MagFatigue", Magnuss.maxFatigue);
        PlayerPrefs.SetInt("MagMaxMovement", Magnuss.moveRange / 2);
	}
	
}
