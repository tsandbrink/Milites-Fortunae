using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DualSwordAttack : MonoBehaviour {
    //Contains Attack and Decision Tree Functions for Enemy Dual-Sword Type Characters

	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;
	public Unit playerUnit;
	UnitStats TargetUnitStats;

	int EnemyStandardFCost;
	public int whirlWindFCost;
	public int WhirlwindDamage;

	public int tripFCost;
	public int tripAcc;
	public int tripDam;

	Animator Anim;

	// Use this for initialization
	void Start(){
		EnemyStandardFCost = BattleStateMachine.EnemyStandardFCost;
		Anim = GetComponent<Animator> ();
	}
	
    //Decision Tree Algorithm for AI Dual-Swords to pick Ability. Returns 1 for Basic Attack; 2 for Whirlwind; 3 for Trip; 0 for No Ability.
	public int dualChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution
		    && (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1
			&& WhirlwindAttackCheck() == false) {
			return 1;
		} else if (WhirlwindAttackCheck () == true && thisEnemyUnit.fatigue >= whirlWindFCost) {
			return 2;
		} else if (tripAttackCheck() == true && thisEnemyUnit.fatigue >= tripFCost){
			return 3;
		} else if (thisEnemyUnit.fatigue < EnemyStandardFCost || (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) > 1) {
			return 0;
		} else {
			return 1;
		}
	}

    //Function AI uses in its Decision Tree to see if it should use Whirlwind Ability
	bool WhirlwindAttackCheck(){
		int i = 0;
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) 
				+ Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) <= 2
				&& Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) < 2
				&& Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) < 2
				&& u.transform.position.y >= -1
				&& u.health > 0) {
				i++;
			}

		}
		Debug.Log (i);
		if (i >= 2) {
			return true;
		} else if (i == 1 && Mathf.Abs (playerUnit.transform.position.x - thisEnemyUnit.transform.position.x)
			+ Mathf.Abs (playerUnit.transform.position.z - thisEnemyUnit.transform.position.z) == 2
			&& Mathf.Abs (playerUnit.transform.position.x - thisEnemyUnit.transform.position.x) < 2
			&& Mathf.Abs (playerUnit.transform.position.z - thisEnemyUnit.transform.position.z) < 2
			&& playerUnit.transform.position.y >= -1) {
			return true;
		}
		else {
			return false;
		}
	}

    //Call Ability Functions with Delay && Display Chosen Ability on Screen
	public void WhirlwindAttack(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Whirlwind";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("WhirlwindAttack1", 1f);
		return;
	}

	public void tripAttack(){
		Invoke ("tripAttack1", 1f);
		return;
	}

    //Function AI uses to determine if it should use trip ability
	bool tripAttackCheck(){
		int temp = Random.Range (1, 100);
		if (temp > 50) {
			return true;
		} else {
			return false;
		}
	}

    //AI Executes Whirlwind Attack and Moves BattleStateMachine to EnemyRecaculate Battle Phase
	void WhirlwindAttack1(){
		Anim.Play ("Whirlwind");
		List<Unit> UnitsInRange = new List<Unit> ();
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) 
			    + Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) <= 2
			    && Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) < 2
			    && Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) < 2
			    && u.transform.position.y >= 0
				&& u.health >= 0) {
				UnitsInRange.Add(u);
			}
		}
		foreach (Unit u in UnitsInRange) {
            Debug.Log(u.thisUnitStats.unitName);
            if (BattleStateMachine.AccuracyTest (BattleStateMachine.standardAttackAcc) == true) {
				int damageDealt = thisEnemyUnit.thisEnemyStats.attack - u.thisUnitStats.constitution + WhirlwindDamage;
				u.thisUnitStats.health -= damageDealt;
				u.thisUnitStats.SetHealthBar();
				u.damageText.text = damageDealt.ToString();
			} 
            else
            {
                Color c = new Color(0, 0, 255);
                u.damageText.color = c;
              //  u.damageText.text = "Miss!"; (BattleStateMachine will display "Miss")
            }
		}
		thisEnemyUnit.fatigue -= whirlWindFCost;
		foreach (Unit u in UnitsInRange) {
			StartCoroutine(ClearText(u, 1));
		}
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

    //AI Executes Trip Ability and Moves BattleStateMachine to EnemyRecaculate Battle Phase
	void tripAttack1(){
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		if (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ) == 1 
			&& thisEnemyUnit.fatigue >= EnemyStandardFCost) {
			//AccuracyTest (standardAttackAcc);
			thisEnemyUnit.thisEnemyStats.Anim.Play("Trip");
            thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Trip";
            thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
			if (BattleStateMachine.AccuracyTest (tripAcc) == true) {
				BattleStateMachine.DamageTest (tripDam, thisEnemyUnit.thisEnemyStats);
				playerUnit.thisUnitStats.SetHealthBar ();
				if (playerUnit.thisUnitStats.tripped == 0) {
					if (tripTest () == true) {
						playerUnit.thisUnitStats.tripped = 1;
						//statusText.text = tripped;
					}
				}
			} 
			thisEnemyUnit.fatigue -= tripFCost;

		}
		StartCoroutine (ClearText (playerUnit, 1));
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

	bool tripTest(){
		int temp = Random.Range(1, 100);
		if (temp <= EnemyStats.chanceToHit)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	IEnumerator ClearText (Unit unit, float delay){
		yield return new WaitForSeconds(2.5f);
		unit.damageText.text = null;
	}


}
