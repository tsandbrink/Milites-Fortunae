using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeaderAttack : MonoBehaviour {
	public EnemyUnit[] enemies;
	public EnemyStats[] enemiesStats;
	public EnemyStats EnemyStats;
	//public Transform enemy;
	public Transform playerUnit;
	public Transform selectionCube;
	public Button basicAttack;
	public Button rally;
	public Button reinforce;

	//public Text enemyDamageText;
	public Text standardfCostText;
	public Text rallyFCostText;
	public Text reinforceFCostText;
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

	Unit rallyUnit;

	public static int hasAttacked = 0;

	public int fCost;
	public int rallyFCost;
	public int reinforceFCost;
	public float standardAttackAcc;

	public int standardAttackDam;

	public int standardAttackRange;
	public int rallyRange;

	public BattleStateMachine battleStateMachine;

	Animator Anim;

	public EndTurn endTurn;

	public Pathfinding Apathfinding;

	// Use this for initialization
	void Awake () {
		standardfCostText.text = fCost.ToString ();
		rallyFCostText.text = rallyFCost.ToString ();
		reinforceFCostText.text = reinforceFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		rally.gameObject.SetActive (false);
		reinforce.gameObject.SetActive (false);
		Abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		rallyFCostText.gameObject.SetActive (false);
		reinforceFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator>();
		rallyUnit = null;
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
			rally.gameObject.SetActive (true);
			reinforce.gameObject.SetActive (true);
			Abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive (true);
			rallyFCostText.gameObject.SetActive (true);
			reinforceFCostText.gameObject.SetActive (true);
		} else {
			basicAttack.gameObject.SetActive (false);
			rally.gameObject.SetActive (false);
			reinforce.gameObject.SetActive (false);
			Abilities.text = "";
			standardfCostText.gameObject.SetActive (false);
			rallyFCostText.gameObject.SetActive (false);
			reinforceFCostText.gameObject.SetActive (false);
		}

		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
			BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE) {
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardCheck() == false || hasAttacked != 0) {
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < rallyFCost || hasAttacked != 0) {
			rally.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardCheck() == true && hasAttacked == 0) {
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= rallyFCost && hasAttacked == 0) {
			rally.interactable = true;
		}
		if (Unit.fatigue < reinforceFCost || hasAttacked != 0) {
			reinforce.interactable = false;
		}
		if (Unit.fatigue >= reinforceFCost && hasAttacked == 0) {
			reinforce.interactable = true;
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
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			if (RallyModeEnabled () == true) {
				Rally ();
				attackSelected = 0;
			//	setAttackRange (rallyRange);
			//	attackRange.SetActive (false);
				endTurn.endTurnFunction1 ();
				endTurn.endTurnButton.interactable = false;
			}
		} else if (attackSelected == 2 
			&& RallyModeEnabled() == false
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
		//	setAttackRange (rallyRange);
		//	attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		} else if (attackSelected == 3
			&& Input.GetMouseButtonDown (0)
			&& BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) { 
			Reinforce ();
			attackSelected = 0;

			//attackRange.SetActive (false);
			//endTurn.endTurnFunction1 ();
			//endTurn.endTurnButton.interactable = false;
		} else if (attackSelected == 3 
			&& Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			Unit.selectedLight.gameObject.SetActive (false);
			//attackRange.SetActive (false);
		}

		//Display Chance to Hit
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


		}else {
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


	//Tests
	bool AccuracyTest(float AttackType){
		if (UnitStats.chanceToHit > 100) {
			UnitStats.chanceToHit =100;
		}
		if (UnitStats.chanceToHit < 0) {
			UnitStats.chanceToHit = 0;
		}
		int temp = Random.Range (1, 100);
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

	public bool RallyModeEnabled(){
		int x = 0;
			foreach (Unit u in Unit.notThisUnit) {
				if (Input.GetMouseButtonDown (0) == true
				   && selectionCube.position.x == u.transform.position.x
				   && selectionCube.position.z == u.transform.position.z) {
					x = 1;
					rallyUnit = u;
				}
			}

		if (x == 1) {
			return true;
		} else {
			return false;
		}

	}

	public void Rally(){
		Anim.Play ("Rally");
        Color c = new Color(0, 255, 0);
        Unit.damageText.color = c;
        Unit.damageText.text = "Rally";
        Unit.fadeDamageTextFunction();

		List <Node> adjacentSpaces = new List<Node>();
		adjacentSpaces = findAdjacentNodes (transform.position);
		Node[] NodeArray = adjacentSpaces.ToArray ();
		if (adjacentSpaces.Count > 0) {
			int x = Random.Range (0, adjacentSpaces.Count);
			Vector3 newSpot = new Vector3 (NodeArray [x].worldPosition.x, transform.position.y, NodeArray [x].worldPosition.z);
			rallyUnit.transform.position = newSpot;
		} else {
			int i = 0;
			do {
				adjacentSpaces = findAdjacentNodes (NodeArray [0].worldPosition);
				if (adjacentSpaces.Count > 0){
					i = 1;
				}
				NodeArray = adjacentSpaces.ToArray();
			} while (i == 0);
			int x = Random.Range (0, adjacentSpaces.Count);
			Vector3 newSpot = new Vector3 (NodeArray [x].worldPosition.x, transform.position.y, NodeArray [x].worldPosition.z);
			rallyUnit.transform.position = newSpot;
		}
		// move rally unit to position next to player
		rallyUnit.fatigue += 5;
		rallyUnit.thisUnitStats.accuracy += 1;
		rallyUnit.thisUnitStats.defense += 1;
		rallyUnit.fatigueDamageText.text = "+5 Stamina, +1 Def, +1 Acc";
		StartCoroutine (ClearText (rallyUnit, 2f));
		Unit.fatigue -= rallyFCost;
		hasAttacked++;
		foreach (Unit u in Unit.notThisUnit) {
			u.selectedLight.gameObject.SetActive (false);
		}
		return;
	}

	public void Reinforce(){
        Color c = new Color(0, 255, 0);
        Unit.damageText.color = c;
        Unit.damageText.text = "Reinforce";
        Unit.fadeDamageTextFunction();
        foreach (Unit u in Unit.notThisUnit) {
			u.maxFatigue += 1;
			u.fatigue += 1;
		}
		Unit.fatigue -= reinforceFCost;
		hasAttacked++;
		endTurn.endTurnFunction1 ();
		endTurn.endTurnButton.interactable = false;
		return;
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

	//checks
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
		} 
		//}
		//Debug.Log (i);
		if (i != 0) {
			return true;
		} else {
			return false;
		}
	}

	IEnumerator ClearText (Unit ralliedUnit, float delay){
		yield return new WaitForSeconds(delay);
		ralliedUnit.fatigueDamageText.text = null;
	}

	List <Node> findAdjacentNodes(Vector3 ptFind){
		//turnOffWalkables.TurnOnUnwalkables ();
		List <Node> adjacentNodes = new List<Node>();
		Vector3 A = new Vector3 (ptFind.x, transform.position.y, ptFind.z + 1);
		Node Zero = Apathfinding.grid.NodeFromWorldPoint (A);
		if (spaceCheck(A) == true) {
			adjacentNodes.Add (Zero);
		}
		Vector3 B = new Vector3 (ptFind.x, transform.position.y, ptFind.z - 1);
		Node One = Apathfinding.grid.NodeFromWorldPoint (B);
		if (spaceCheck(B) == true) {
			adjacentNodes.Add (One);
		}
		Vector3 C = new Vector3 (ptFind.x + 1, transform.position.y, ptFind.z);
		Node Two = Apathfinding.grid.NodeFromWorldPoint (C);
		if (spaceCheck(C) == true) {
			adjacentNodes.Add (Two);
		}
		Vector3 D = new Vector3 (ptFind.x - 1, transform.position.y, ptFind.z);
		Node Three = Apathfinding.grid.NodeFromWorldPoint (D);
		if (spaceCheck(D) == true) {
			adjacentNodes.Add (Three);
		}

		return adjacentNodes;
	}

	bool spaceCheck(Vector3 space){
		int i = 0;
		Node n = Apathfinding.grid.NodeFromWorldPoint (space);
		if (n.walkable == false) {
			i++;
		} 
		foreach (EnemyUnit e in battleStateMachine.enemyUnits) {
			if (e.transform.position.x == space.x && e.transform.position.z == space.z) {
				i++;
			}
		}
		foreach (Unit u in battleStateMachine.units) {
			if (u.transform.position.x == space.x && u.transform.position.z == space.z) {
				i++;
			}
		}
		if (i > 0) {
			return false;
			// i.e. occupied
		} else {
			return true;
		}
	}

	//Select Attack
	public void attackSelectedFunction(){
		attackSelected = 1;
		AttackDescription2.text = "Standard low cost attack";
		AttackModifiers2.text = null;
	}

	public void RallySelectedFunction(){
		attackSelected = 2;
		foreach (Unit u in Unit.notThisUnit) {
			u.selectedLight.gameObject.SetActive (true);
		}
		AttackDescription2.text = "Returns another Unit of Choice to Square adjacent leader, restoring 5 stamina and buffing accuracy and defense";
		AttackModifiers2.text = null;
	}

	public void ReinforceSelectedFunction(){
		attackSelected = 3;
		AttackDescription2.text = "Boost all allies Max Stamina by 1 and restore 1 stamina to each";
		AttackModifiers2.text = null;
	}


	//Descriptors (Called via Button mouseOver)
	public void standardDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Standard low cost attack";
			AttackModifiers.text = null;
		}
	}

	public void RallyDescription(){
		if (attackSelected == 0){
			AttackDescription.text = "Return another Ally of Choice to a square adjacent to Medic, restoring 5 stamina, buffing accuracy, and improving defense";
			AttackModifiers.text = null;
		}
	}


	public void ReinforceDescription(){
		if (attackSelected == 0) {
			AttackDescription.text = "Boost all allies Max Stamina by 1 and restore 1 Stamina to each";
			AttackModifiers.text = null;
		}
	}

	public void clearDescription(){
		AttackDescription.text = null;
		AttackModifiers.text = null;
	}
}
