using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GoldScript : MonoBehaviour {

    public static int Gold = 0;
    public Text goldText;
    public Text goldText2;

	// Use this for initialization
	void Start () {
        goldText.text = "Gold: " + Gold.ToString();
	}
	
	// Update is called once per frame
    
}
