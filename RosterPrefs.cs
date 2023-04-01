using UnityEngine;
using System.Collections;

public class RosterPrefs : MonoBehaviour {

    // Use this for initialization
    //void Start () {
    //		PlayerPrefs.SetInt ("Magnus", 0);
    //}
    //PlayerPrefs Magnus;
	
	// Update is called once per frame
	

	public void MagnusOnRoster(){
        int magnusCost = 200;
        if (GoldScript.Gold > magnusCost)
        {
            PlayerPrefs.SetInt("Magnus", 1);
            GoldScript.Gold -= magnusCost;
        }
	}

    public void MariusOnRoster()
    {
        int mariusCost = 500;
        if (GoldScript.Gold > mariusCost)
        {
            PlayerPrefs.SetInt("Marius", 1);
            GoldScript.Gold -= mariusCost;
        }
    }
}
