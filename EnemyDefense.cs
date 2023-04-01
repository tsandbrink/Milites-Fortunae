using UnityEngine;
using System.Collections;

public class EnemyDefense : MonoBehaviour {

	public static float defense;
	public EnemyArmor EnemyArmor;

	// Use this for initialization
	void Start () {
		defense = 4 + EnemyArmor.defense;
	}
	
	// Update is called once per frame

}
