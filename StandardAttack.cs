using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StandardAttack : MonoBehaviour {
	public EnemyUnit[] enemies;
	public EnemyStats[] enemiesStats;
	public EnemyStats EnemyStats;
	//public Transform enemy;
	public Transform playerUnit;
	public Transform selectionCube;
	public Button basicAttack;
	public Button powerAttack;
	public Button lungeAttack;

	//public Text enemyDamageText;
	public Text standardfCostText;
	public Text powerFCostText;
	public Text lungeFCostText;
	public Text chanceToHitText;
	public Text damageText;
	public Text Abilities;

	public Text AttackDescription;
	public Text AttackDescription2;
	public Text AttackModifiers;
	public Text AttackModifiers2;

	public GameObject attackRange;
	Color d = new Color (255, 255, 255);
	public int attackSelected = 0;
	public Unit Unit;
	public UnitStats UnitStats;
	public static int hasAttacked = 0;

	public int fCost;
	public int powerFCost;
	public int lungeAttackFCost;
	public float standardAttackAcc;
	public float powerAttackAcc;
	public float lungeAttackAcc;

	public int standardAttackDam;
	public int powerAttackDam; 
	public int lungeAttackDam;

	public int standardAttackRange;
	public int lungeAttackRange;
	public int powerAttackRange;

    public BattleStateMachine battleStateMachine;

	Animator Anim;

	//public Text attackLog;

	public EndTurn endTurn;



	// Use this for initialization
	void Awake () {
		//basicAttack = basicAttack.GetComponent<Button>();
		standardfCostText.text = fCost.ToString ();
		powerFCostText.text = powerFCost.ToString ();
		lungeFCostText.text = lungeAttackFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		powerAttack.gameObject.SetActive (false);
		lungeAttack.gameObject.SetActive (false);
		Abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		powerFCostText.gameObject.SetActive (false);
		lungeFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (hasAttacked == 0) {
			if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
				BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
				EnemyStats = determineEnemyTarget ();
			}
		}
	
		//Set text objects
		if (Unit.isSelected == 1 && hasAttacked == 0) {
			basicAttack.gameObject.SetActive (true);
			powerAttack.gameObject.SetActive (true);
			lungeAttack.gameObject.SetActive (true);
			Abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive (true);
			powerFCostText.gameObject.SetActive (true);
			lungeFCostText.gameObject.SetActive (true);
		} else {
			basicAttack.gameObject.SetActive (false);
			powerAttack.gameObject.SetActive (false);
			lungeAttack.gameObject.SetActive (false);
			Abilities.text = "";
			standardfCostText.gameObject.SetActive (false);
			powerFCostText.gameObject.SetActive (false);
			lungeFCostText.gameObject.SetActive (false);
		}

		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
			BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE) {
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardAndPowerCheck() == false || hasAttacked != 0) {
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < powerFCost || StandardAndPowerCheck() == false || hasAttacked != 0) {
			powerAttack.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardAndPowerCheck() == true && hasAttacked == 0) {
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= powerFCost && StandardAndPowerCheck() == true && hasAttacked == 0) {
			powerAttack.interactable = true;
		}
		if (Unit.fatigue < lungeAttackFCost || LungeCheck() == false || hasAttacked != 0) {
			lungeAttack.interactable = false;
		}
		if (Unit.fatigue >= lungeAttackFCost && LungeCheck() == true && hasAttacked == 0) {
			lungeAttack.interactable = true;
		}

		// Call Attacks
		if (attackSelected == 1 
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown (0)
             && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			standardAttack ();
			attackSelected = 0;
			setAttackRange (standardAttackRange);
			attackRange.SetActive (false);
            if (EnemyStats.CounterAttackActivated < 3 && EnemyStats.health > 0)
            {
                battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;                
                battleStateMachine.playerUnit = Unit;
                battleStateMachine.invokeEnemyCounterAttack();
            }
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		} else if (attackSelected == 1 
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (standardAttackRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		} else if (attackSelected == 2 
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown (0)
             && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			PowerAttack ();
			attackSelected = 0;
			setAttackRange (powerAttackRange);
			attackRange.SetActive (false);
            if (EnemyStats.CounterAttackActivated < 3 && EnemyStats.health > 0)
            {
                battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
                battleStateMachine.playerUnit = Unit;
                battleStateMachine.invokeEnemyCounterAttack();
            }
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
        } else if (attackSelected == 2 
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (powerAttackRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		} else if (attackSelected == 3
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown (0)
             && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) { 
			LungeAttack ();
			attackSelected = 0;
			setAttackRange (lungeAttackRange);
			attackRange.SetActive (false);
            if (EnemyStats.CounterAttackActivated < 3 && EnemyStats.health > 0)
            {
                battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
                battleStateMachine.playerUnit = Unit;
                battleStateMachine.invokeEnemyCounterAttack();
            }
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
        } else if (attackSelected == 3 
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (lungeAttackRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		}

		//Display Chance To Hit
		if (attackSelected == 1
			&& CheckSelectionCube() == true) {
            if (UnitStats.chanceToHit <= 100)
            {
                chanceToHitText.text = UnitStats.chanceToHit.ToString() + "%";
				damageText.text = (UnitStats.attack - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 100)
            {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack - UnitStats.targetEnemyStats.constitution).ToString();
            }
           

		} else if (
			 attackSelected == 2
			&& CheckSelectionCube() == true

		) {
            if (UnitStats.chanceToHit <= 110)
            {
                chanceToHitText.text = (UnitStats.chanceToHit + powerAttackAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + powerAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 110)
            {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + powerAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
            
		} else if (attackSelected == 3
			&& CheckSelectionCube() == true
		
		) {
            if (UnitStats.chanceToHit <= 100)
            {
                chanceToHitText.text = (UnitStats.chanceToHit + lungeAttackAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + lungeAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 100)
            {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + lungeAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
		}
		else {
			chanceToHitText.text = null;
			damageText.text = null;
		}
		                                               
	}

	//Attacks
	public void standardAttack () {
			if (Unit.fatigue >= fCost 
		    	&& hasAttacked == 0
		   		&& (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
			    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {
				transform.LookAt (EnemyStats.transform.position);
                Color c = new Color(0, 255, 0);
                Unit.damageText.color = c;
                Unit.damageText.text = "Basic Attack";
                Unit.fadeDamageTextFunction();
                Anim.Play ("Attack");
		//		AccuracyTest (standardAttackAcc);
				if (AccuracyTest (standardAttackAcc) == true) {
					DamageTest (standardAttackDam);
					EnemyStats.SetHealthBar();
				} 
				Unit.fatigue -= fCost;
				hasAttacked ++;
			Invoke("clearText", 1.5f);
		}
	}

	public void PowerAttack(){
			if (Unit.fatigue >= powerFCost 
		    	&& hasAttacked == 0
			    && BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
			    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE) {
				transform.LookAt (EnemyStats.transform.position);
                Color c = new Color(0, 255, 0);
                Unit.damageText.color = c;
                Unit.damageText.text = "Power Attack";
                Unit.fadeDamageTextFunction();
                Anim.Play ("PowerAttack");
		//		AccuracyTest (powerAttackAcc);
				if (AccuracyTest (powerAttackAcc) == true) {
					DamageTest (powerAttackDam);
					EnemyStats.SetHealthBar();
				} 
				Unit.fatigue -= powerFCost;
				hasAttacked ++;
			Invoke("clearText", 1.5f);
			}
	}

	public void LungeAttack(){
		if (Unit.fatigue >= lungeAttackFCost
		    && hasAttacked == 0
		    &&BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE){
			transform.LookAt (EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Lunge Attack";
            Unit.fadeDamageTextFunction();
            Anim.Play ("LungeAttack");
		//	AccuracyTest (lungeAttackAcc);
			if (AccuracyTest (lungeAttackAcc) == true){
				DamageTest (lungeAttackDam);
				EnemyStats.SetHealthBar();
			}
			Unit.fatigue -= lungeAttackFCost;
			hasAttacked ++;
			Invoke ("clearText", 1.5f);
		}
	}

	//Tests
	bool AccuracyTest(float AttackType){
		if (UnitStats.chanceToHit > 100) {
			UnitStats.chanceToHit =100;
		}
		if (UnitStats.chanceToHit < 0) {
			UnitStats.chanceToHit = 0;
		}
		if (EnemyStats.tripped != 0) {
            Invoke("GetHit", .25f);
            return true;
		}
		int temp = Random.Range (1, 100);
		Debug.Log (temp);
		Debug.Log (UnitStats.chanceToHit);
		if (temp <= UnitStats.chanceToHit + AttackType) {
			Invoke ("GetHit", .25f);
			return true;
		} 
		else {
			EnemyStats.transform.LookAt (transform);
			Invoke ("Block", .25f);
			Color c = new Color(0,0,255);
			EnemyStats.enemyDamageText.color = c;
			EnemyStats.enemyDamageText.text = "Miss!";
			battleStateMachine.turnCounter += 1;
			battleStateMachine.attackLogScrollyBar.value = 0;
			battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString()
				+ ". <color=red>" + EnemyStats.enemyName + "</color> blocks <color=blue>" 
				+ UnitStats.unitName + "</color>'s attack.";
			battleStateMachine.attackLogScrollyBar.value = 0;
			return false;
		}
	}

	void GetHit(){
		EnemyStats.Anim.Play ("GetHit");
		battleStateMachine.FxAudioSource.clip = battleStateMachine.Hit;
		battleStateMachine.FxAudioSource.Play ();
	}

	void Block(){
		EnemyStats.Anim.Play ("Block");
		battleStateMachine.FxAudioSource.clip = battleStateMachine.Miss;
		battleStateMachine.FxAudioSource.Play ();
	}

	void DamageTest (int AttackType){
		int damageDealt = UnitStats.attack - EnemyStats.constitution + AttackType;
		EnemyStats.health -= damageDealt;
		//EnemyStats.enemyDamageText.text = damageDealt.ToString ();
        StartCoroutine(appearText(damageDealt));
        battleStateMachine.turnCounter += 1;
		battleStateMachine.attackLogScrollyBar.value = 0;
		battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() + ". <color=blue>" + UnitStats.unitName + " </color>hits <color=red>" 
			+ EnemyStats.enemyName + " </color>for " + damageDealt.ToString() + " damage.";
		battleStateMachine.attackLogScrollyBar.value = 0;
	}

    IEnumerator appearText(int i)
    {
        yield return new WaitForSeconds(.6f);
        Color c = new Color(255, 0, 0);
        EnemyStats.enemyDamageText.color = c;
        EnemyStats.enemyDamageText.text = i.ToString();
    }

	//Clean Up Functions
	void clearText(){
		foreach (EnemyStats EnemyStats in enemiesStats) {
			EnemyStats.enemyDamageText.color = d;
			EnemyStats.enemyDamageText.text = "";
		}
		AttackDescription2.text = "";
		AttackModifiers2.text = "";
	}

	void setAttackRange(int attackRange1){
			attackRange.transform.Translate (attackRange1 / 2, 0, attackRange1 / 2);
	}

	//Select Attack
	public void attackSelectedFunction(){
		attackSelected = 1;
		AttackDescription2.text = "Standard low cost attack";
		AttackModifiers2.text = null;
	}

	public void PowerAttackSelectedFunction(){
		attackSelected = 2;
		AttackDescription2.text = "Swing harder sacrificing accuracy for greater damage";
		AttackModifiers2.text = "+2 Dam, -10 Acc";
	}

	public void LungeAttackSelectedFunction(){
		attackSelected = 3;
		AttackDescription2.text = "Wide Sweeping Attack, can hit farther away enemies";
		AttackModifiers2.text = "+1 Dam";
	}

	//Checks
	bool CheckSelectionCube(){
		int i = 0;
		foreach (EnemyUnit e in enemies){
			if (selectionCube.position.x == e.transform.position.x
			    && selectionCube.position.z == e.transform.position.z
			    && attackSelected == 1
			    && Mathf.Abs (e.transform.position.x - playerUnit.position.x)
			    + Mathf.Abs (e.transform.position.z - playerUnit.position.z) == 1) {
				i++;
			}
			else if (attackSelected == 3 && Mathf.Abs(e.transform.position.x - playerUnit.position.x) 
				         + Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= 2
				         && Mathf.Abs(e.transform.position.x - playerUnit.position.x) < 2
				         && Mathf.Abs(e.transform.position.z - playerUnit.position.z) < 2
							&& e.transform.position.y >= 0
				&& selectionCube.position.x == e.transform.position.x
				&& selectionCube.position.z == e.transform.position.z){
					i++;
				}
			else if (selectionCube.position.x == e.transform.position.x
				&& selectionCube.position.z == e.transform.position.z
				&& attackSelected == 2
				&& Mathf.Abs (e.transform.position.x - playerUnit.position.x)
				+ Mathf.Abs (e.transform.position.z - playerUnit.position.z) == 1) {
				i++;
			}
		}
		//}
		//Debug.Log (i);
		if (i != 0) {
			return true;
		} else {
			return false;
		}
	}

	bool StandardAndPowerCheck(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (Mathf.Abs(e.transform.position.x - playerUnit.position.x) 
			    + Mathf.Abs(e.transform.position.z - playerUnit.position.z) == 1
			    && e.transform.position.y >= 0){
			
				i++;
			}
		}
		if (i != 0){
			return true;
		} else{
			return false;
		}
	}

	bool LungeCheck(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (Mathf.Abs(e.transform.position.x - playerUnit.position.x) 
			    + Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= 2
			    && Mathf.Abs(e.transform.position.x - playerUnit.position.x) < 2
			    && Mathf.Abs(e.transform.position.z - playerUnit.position.z) < 2
			    && e.transform.position.y >= 0){
			
				i++;
			}
		}
		if (i != 0) {
			return true;
		} else {
			return false;
		}
	}

	EnemyStats determineEnemyTarget(){
		EnemyStats target = null;
		foreach (EnemyStats e in enemiesStats) {
			if (selectionCube.position.x == e.transform.position.x
			    && selectionCube.position.z == e.transform.position.z){
				target = e;
				break;
			}
			else {target = null;
			}
		}
		return target;
	}

	public void StandardDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Standard low cost attack";
			AttackModifiers.text = null;
		}
	}

	public void PowerDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Swing harder sacrificing accuracy for greater damage";
			AttackModifiers.text = "+2 Dam, -10 Acc";
		}
	}

	public void LungeDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Wide Sweeping Attack, can hit farther away enemies";
			AttackModifiers.text = "+1 Dam";
		}
	}

	public void clearDescription(){
		AttackDescription.text = null;
		AttackModifiers.text = null;
	}
}
