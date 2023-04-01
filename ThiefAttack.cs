 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThiefAttack : MonoBehaviour {
	public EnemyUnit[] enemies;
	public EnemyStats[] enemiesStats;
	public EnemyStats EnemyStats;
	//public Transform enemy;
	public Transform playerUnit;
	public Transform selectionCube;
	public Button basicAttack;
	public Button mineAttack;
	public Button dash;

	public Text standardfCostText;
	public Text mineFCostText;
	public Text dashFCostText;
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
	public static bool mineStop;

	public Unit[] playerUnits;
	public GameObject[] unwalkables;

	public int fCost;
	public int mineFCost;
	public int dashFCost;

	public float standardAttackAcc;

	public int standardAttackDam;
	public static int mineDam = 6; 


	public int standardAttackRange;
	public int mineRange;
	public int dashRange;

	public BattleStateMachine battleStateMachine;

	public Pathfinding pathfinding;

	Animator Anim;

	public Collider mine;

	public ParticleSystem smoke;
	public EndTurn endTurn;

	void Awake () {
		standardfCostText.text = fCost.ToString ();
		mineFCostText.text = mineFCost.ToString ();
		dashFCostText.text = dashFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		mineAttack.gameObject.SetActive (false);
		dash.gameObject.SetActive (false);
		Abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		mineFCostText.gameObject.SetActive (false);
		dashFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator>();
		smoke.gameObject.SetActive (false);
		mineStop = false;
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
			mineAttack.gameObject.SetActive (true);
			dash.gameObject.SetActive (true);
			Abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive (true);
			mineFCostText.gameObject.SetActive (true);
			dashFCostText.gameObject.SetActive (true);
		} else {
			basicAttack.gameObject.SetActive (false);
			mineAttack.gameObject.SetActive (false);
			dash.gameObject.SetActive (false);
			Abilities.text = "";
			standardfCostText.gameObject.SetActive (false);
			mineFCostText.gameObject.SetActive (false);
			dashFCostText.gameObject.SetActive (false);
		}

		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
			BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE) {
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardCheck() == false || hasAttacked != 0) {
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < mineFCost || hasAttacked != 0 || BattleStateMachine.currentState == BattleStateMachine.BattleStates.START) {
			mineAttack.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardCheck() == true && hasAttacked == 0) {
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= mineFCost && hasAttacked == 0) {
			mineAttack.interactable = true;
		}
		if (Unit.fatigue < dashFCost || hasAttacked != 0 || BattleStateMachine.currentState == BattleStateMachine.BattleStates.START) {
			dash.interactable = false;
		}
		if (Unit.fatigue >= dashFCost && hasAttacked == 0) {
			dash.interactable = true;
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
			&& mineAndDashCheckSelectionCube() == true
			&& Input.GetMouseButtonDown (0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			mineAttackFunction ();
			attackSelected = 0;
			setAttackRange (mineRange);
			attackRange.SetActive (false);
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		} else if (attackSelected == 2 
			&& mineAndDashCheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (mineRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
			mineStop = false;
		} else if (attackSelected == 3
			&& mineAndDashCheckSelectionCube() == true
			&& Input.GetMouseButtonDown (0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) { 
			Vector3 v = new Vector3(selectionCube.transform.position.x, transform.position.y, selectionCube.transform.position.z);
			Anim.Play ("Dash");
			smoke.gameObject.SetActive (true);
			smoke.Play ();
			smoke.transform.parent = null;
			dashFunction (v);
			attackSelected = 0;
			setAttackRange (dashRange);
			attackRange.SetActive (false);
			Invoke ("reAttachParenting", 5.0f);
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		} else if (attackSelected == 3 
			&& mineAndDashCheckSelectionCube() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (dashRange);
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


		} 
		else {
			chanceToHitText.text = null;
			damageText.text = null;
		}
	}
		

	void reAttachParenting(){
		smoke.transform.parent = transform;
		Vector3 v = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		smoke.transform.position = v;
		smoke.gameObject.SetActive (false);
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
			Invoke("clearText", 2f);
		}
	}

	public void mineAttackFunction(){
		if (Unit.fatigue >= mineFCost
		    && hasAttacked == 0
		    && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {
			
			Vector3 v = new Vector3 (selectionCube.position.x, mine.gameObject.transform.position.y, selectionCube.position.z);
			//mine.gameObject.transform.position = Vector3.MoveTowards (mine.gameObject.transform.position, v, 1000 * Time.deltaTime);
			mine.gameObject.transform.position = v;
			transform.LookAt (v);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Set Mine";
            Unit.fadeDamageTextFunction();
            Anim.Play ("mineAttack");
			Unit.fatigue -= mineFCost;
			hasAttacked ++;
			Invoke("clearText", 1.5f);
			battleStateMachine.turnCounter += 1;
			battleStateMachine.attackLogScrollyBar.value = 0;
			battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() 
				+ ". <color=blue>" + UnitStats.unitName + " </color> sets Mine.";
			battleStateMachine.attackLogScrollyBar.value = 0;
		}
	}

	public void dashFunction(Vector3 v){
		if (Unit.fatigue >= dashFCost
		    && hasAttacked == 0
		    && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {
			//smoke.gameObject.SetActive (true);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Dash";
            Unit.fadeDamageTextFunction();
			transform.LookAt (v);
			transform.position = v;
			//transform.position.Set (selectionCube.transform.position.x, transform.position.y, selectionCube.transform.position.z);
			Unit.fatigue -= dashFCost;
			hasAttacked ++;
			Invoke("clearText", 2f);
			battleStateMachine.turnCounter += 1;
			battleStateMachine.attackLogScrollyBar.value = 0;
			battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() 
				+ ". <color=blue>" + UnitStats.unitName + " </color> uses dash.";
			battleStateMachine.attackLogScrollyBar.value = 0;
		}
	}
		
	bool CheckSelectionCube(){
		int i = 0;
		foreach (EnemyUnit e in enemies){
			if (selectionCube.position.x == e.transform.position.x
				&& selectionCube.position.z == e.transform.position.z
				&& Mathf.Abs(e.transform.position.x - playerUnit.position.x) 
				+ Mathf.Abs(e.transform.position.z - playerUnit.position.z) == 1){
				i++;
			}
		}
		if (i != 0) {
			return true;
		} else {
			return false;
		}
	}

	bool mineAndDashCheckSelectionCube(){
		if ((Mathf.Abs (selectionCube.transform.position.x - transform.position.x)
		    + Mathf.Abs (selectionCube.transform.position.z - transform.position.z)) <= mineRange/2
		    && checkForUnwalkables () == true) {
			return true;
		} else {
			return false;
		}
	}

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
        StartCoroutine (appearText(damageDealt));
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

	void clearText(){
		foreach (EnemyStats EnemyStats in enemiesStats) {
			EnemyStats.enemyDamageText.color = d;
			EnemyStats.enemyDamageText.text = null;
		}
		AttackDescription2.text = null;
		AttackModifiers2.text = null;
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

	bool checkForUnwalkables(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (selectionCube.transform.position.x == e.transform.position.x
			    && selectionCube.transform.position.z == e.transform.position.z
				&& e.health > 0) {
				i++;
			}
		}
		foreach (Unit u in playerUnits) {
			if (selectionCube.transform.position.x == u.transform.position.x
			    && selectionCube.transform.position.z == u.transform.position.z
				&& u.health > 0
				&& u.gameObject.activeInHierarchy == true) {
				i++;
			}
		}
		foreach (GameObject g in unwalkables) {
			if (selectionCube.transform.position.x == g.transform.position.x
			    && selectionCube.transform.position.z == g.transform.position.z) {
				i++;
			}
		}
		if (i == 0) {
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

	public void mineAttackSelectedFunction(){
		attackSelected = 2;
		mineStop = true;
		AttackDescription2.text = "Place an explosive on the battlefield that deals heavy damage to anyone who passes over it";
		AttackModifiers2.text = null;
	}

	public void dashSelectedFunction(){
		attackSelected = 3;
		AttackDescription2.text = "Move to any square within range without using stamina";
		AttackModifiers2.text = null;
	}

	public void StandardDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Standard low cost attack";
			AttackModifiers.text = null;
		}
	}

	public void PowerDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Place an explosive on the battlefield that deals heavy damage to anyone who passes over it";
			AttackModifiers.text = null;
		}
	}

	public void LungeDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Move to any square within range without using stamina ";
			AttackModifiers.text = null;
		}
	}

	public void clearDescription(){
		AttackDescription.text = null;
		AttackModifiers.text = null;
	}
}
