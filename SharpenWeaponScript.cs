using UnityEngine;
using System.Collections;

public class SharpenWeaponScript : MonoBehaviour {

    public static int Sharpened = 0;
    public int SharpeningCost;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void VolundSharpenWeapons()
    {
        if (GoldScript.Gold > SharpeningCost)
        {
            Sharpened++;
        }
    }
}
