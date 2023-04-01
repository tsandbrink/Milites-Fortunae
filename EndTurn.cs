using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndTurn : MonoBehaviour {


	public Unit thisUnit;
	public Unit[] Units;
	public EnemyUnit[] enemyunits;
    public Pathfinding pathfinding;

    public static int turns; //Part of Reward Formula


	public Button endTurnButton;

	// Use this for initialization
    void Start()    {
        turns = 0; 
    }
	
	// Update is called once per frame
	void Update(){
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYATTACK
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYDECREASEFATIGUE
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYRECALCULATE) {
			endTurnButton.interactable = false;
		}
	}


	public void endTurnFunction1(){
		Invoke ("endTurnFunction", 1.5f);
	}

	public void endTurnFunction(){
        EnemyTarget.blocker = 0;
		ThiefAttack.mineStop = false;
        pathfinding.NodeList.Clear();
		pathfinding.EnemyAI.i = 0;
		foreach (Node n in pathfinding.grid.grid) {
			n.NodeScore = 0;
		}

		//BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYFINDTARGET;
		//Debug.Log ("ended1");
        if (thisUnit.fatigue < 0){
				thisUnit.fatigue = 0;
			}

			foreach (Unit u in Units){
				u.fatigue += 1;
				u.hasMoved = 0;
				u.isMoving = 0;
				u.selectedLight.gameObject.SetActive (false);
			if (u.thisUnitStats.tripped > 0) {
				u.thisUnitStats.tripped++;
			}
				if (u.gameObject.transform.eulerAngles.y >= 1 && u.gameObject.transform.eulerAngles.y <= 89){
				u.transform.rotation = Quaternion.Euler (0, 90f, 0);
				}
				else if (u.gameObject.transform.eulerAngles.y >= 91 && u.gameObject.transform.eulerAngles.y <= 179){
				u.transform.rotation = Quaternion.Euler (0, 180f, 0);
				}
				else if (u.gameObject.transform.eulerAngles.y >= 181 && u.gameObject.transform.eulerAngles.y < 269){
				u.transform.rotation = Quaternion.Euler (0, 270f, 0);
				}
				else if (u.gameObject.transform.eulerAngles.y > 271 && u.gameObject.transform.eulerAngles.y < 359){
				u.transform.rotation = Quaternion.Euler(0, 0f, 0);
				}
				//u.isSelected = 0;
			}
        foreach (EnemyUnit e in enemyunits)
        {
            e.targetIndex = 0;
			e.isMoving = 0;
			e.inRange = 0;
			e.canAttack = false;
			//e.targetNode.NodeScore = 0;
			if (e.thisEnemyStats.tripped == 3) {
               
				e.thisEnemyStats.tripped = 0;
				e.thisEnemyStats.transform.rotation = Quaternion.Euler(0, 0, 0);
				e.thisEnemyStats.defense = 4 + e.thisEnemyStats.EnemyArmor.defense;
				e.thisEnemyStats.statusText.text = null; 
			}
            if (e.thisEnemyStats.tripped > 0)
            {
                e.thisEnemyStats.tripped++;
            }
			if (e.Type == "ARCHER") {
				e.enemyArcherAttack.playerInSight = 0;
				e.enemyArcherAttack.standingInSight = 0;
			}
			if (e.Type == "LIGHT INFANTRY") {
				e.enemyLightInfantryAttack.inCharge = 0;
			}
        }
        StandardAttack.hasAttacked = 0;
		SoldierAttack.hasAttacked = 0;
        PlayerDualSwordsAttack.hasAttacked = 0;
		ArcherAttack.hasAttacked = 0;
		ThiefAttack.hasAttacked = 0;
		AmazonAttack.hasAttacked = 0;
		LightInfantryAttack.hasAttacked = 0;
		LeaderAttack.hasAttacked = 0;
		Debug.Log ("ended");

		BattleStateMachine.hasRecalculated = 0;
        turns++;  
		EscapeScript.x = 0;
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYFINDTARGET;
    }
}
