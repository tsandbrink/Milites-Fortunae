using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AmazonAttack : MonoBehaviour {

	public EnemyUnit[] enemies;
	public EnemyStats[] enemiesStats;
	public EnemyStats EnemyStats;
	//public Transform enemy;
	public Transform playerUnit;
	public Transform selectionCube;
	public int attackSelected = 0;
	public Unit Unit;
	public UnitStats UnitStats;
	public static int hasAttacked = 0;

	public BattleStateMachine battleStateMachine;

	public int fCost;
	public int volleyFCost;
	public int powerShotFCost;

	public float standardAttackAcc;
	public float volleyAcc;
	public float powerAcc;

	public int standardDam;
	public int volleyDam;
	public int powerDam;

	public int standardRange;
	public int volleyRange;
	public int powerRange;

    public Button basicAttack;
	public Button volleyAttack;
	public Button powerAttack;

	Color d = new Color(255, 255, 255);

	public Text standardfCostText;
	public Text volleyFCostText;
	public Text powerFCostText;
	public Text chanceToHitText;
	public Text abilities;

	public Text attackDescription;
	public Text attackDescription2;
	public Text attackModifiers;
	public Text attackModifiers2;

	public GameObject attackRange;
	public Text damageText;

	Animator Anim;
	int isAttacking = 0;
	public EndTurn endTurn;

	void Awake () {
		standardfCostText.text = fCost.ToString();
		volleyFCostText.text = volleyFCost.ToString ();
		powerFCostText.text = powerShotFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		volleyAttack.gameObject.SetActive (false);
		powerAttack.gameObject.SetActive (false);
		abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		volleyFCostText.gameObject.SetActive (false);
		powerFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking == 0 && hasAttacked == 0) {
			if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
				BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
				EnemyStats = determineEnemyTarget ();
			}
		}

		if (Unit.isSelected == 1 && hasAttacked == 0)
		{
			basicAttack.gameObject.SetActive(true);
			volleyAttack.gameObject.SetActive(true);
			powerAttack.gameObject.SetActive(true);
			abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive(true);
			volleyFCostText.gameObject.SetActive(true);
			powerFCostText.gameObject.SetActive(true);
		}
		else {
			basicAttack.gameObject.SetActive(false);
			volleyAttack.gameObject.SetActive(false);
			powerAttack.gameObject.SetActive(false);
			abilities.text = "";
			standardfCostText.gameObject.SetActive(false);
			volleyFCostText.gameObject.SetActive(false);
			powerFCostText.gameObject.SetActive(false);
		}
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
			BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE)
		{
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardAndPowerCheck(standardRange, 7) == false || hasAttacked != 0)
		{
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < volleyFCost || hasAttacked != 0
			|| volleyCheck() == false)
		{
			volleyAttack.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardAndPowerCheck(standardRange, 7) == true && hasAttacked == 0)
		{
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= volleyFCost && hasAttacked == 0 && volleyCheck() == true)
		{
			volleyAttack.interactable = true;
		}
		if (Unit.fatigue < powerShotFCost || StandardAndPowerCheck(powerRange, 7) == false || hasAttacked != 0)
		{
			powerAttack.interactable = false;
		}
		if (Unit.fatigue >= powerShotFCost && StandardAndPowerCheck(powerRange, 7) == true && hasAttacked == 0)
		{
			powerAttack.interactable = true;
		}

		//callAttacks
		if (attackSelected == 1 //standardAttack
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown(0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			StartCoroutine("standardAttack");
			attackSelected = 0;
			setAttackRange(standardRange);
			attackRange.SetActive(false);
			if (EnemyStats.CounterAttackActivated < 3 
				&& EnemyStats.health > 0)
			{
				battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
				battleStateMachine.playerUnit = Unit;
				battleStateMachine.invokeEnemyCounterAttack();
			}
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		}
		else if (attackSelected == 1
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown(0))
		{
			attackSelected = 0;
			setAttackRange(standardRange);
			attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
		}
		else if (attackSelected == 2
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown(0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			StartCoroutine ("volleyShot");
			attackSelected = 0;
			setAttackRange(volleyRange);
			attackRange.SetActive(false);
			if (EnemyStats.CounterAttackActivated < 3
				&& EnemyStats.health > 0)
			{
				battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
				battleStateMachine.playerUnit = Unit;
				battleStateMachine.invokeEnemyCounterAttack();
			}
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		}
		else if (attackSelected == 2
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown(0))
		{
			attackSelected = 0;
			setAttackRange(volleyRange);
			attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
		}
		else if (attackSelected == 3
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown(0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			StartCoroutine ("powerShot");
			attackSelected = 0;
			setAttackRange(powerRange);
			attackRange.SetActive(false);
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		}
		else if (attackSelected == 3
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown(0))
		{
			attackSelected = 0;
			setAttackRange(powerRange);
			attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
		}

		//DisplayChanceToHit and Damage
		if (attackSelected == 1
			&& CheckSelectionCube() == true
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
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


		}
		else if (attackSelected == 2
			&& CheckSelectionCube() == true
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			if (UnitStats.chanceToHit <= 100)
			{
				chanceToHitText.text = (UnitStats.chanceToHit + volleyAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + volleyDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
			else if (UnitStats.chanceToHit > 100)
			{
				chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + volleyDam - UnitStats.targetEnemyStats.constitution).ToString();
			}

		}
		else if (attackSelected == 3
			&& CheckSelectionCube() == true
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			if (UnitStats.chanceToHit <= 105)
			{
				chanceToHitText.text = (UnitStats.chanceToHit + powerAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + powerDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
			else if (UnitStats.chanceToHit > 105)
			{
				chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + powerDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
		}
		else {
			chanceToHitText.text = null;
			damageText.text = null;
		}

	}

	EnemyStats determineEnemyTarget(){
		EnemyStats target = null;
		foreach (EnemyStats e in enemiesStats)
		{
			if (selectionCube.position.x == e.gameObject.transform.position.x
				&& selectionCube.position.z == e.gameObject.transform.position.z)
			{
				target = e;
				break;
			}
			else {
				target = null;
			}
		}
		return target;
	}

	//Checks
	bool StandardAndPowerCheck(int thisAttackRange, int modifier){
		int i = 0;
		Vector3 temp = new Vector3 (Unit.transform.position.x, Unit.transform.position.y + 1f, Unit.transform.position.z);
		foreach (EnemyUnit e in enemies)
		{
			Vector3 temp2 = new Vector3 (e.transform.position.x, e.transform.position.y + 1f, e.transform.position.z);
			e.colliderBox.SetActive (false);
			if (Mathf.Abs(e.transform.position.x - playerUnit.position.x)
				+ Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= thisAttackRange - modifier
				&& e.transform.position.y >= 0
				&& Physics.Linecast(temp, temp2) == false)
			{
				i++;
				if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
					BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
					e.inRange++;
				}
			}
			e.colliderBox.SetActive (true);

		}
		if (i != 0)
		{
			return true;
		}
		else {
			return false;
		}
	}

	bool volleyCheck(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (Mathf.Abs (e.transform.position.x - playerUnit.position.x)
				+ Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= volleyRange - 8
			    && e.transform.position.y >= 0) {
				i++;
				if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
					BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
					e.inRange++;
				}
			}
		}
		if (i != 0)
		{
			return true;
		}
		else {
			return false;
		}
	}

	public bool AccuracyTest(float AttackType)
	{
		if (UnitStats.chanceToHit > 100)
		{
			UnitStats.chanceToHit = 100;
		}
		if (UnitStats.chanceToHit < 0)
		{
			UnitStats.chanceToHit = 0;
		}
		if (EnemyStats.tripped != 0) {
            Invoke("GetHit", .25f);
            return true;
		}
		int temp = Random.Range(1, 100);
		if (temp <= UnitStats.chanceToHit + AttackType)
		{
			Invoke ("GetHit", .25f);
			return true;
		}
		else {
			EnemyStats.transform.LookAt (transform);
			Invoke ("Block", .25f);
			Color c = new Color(0, 0, 255);
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

	public void DamageTest(int AttackType)
	{
		int damageDealt = UnitStats.attack - EnemyStats.constitution + AttackType;
		EnemyStats.health -= damageDealt;
		//EnemyStats.enemyDamageText.text = damageDealt.ToString();
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

	IEnumerator standardAttack(){
		if (Unit.fatigue >= fCost
			&& hasAttacked == 0
			&& (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
				|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
		{
			transform.LookAt(EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Basic Attack";
            Unit.fadeDamageTextFunction();
            Anim.Play("Attack");
			isAttacking = 1;
			yield return new WaitForSeconds(.5f);
			if (AccuracyTest(standardAttackAcc) == true)
			{
				DamageTest(standardDam);
				EnemyStats.SetHealthBar();
			}
			Unit.fatigue -= fCost;
			hasAttacked++;
			Invoke("clearText", 1);
			isAttacking = 0;
		}
	}

	IEnumerator volleyShot(){
		if (Unit.fatigue >= volleyFCost
		    && hasAttacked == 0
		    && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {
			transform.LookAt(EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Volley";
            Unit.fadeDamageTextFunction();
            Anim.Play("Attack");
			isAttacking = 1;
			yield return new WaitForSeconds(.5f);
			if (AccuracyTest(volleyAcc) == true)
			{
				DamageTest(volleyDam);
				EnemyStats.SetHealthBar();
			}
			Unit.fatigue -= volleyFCost;
			hasAttacked++;
			Invoke ("clearText", 1);
			isAttacking = 0;
		}
	}

	IEnumerator powerShot(){
		if (Unit.fatigue >= powerShotFCost
			&& hasAttacked == 0
			&& (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
				|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {
			transform.LookAt(EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Power Shot";
            Unit.fadeDamageTextFunction();
            Anim.Play("Attack");
			isAttacking = 1;
			yield return new WaitForSeconds(.5f);
			if (AccuracyTest (powerAcc) == true) {
				DamageTest (powerDam);
				EnemyStats.SetHealthBar ();
				}
			}
		Unit.fatigue -= powerShotFCost;
		hasAttacked++;
		Invoke ("clearText", 1);
		isAttacking = 0;
	}

	bool CheckSelectionCube()
	{
		int i = 0;
		Vector3 temp = new Vector3(Unit.transform.position.x, Unit.transform.position.y + 1f, Unit.transform.position.z);
		Vector3 temp2 = new Vector3 (selectionCube.position.x, selectionCube.position.y + 1f, selectionCube.position.z);
		Unit.colliderBox.SetActive (false);
		foreach (EnemyUnit e in enemies)
		{
			e.colliderBox.SetActive (false);
			if (selectionCube.position.x == e.transform.position.x
				&& selectionCube.position.z == e.transform.position.z)
			{
				if (attackSelected == 1
					&& Mathf.Abs (e.transform.position.x - playerUnit.position.x)
					+ Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= standardRange - 7
					&& Physics.Linecast(temp, temp2) == false) {
					i++;
				} else if (attackSelected == 2
					&& Mathf.Abs (e.transform.position.x - playerUnit.position.x)
					+ Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= volleyRange - 8) {
					i++;
				} else if (attackSelected == 3
					&& Mathf.Abs (e.transform.position.x - playerUnit.position.x)
					+ Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= powerRange - 7
					&& Physics.Linecast(temp, temp2) == false) {
					i++;
				}
			}
			e.colliderBox.SetActive (true);
		}
		Unit.colliderBox.SetActive (true);
		if (i != 0)
		{
			return true;
		}
		else {
			return false;
		}
	}

	void setAttackRange(int attackRange1)
	{
		attackRange.transform.Translate(attackRange1 / 2, 0, attackRange1 / 2);
	}

	void clearText()
	{
		foreach (EnemyStats EnemyStats in enemiesStats)
		{
			EnemyStats.enemyDamageText.color = d;
			EnemyStats.enemyDamageText.text = null;
			EnemyStats.thisEnemyUnit.fatigueDamageText.text = null;

		}
		attackDescription2.text = null;
		attackModifiers2.text = null;
	}

	//Select Attack Button Functions
	public void attackSelectedFunction()
	{
		attackSelected = 1;
		attackDescription2.text = "Standard low cost attack";
		attackModifiers2.text = "6 Range";
	}

	public void volleySelectedFunction()
	{
		attackSelected = 2;
		attackDescription2.text = "Shoot arrow in arc over obstacles";
		attackModifiers2.text = "-10 Acc; 6 Range";
	}

	public void powerSelectedFunction()
	{
		attackSelected = 3;
		attackDescription2.text = "Close high power attack";
		attackModifiers2.text = "+1 Dam, -5 Acc; 6 Range";
	}

	public void StandardDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Standard low cost attack";
			attackModifiers.text = "6 Range";
		}
	}

	public void VolleyDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Shoot arrow in arc over obstacles";
			attackModifiers.text = "7 Range";
		}
	}

	public void powerDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Close high power attack ";
			attackModifiers.text = "+1 Dam, -10 Acc; 6 Range";
		}
	}

	public void clearDescription()
	{
		attackDescription.text = null;
		attackModifiers.text = null;
	}
}
