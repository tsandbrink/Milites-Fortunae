using UnityEngine;
using System.Collections;

public class LayerSwitch : MonoBehaviour {
	public Transform thisCube;
	// Use this for initialization
	void Start () {
		gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE 
			|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
			thisCube.gameObject.SetActive (true);
		} 
		else if (//BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE ||
		         BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET ||
		         BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYATTACK
		         || BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYDECREASEFATIGUE
		         || BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYRECALCULATE){
			thisCube.gameObject.SetActive (false);
		}
	}
}
