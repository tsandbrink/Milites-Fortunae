using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LightInfantryAttack : MonoBehaviour {

	public EnemyUnit[] enemies;
	public EnemyStats[] enemiesStats;
	public EnemyStats EnemyStats;
	//public Transform enemy;
	public Transform playerUnit;
	public Transform selectionCube;
	public Button basicAttack;
	public Button chargeAttack;
	public Button breakDefenseAttack;

	public GameObject chargeRangeGrid;

	//public Text enemyDamageText;
	public Text standardfCostText;
	public Text chargeFCostText;
	public Text breakDefenseFCostText;
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
	public int chargeFCost;
	public int breakDefenseFCost;

	public int breakDefenseDefenseDam;

	public float standardAttackAcc;
	public float chargeAcc;
	public float breakDefenseAcc;

	public int standardAttackDam;
	public int chargeAttackDam; 
	public int breakDefenseAttackDam;

	public int standardAttackRange;
	public int chargeAttackRange;
	public int breakDefenseAttackRange;

	public BattleStateMachine battleStateMachine;

	Animator Anim;

	// Use this for initialization
	void Awake () {
		standardfCostText.text = fCost.ToString ();
		chargeFCostText.text = chargeFCost.ToString ();
		breakDefenseFCostText.text = breakDefenseFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		chargeAttack.gameObject.SetActive (false);
		breakDefenseAttack.gameObject.SetActive (false);
		Abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		chargeFCostText.gameObject.SetActive (false);
		breakDefenseFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator>();
		chargeRangeGrid.SetActive (false);
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
			chargeAttack.gameObject.SetActive (true);
			breakDefenseAttack.gameObject.SetActive (true);
			Abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive (true);
			chargeFCostText.gameObject.SetActive (true);
			breakDefenseFCostText.gameObject.SetActive (true);
		} else {
			basicAttack.gameObject.SetActive (false);
			chargeAttack.gameObject.SetActive (false);
			breakDefenseAttack.gameObject.SetActive (false);
			Abilities.text = "";
			standardfCostText.gameObject.SetActive (false);
			chargeFCostText.gameObject.SetActive (false);
			breakDefenseFCostText.gameObject.SetActive (false);
		}

		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
			BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE) {
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardCheck() == false || hasAttacked != 0) {
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < chargeFCost || chargeCheck() == false || hasAttacked != 0) {
			chargeAttack.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardCheck() == true && hasAttacked == 0) {
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= chargeFCost && chargeCheck() == true && hasAttacked == 0) {
			chargeAttack.interactable = true;
		}
		if (Unit.fatigue < breakDefenseFCost || StandardCheck() == false || hasAttacked != 0) {
			breakDefenseAttack.interactable = false;
		}
		if (Unit.fatigue >= breakDefenseFCost && StandardCheck() == true && hasAttacked == 0) {
			breakDefenseAttack.interactable = true;
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
			charge ();
			chargeRangeGrid.SetActive (false);
			attackSelected = 0;
			//setAttackRange (chargeAttackRange);
			attackRange.SetActive (false);
			if (EnemyStats.CounterAttackActivated < 3 && EnemyStats.health > 0)
			{
				battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
				battleStateMachine.playerUnit = Unit;
				battleStateMachine.invokeEnemyCounterAttack();
			}
		} else if (attackSelected == 2 
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			Unit.selectedLight.gameObject.SetActive (false);
			//setAttackRange (chargeAttackRange);
			attackRange.SetActive (false);
		} else if (attackSelected == 3
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown (0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) { 
			breakDefense ();
			attackSelected = 0;
			setAttackRange (breakDefenseAttackRange);
			attackRange.SetActive (false);
			if (EnemyStats.CounterAttackActivated < 3 && EnemyStats.health > 0)
			{
				battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
				battleStateMachine.playerUnit = Unit;
				battleStateMachine.invokeEnemyCounterAttack();
			}
		} else if (attackSelected == 3 
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (breakDefenseAttackRange);
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
				chanceToHitText.text = (UnitStats.chanceToHit + chargeAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + chargeAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
			else if (UnitStats.chanceToHit > 110)
			{
				chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + chargeAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
			}

		} else if (attackSelected == 3
			&& CheckSelectionCube() == true

		) {
			if (UnitStats.chanceToHit <= 100)
			{
				chanceToHitText.text = (UnitStats.chanceToHit + breakDefenseAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + breakDefenseAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
			else if (UnitStats.chanceToHit > 100)
			{
				chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + breakDefenseAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
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

    public void charge1()
    {
        if (Unit.fatigue >= chargeFCost
            && hasAttacked == 0
            && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
            || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
        {
            transform.LookAt(EnemyStats.transform.position);
            Anim.Play("chargeAttack");
            float distanceZ = Mathf.Abs(transform.position.z - EnemyStats.transform.position.z);
            float distanceX = Mathf.Abs(transform.position.x - EnemyStats.transform.position.x);
            if (transform.rotation.y == 0 || transform.rotation.y == 180)
            {
                transform.Translate(0, 0, distanceZ - 1, transform);
                float x = transform.position.x * 2;
                float z = transform.position.z * 2;
                float xx = Mathf.RoundToInt(x);
                float zz = Mathf.RoundToInt(z);
                float xxx = xx / 2;
                float zzz = zz / 2;
                Vector3 newPosition = new Vector3(xxx, transform.position.y, zzz);
                transform.position = newPosition;
            }
            else
            {//if (transform.rotation.y == 90 || transform.rotation.y == 270) {
                transform.Translate(0, 0, distanceX - 1, transform);
                float x = transform.position.x * 2;
                float z = transform.position.z * 2;
                float xx = Mathf.RoundToInt(x);
                float zz = Mathf.RoundToInt(z);
                float xxx = xx / 2;
                float zzz = zz / 2;
                Vector3 newPosition = new Vector3(xxx, transform.position.y, zzz);
                transform.position = newPosition;
            }
            if (AccuracyTest(chargeAcc) == true)
            {
                DamageTest(chargeAttackDam);
                EnemyStats.SetHealthBar();
                if (EnemyStats.tripped == 0)
                {
                    if (tripTest() == true)
                    {
                        EnemyStats.tripped = 1;
                        //statusText.text = tripped;
                    }
                }
            }
            Unit.fatigue -= fCost;
            hasAttacked++;
            Invoke("clearText", 1.5f);
        }
    }

	public void charge(){
        EnemyStats.enemyDamageText.text = "Charge Attack";
  //      EnemyStats.fadeDamageTextFunction();
        Invoke("charge1", 1.0f);
	}

    public void breakDefense1()
    {
        if (Unit.fatigue >= fCost
        && hasAttacked == 0
        && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
            || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
        {
            transform.LookAt(EnemyStats.transform.position);
            Anim.Play("Attack");
            //		AccuracyTest (standardAttackAcc);
            if (AccuracyTest(standardAttackAcc) == true)
            {
                DamageTest(standardAttackDam);
                EnemyStats.defense -= breakDefenseDefenseDam;
                EnemyStats.EnemyArmor.defense -= breakDefenseDefenseDam;
                EnemyStats.SetHealthBar();
                EnemyStats.thisEnemyUnit.fatigueDamageText.text = "Def -" + breakDefenseDefenseDam.ToString();
            }
            Unit.fatigue -= breakDefenseDefenseDam;
            hasAttacked++;
            Invoke("clearText", 1.5f);
        }
    }

	public void breakDefense(){
        EnemyStats.enemyDamageText.text = "Charge Attack";
      //  EnemyStats.fadeDamageTextFunction();
        Invoke("breakDefense1", 1.0f);
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
		EnemyStats.enemyDamageText.text = damageDealt.ToString ();
	}

	bool tripTest(){
		int temp = Random.Range(1, 100);
		if (temp <= UnitStats.chanceToHit)
		{
			return true;
		}
		else
		{
			return false;
		}
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

	bool StandardCheck(){
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

	bool chargeCheck(){
		int i = 0;
		Unit.colliderBox.SetActive (false);
		Vector3 temp = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		foreach (EnemyUnit e in enemies) {
			e.colliderBox.SetActive (false);
			float distanceX = Mathf.Abs (e.transform.position.x - Unit.transform.position.x);
			float distanceZ = Mathf.Abs (e.transform.position.z - Unit.transform.position.z);
			Vector3 temp2 = new Vector3 (e.transform.position.x, e.transform.position.y , e.transform.position.z);
			if (Physics.Linecast (temp, temp2) == false
			    && distanceX == 0 && distanceZ > 1 && distanceZ <= chargeAttackRange
			    && e.health > 0) {
				i++;
			} else if (Physics.Linecast (temp, temp2) == false
			         && distanceZ == 0 && distanceX > 1 && distanceX <= chargeAttackRange
			         && e.health > 0) {
				i++;
			}
		}
		Unit.colliderBox.SetActive (true);
		if (i > 0) {
			return true;
		} else {
			return false;
		}
	}

	bool CheckSelectionCube(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (selectionCube.position.x == e.transform.position.x
			    && selectionCube.position.z == e.transform.position.z
			    && attackSelected == 1
			    && Mathf.Abs (e.transform.position.x - playerUnit.position.x)
			    + Mathf.Abs (e.transform.position.z - playerUnit.position.z) == 1) {
				i++;
			} else if (selectionCube.position.x == e.transform.position.x
			         && selectionCube.position.z == e.transform.position.z
			         && attackSelected == 2
			         && Mathf.Abs (e.transform.position.x - playerUnit.position.x)
			         + Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= chargeAttackRange) {
				i++;
			} else if (selectionCube.position.x == e.transform.position.x
			         && selectionCube.position.z == e.transform.position.z
			         && attackSelected == 3
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

	//Select Attack
	public void attackSelectedFunction(){
		attackSelected = 1;
		AttackDescription2.text = "Standard low cost attack";
		AttackModifiers2.text = null;
	}

	public void ChargeAttackSelectedFunction(){
		attackSelected = 2;
		AttackDescription2.text = "Charge in a straight line at far-away enemeies with chance to knock down";
		AttackModifiers2.text = "+2 Dam, -10 Acc";
		chargeRangeGrid.gameObject.SetActive (true);
	}

	public void BreakDefenseAttackSelectedFunction(){
		attackSelected = 3;
		AttackDescription2.text = "Low damage attack that reduces opponent's defenses";
		AttackModifiers2.text = "-1 Dam";
	}

	public void StandardDescription(){
		AttackDescription.text = "Standard low cost attack";
		AttackModifiers.text = null;
	}

	public void ChargeDescription(){
		AttackDescription.text = "Charge in a straight line at far-away enemeies with chance to knock down";
		AttackModifiers.text = "+2 Dam, -10 Acc";
	}

	public void BreakDefenseDescription(){
		AttackDescription.text = "Low damage attack that reduces opponent's defenses";
		AttackModifiers.text = "-1 Dam";
	}

	public void clearDescription(){
		AttackDescription.text = null;
		AttackModifiers.text = null;
	}
}
