using UnityEngine;
using System.Collections;

public class TownRaidWinScript : MonoBehaviour {

	public EnemyUnit Leader;

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			if (Leader.health <= 0) {
				BattleStateMachine.currentState = BattleStateMachine.BattleStates.WIN;
			}
		}
	}
}
