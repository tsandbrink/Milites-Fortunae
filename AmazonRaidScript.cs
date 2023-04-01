using UnityEngine;
using System.Collections;

public class AmazonRaidScript : MonoBehaviour {

	public Unit Appolinia;
	public Unit Arachne;
    public SelectUnits rosterManager;

	// Use this for initialization
    void Start(){
        rosterManager.Clear();
    }

	// Update is called once per frame
	void Update () {
		if (BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			if (Appolinia.health <= 0 || Arachne.health <= 0) {
				BattleStateMachine.currentState = BattleStateMachine.BattleStates.LOSE;
			}
		}
	}
}
