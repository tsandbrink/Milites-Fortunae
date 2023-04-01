using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLeaderAttack : MonoBehaviour {
	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;

	public EnemyUnit reinforcementUnit;
	public Vector3 reinforcementSpot;

	public Unit playerUnit;
	public Pathfinding Apathfinding;
	UnitStats TargetUnitStats;

	int EnemyStandardFCost;
	public int rallyFCost;
	public int reinforceFCost;
	int hasUsedReinforce = 0;

	Animator Anim;



	// Use this for initialization
	void Start () {
		EnemyStandardFCost = BattleStateMachine.EnemyStandardFCost;
		Anim = GetComponent<Animator> ();
		reinforcementUnit.health = 0;
	}
	
	// Update is called once per frame


	public int LeaderChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution
		    && (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1
		    && thisEnemyUnit.fatigue >= EnemyStandardFCost) {
			return 1;
		} else if (findAdjacentNodes ().Count > 0
		           && thisEnemyUnit.fatigue >= rallyFCost) {
			return 2;
		} else if (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ) == 1
		           && thisEnemyUnit.fatigue >= EnemyStandardFCost) {
			return 1;
		} else if (thisEnemyUnit.fatigue >= reinforceFCost && hasUsedReinforce == 0) {
			return 3;
		} else {
			return 0;
		}
	}

	public bool RallyCheck(){
		int i = 0;
		int y = 0;
		List <Vector3> surroundingSpaces = new List<Vector3> ();
		Vector3 A = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
		Vector3 B = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1);
		Vector3 C = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
		Vector3 D = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
		surroundingSpaces.Add (A);
		surroundingSpaces.Add (B);
		surroundingSpaces.Add (C);
		surroundingSpaces.Add (D);
		for (int x = 0; x < surroundingSpaces.Count; x++) {
			if (spaceCheck (surroundingSpaces [x]) == false) {
				i++;
			}
		}
		if (i < 4) { //there is an open space
			foreach (Unit u in BattleStateMachine.units) {
				if (Mathf.Abs (u.transform.position.x - transform.position.x)
				    + Mathf.Abs (u.transform.position.z - transform.position.z) <= u.moveRange) {
					y++;
				}
			}
			foreach (EnemyUnit e in BattleStateMachine.enemyUnits) {
				if (e.health < 4) {
					y++;
				}
			}
			if (y > 0) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	bool spaceCheck(Vector3 space){
		int i = 0;
		Node n = Apathfinding.grid.NodeFromWorldPoint (space);
		if (n.walkable == false) {
			i++;
		} 
		foreach (EnemyUnit e in BattleStateMachine.enemyUnits) {
			if (e.transform.position.x == space.x && e.transform.position.z == space.z) {
				i++;
			}
		}
		foreach (Unit u in BattleStateMachine.units) {
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

	public void Rally1(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Rally";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("Rally", .5f);
	}

	void Rally(){
		EnemyUnit ralliedEnemyUnit = chooseRalliedEnemyUnit ();
		List <Node> adjacentSpaces = new List<Node>();
	
		adjacentSpaces = findAdjacentNodes ();
		int x = Random.Range (0, adjacentSpaces.Count);
		Node[] NodeArray = adjacentSpaces.ToArray();
	//	foreach (Node n in NodeArray) {

	//	}
		Vector3 newSpot = new Vector3(NodeArray[x].worldPosition.x, transform.position.y, NodeArray[x].worldPosition.z);
		ralliedEnemyUnit.transform.position = newSpot;
		ralliedEnemyUnit.fatigue += 5;
		ralliedEnemyUnit.thisEnemyStats.defense += 1;
		ralliedEnemyUnit.thisEnemyStats.accuracy += 1;
		ralliedEnemyUnit.fatigueDamageText.text = "+5 Stamina, +1 Def, +1 Acc";
		thisEnemyUnit.fatigue -= rallyFCost;
		StartCoroutine (ClearText (ralliedEnemyUnit, 2f));
	}

	IEnumerator ClearText (EnemyUnit ralliedUnit, float delay){
		yield return new WaitForSeconds(delay);
		ralliedUnit.fatigueDamageText.text = null;
	}

	public void Reinforce1(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Reinforce";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("Reinforce", .5f);
	}

	void Reinforce(){
		reinforcementUnit.health = reinforcementUnit.thisEnemyStats.maxHealth;
		reinforcementUnit.transform.position = reinforcementSpot;
		thisEnemyUnit.fatigue -= reinforceFCost;
		hasUsedReinforce++;
	}

	EnemyUnit chooseRalliedEnemyUnit(){
		EnemyUnit chosenEnemyUnit = null;
		float sum = 1000;
		foreach (EnemyUnit e in BattleStateMachine.enemyUnits) {
			if (//e.health < e.thisEnemyStats.maxHealth
			    //&& e.fatigue < e.maxFatigue
			    //&& 
				e.health + e.fatigue < sum
				&& e.health > 0
				&& e != thisEnemyUnit) {
				chosenEnemyUnit = e;
				sum = e.health + e.fatigue;
			}
		}
		return chosenEnemyUnit;
	}

	List <Node> findAdjacentNodes(){
		//turnOffWalkables.TurnOnUnwalkables ();
		List <Node> adjacentNodes = new List<Node>();
		Vector3 A = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
		Node Zero = Apathfinding.grid.NodeFromWorldPoint (A);
		if (spaceCheck(A) == true) {
			adjacentNodes.Add (Zero);
		}
		Vector3 B = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1);
		Node One = Apathfinding.grid.NodeFromWorldPoint (B);
		if (spaceCheck(B) == true) {
			adjacentNodes.Add (One);
		}
		Vector3 C = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
		Node Two = Apathfinding.grid.NodeFromWorldPoint (C);
		if (spaceCheck(C) == true) {
			adjacentNodes.Add (Two);
		}
		Vector3 D = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
		Node Three = Apathfinding.grid.NodeFromWorldPoint (D);
		if (spaceCheck(D) == true) {
			adjacentNodes.Add (Three);
		}
	
		return adjacentNodes;
	}
}
