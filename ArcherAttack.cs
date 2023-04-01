using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ArcherAttack : MonoBehaviour {
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
	public int poisonShotFCost;
	public int cripplingShotFCost;

	public float standardAttackAcc;
	public float poisonShotAcc;
	public float cripplingShotAcc;

	public int standardDam;
	public int poisonDam;
	public int cripplingDam;

	public int standardAttackRange;
	public int poisonRange;
	public int cripplingRange;

	public int poisonShotFatigueDam;
	public int cripplingShotDefDam;
	public int cripplingShotConDam;
	public int cripplingShotAccDam;

	public Button basicAttack;
	public Button poisonAttack;
	public Button cripplingAttack;

	Color d = new Color(255, 255, 255);

	public Text standardfCostText;
	public Text poisonFCostText;
	public Text cripplingFCostText;
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

	public bool BoarRaidAttackRangeThing = false;

	public EndTurn endTurn;

	void Awake(){
		standardfCostText.text = fCost.ToString();
		poisonFCostText.text = poisonShotFCost.ToString ();
		cripplingFCostText.text = cripplingShotFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		poisonAttack.gameObject.SetActive (false);
		cripplingAttack.gameObject.SetActive (false);
		abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		poisonFCostText.gameObject.SetActive (false);
		cripplingFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator>();
	}

	// Use this for initialization

	
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
			poisonAttack.gameObject.SetActive(true);
			cripplingAttack.gameObject.SetActive(true);
			abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive(true);
			poisonFCostText.gameObject.SetActive(true);
			cripplingFCostText.gameObject.SetActive(true);
		}
		else {
			basicAttack.gameObject.SetActive(false);
			poisonAttack.gameObject.SetActive(false);
			cripplingAttack.gameObject.SetActive(false);
			abilities.text = "";
			standardfCostText.gameObject.SetActive(false);
			poisonFCostText.gameObject.SetActive(false);
			cripplingFCostText.gameObject.SetActive(false);
		}
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
			BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE)
		{
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardCheck() == false || hasAttacked != 0)
		{
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < poisonShotFCost || hasAttacked != 0
			|| poisonCheck() == false)
		{
			poisonAttack.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardCheck() == true && hasAttacked == 0)
		{
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= poisonShotFCost && hasAttacked == 0 && poisonCheck() == true)
		{
			poisonAttack.interactable = true;
		}
		if (Unit.fatigue < cripplingShotFCost || CripplingCheck() == false || hasAttacked != 0)
		{
			cripplingAttack.interactable = false;
		}
		if (Unit.fatigue >= cripplingShotFCost && CripplingCheck() == true && hasAttacked == 0)
		{
			cripplingAttack.interactable = true;
		}

		//callAttacks
		if (attackSelected == 1
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown(0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			StartCoroutine("standardAttack");
			attackSelected = 0;
			setAttackRange(standardAttackRange);
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
			setAttackRange(standardAttackRange);
			attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
		}
		else if (attackSelected == 2
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown(0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			StartCoroutine ("poisonShot");
			attackSelected = 0;
			setAttackRange(poisonRange);
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
			setAttackRange(poisonRange);
			attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
		}
		else if (attackSelected == 3
			&& CheckSelectionCube() == true
			&& Input.GetMouseButtonDown(0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			StartCoroutine ("cripplingShot");
			attackSelected = 0;
			setAttackRange(cripplingRange);
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

		else if (attackSelected == 3
			&& CheckSelectionCube() == false
			&& Input.GetMouseButtonDown(0))
		{
			attackSelected = 0;
			setAttackRange(cripplingRange);
			attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
		}

		//DisplayChanceToHit
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
				chanceToHitText.text = (UnitStats.chanceToHit + poisonShotAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + poisonDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
			else if (UnitStats.chanceToHit > 100)
			{
				chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + poisonDam - UnitStats.targetEnemyStats.constitution).ToString();
			}

		}
		else if (attackSelected == 3
			&& CheckSelectionCube() == true
			&& EnemyStats.thisEnemyUnit.inRange != 0)
		{
			if (UnitStats.chanceToHit <= 105)
			{
				chanceToHitText.text = (UnitStats.chanceToHit + cripplingShotAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + cripplingDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
			else if (UnitStats.chanceToHit > 105)
			{
				chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + cripplingDam - UnitStats.targetEnemyStats.constitution).ToString();
			}
		}
		else {
			chanceToHitText.text = null;
			damageText.text = null;
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
			return true;
		}
		int temp = Random.Range(1, 100);
		Debug.Log (temp);
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
		battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() 
			+ ". <color=blue>" + UnitStats.unitName + " </color>hits <color=red>" 
			+ EnemyStats.enemyName + " </color>for " + damageDealt.ToString() + " damage";
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

	IEnumerator poisonShot(){
		if (Unit.fatigue >= poisonShotFCost
			&& hasAttacked == 0
			&& (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
				|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
		{
			transform.LookAt(EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Poison Shot";
            Unit.fadeDamageTextFunction();
            Anim.Play("Attack");
			isAttacking = 1;
			yield return new WaitForSeconds(.5f);
			if (AccuracyTest(poisonShotAcc) == true)
			{
				DamageTest(poisonDam);
				EnemyStats.SetHealthBar();
				EnemyStats.thisEnemyUnit.fatigue -= poisonShotFatigueDam;
				EnemyStats.thisEnemyUnit.fatigueDamageText.text = "Stamina -" + poisonShotFatigueDam.ToString ();
				battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + " and reduces stamina by "
				+ poisonShotFatigueDam.ToString () + ".";
			}
			Unit.fatigue -= poisonShotFCost;
			hasAttacked++;
			Invoke("clearText", 1);
			isAttacking = 0;
		}
	}

	IEnumerator cripplingShot(){
		if (Unit.fatigue >= cripplingShotFCost
			&& hasAttacked == 0
			&& (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
				|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
		{
			transform.LookAt(EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Crippling Shot";
            Unit.fadeDamageTextFunction();
            Anim.Play("Attack");
			isAttacking = 1;
			yield return new WaitForSeconds(.5f);
			if (AccuracyTest(cripplingShotAcc) == true)
			{
				DamageTest(cripplingDam);
				EnemyStats.SetHealthBar();
				EnemyStats.defense -= cripplingShotDefDam;
				EnemyStats.EnemyArmor.defense -= cripplingShotDefDam;
				EnemyStats.constitution -= cripplingShotConDam;
				EnemyStats.accuracy -= cripplingShotAccDam;
				EnemyStats.thisEnemyUnit.fatigueDamageText.text = 
					"Def -" + cripplingShotDefDam.ToString () + "Con -" + cripplingShotConDam.ToString() + "Acc -" + cripplingShotAccDam.ToString();
				battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + " and reduces Defense, Accuracy, and Constitution by 1.";
			}
			Unit.fatigue -= cripplingShotFCost;
			hasAttacked++;
			Invoke("clearText", 1);
			isAttacking = 0;
		}
	}



	EnemyStats determineEnemyTarget()
	{
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
	bool StandardCheck(){
		int i = 0;
		Vector3 temp = new Vector3 (Unit.transform.position.x, Unit.transform.position.y + 1f, Unit.transform.position.z);
		Unit.colliderBox.SetActive (false);
		foreach (EnemyUnit e in enemies)
		{
			Vector3 temp2 = new Vector3 (e.transform.position.x, e.transform.position.y + 1f, e.transform.position.z);
			e.colliderBox.SetActive (false);
			if (Mathf.Abs(e.transform.position.x - playerUnit.position.x)
				+ Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= standardAttackRange - 7
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
		Unit.colliderBox.SetActive (true);
		if (i != 0)
		{
			return true;
		}
		else {
			return false;
		}
	}

	bool poisonCheck(){
		int i = 0;
		Vector3 temp = new Vector3 (Unit.transform.position.x, Unit.transform.position.y + 1f, Unit.transform.position.z);
		foreach (EnemyUnit e in enemies)
		{
			Vector3 temp2 = new Vector3 (e.transform.position.x, e.transform.position.y + 1f, e.transform.position.z);
			e.colliderBox.SetActive (false);
			if (Mathf.Abs(e.transform.position.x - playerUnit.position.x)
				+ Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= poisonRange -6
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

	bool CripplingCheck(){
		int i = 0;
		Vector3 temp = new Vector3 (Unit.transform.position.x, Unit.transform.position.y + 1f, Unit.transform.position.z);
		foreach (EnemyUnit e in enemies)
		{
			Vector3 temp2 = new Vector3 (e.transform.position.x, e.transform.position.y + 1f, e.transform.position.z);
			e.colliderBox.SetActive (false);
			if (Mathf.Abs(e.transform.position.x - playerUnit.position.x)
				+ Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= cripplingRange -6
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
				    + Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= standardAttackRange - 7
					&& Physics.Linecast(temp, temp2) == false) {
					i++;
				} else if (attackSelected == 2
				         && Mathf.Abs (e.transform.position.x - playerUnit.position.x)
				         + Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= poisonRange - 6
						&& Physics.Linecast(temp, temp2) == false) {
					i++;
				} else if (attackSelected == 3
				         && Mathf.Abs (e.transform.position.x - playerUnit.position.x)
				         + Mathf.Abs (e.transform.position.z - playerUnit.position.z) <= cripplingRange - 6
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
		if (BoarRaidAttackRangeThing == true) {
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
		}
		attackSelected = 1;
		attackDescription2.text = "Standard low cost attack";
		attackModifiers2.text = "6 Range";
	}

	public void PoisonSelectedFunction()
	{
		if (BoarRaidAttackRangeThing == true) {
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
		}
		attackSelected = 2;
		attackDescription2.text = "Reduces both Stamina and Health";
		attackModifiers2.text = "-5 Acc; 5 Range";
	}

	public void CripplingSelectedFunction()
	{
		if (BoarRaidAttackRangeThing == true) {
			transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
		}
		attackSelected = 3;
		attackDescription2.text = "Low-Power Attack that reduces ability to resist damage and defend attacks";
		attackModifiers2.text = "-1 Dam, -5 Acc; 5 Range";
	}

	public void StandardDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Standard low cost attack";
			attackModifiers.text = "6 Range";
		}
	}

	public void PoisonDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Reduces both Stamina and Health of Target";
			attackModifiers.text = "-5 Acc; 5 Range";
		}
	}

	public void CripplingDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Low-Power Attack that reduces target's ability to resist damage and defend attacks";
			attackModifiers.text = "-1 Dam, -5 Acc; 5 Range";
		}
	}

	public void clearDescription()
	{
		attackDescription.text = null;
		attackModifiers.text = null;
	}
}
