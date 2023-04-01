using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class EnemyTarget : MonoBehaviour {
    //Contains the Decision Tree AI uses to select which Character it will move and what space to move the character to.

	//Vector3 temp;
	public EnemyUnit enemy;
	public EnemyUnit[] enemies;
	public EnemyUnit[] canAttacks;
	public Unit[] playerUnits;
	public GameObject[] unwalkables;
	public Button EndTurnButton;
	
	public Unit playerUnit; //i.e the player unit the AI will target
    public Unit dummy; //"unit" that isn't actually in the game - used as a placeholder in some functions 
	
	public BattleStateMachine BattleStateMachine;
	public Pathfinding pathfinding;
    public static int blocker; //used to control when functions get called 

	public Grid gameGrid;

	public TurnOffWalkables turnOffWalkables;

    public int i = 0;
	public int NodeScoreModifier = 10; //score assigned to grid spaces - AI determines where to move its unit based on the node's assigned score
	public int BattleIVswitch = 0;

	public bool chargeCheck;
	Unit tempForCharge;

    // Use this for initialization
   void Start()
    { 
        blocker = 1;
		chargeCheck = false;
		tempForCharge = null;
    }
	// Update is called once per frame
	void Update(){
		//checks if player has won the battle
        if (BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			if (checkEnemiesHealth () == true) {
				BattleStateMachine.currentState = BattleStateMachine.BattleStates.WIN;
				EndTurnButton.interactable = false;
			}
		}

	}

    //AI determines which Character it will move
    //Function Called by the BattleStatMachine when its the AI's turn
	public EnemyUnit MakeMove () { 
		//Debug.Log(BattleStateMachine.currentState);
		
        //Checks for grid spaces that are not walkable or an enemy Unit is already standing in and eliminates them as possible moves
        //by raising the cost (hCost) the pathfinding algorithm uses too high
        foreach (Node n in gameGrid.grid) {
			if (n.walkable == false) {
				n.hCost += 10000;
			}
			if (checkForEnemyUnits (n) == false) {
				n.hCost += 10000;
			}
		}
        //Removes the unwalkable flag from grid spaces so other functions can execute - still unwalkable because pathfinding cost (hCost) is really high
		turnOffWalkables.TurnOffUnwalkables ();
		
        //ChosenUnit will be what this Function Returns and will be the EnemyUnit the AI moves on its turn.
        EnemyUnit chosenUnit = null;
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET
		    && i == 0) {
			          
			//Makes an Array with all enemy units that can perform an attack
			canAttacks = findCanAttack ();
			i = 1; // i is used to make sure function only runs once 

			if (canAttacks.Length == 1) { //if only 1 enemy unit can attack function ends
				chosenUnit = canAttacks [0];
				chosenUnit.isSelected = 1;
			} else if (canAttacks.Length > 1) { //if more than 1 enemy unit can attack calls function to choose between them
				chosenUnit = chooseFromCanAttacks ();
				chosenUnit.isSelected = 1;
			} else if (canAttacks.Length == 0) { //if no enemies can attack then defaults to Soldier(i.e. murmillo) which can activate counter attack
				if (checkForEnemySoldiers () == true) {
					chosenUnit = ChooseEnemySoldier ();
					chosenUnit.isSelected = 1;
				} else { //if no enemy soldiers calls the function to choose among other enemy units 
					chosenUnit = chooseFromCannotAttacks ();
					chosenUnit.isSelected = 1;
				}
				
			}
			
		}
		
		return chosenUnit;
	}
	
    //After the AI picks its Unit it then Picks a Player Unit to Target using this function with chosenUnit as parameter
	public Unit continueMakingMove(EnemyUnit chosenUnit){	
		if (chosenUnit.thisEnemyStats.tripped == 0)// && blocker == 0	)
            {
                
				//if the chosenUnit is tripped (happens when only 1 enemyUnit remains) makes sure the chosenUnit doesn't move
				
					chosenUnit.target = chosenUnit.targetNode.worldPosition;
			if (chosenUnit.Type == "ARCHER") {
				playerUnit = ArcherSelectTarget (chosenUnit); //Archers require different algorithm to Select their Target because they attack at a distance
			} else {
				playerUnit = selectTarget (chosenUnit); //All other unit types use this algorithm which measures the distance from the chosenUnit
                                                        //to player units
			}
                
				float distanceX = chosenUnit.transform.position.x - playerUnit.transform.position.x;
                float distanceZ = chosenUnit.transform.position.z - playerUnit.transform.position.z;
                foreach (Unit u in playerUnits)
                {
                    u.layerswitch.thisCube.gameObject.SetActive(true);
                }
                //playerUnit.layerswitch.thisCube.gameObject.SetActive(false);
			
            //determines if Archer needs to move or if it can proceed with Attack - Can proceed with attack if it is within shooting range 
            if (chosenUnit.Type == "ARCHER") {
				int i = 0;
				if (chosenUnit.target.x != chosenUnit.transform.position.x || 
					chosenUnit.target.z != chosenUnit.transform.position.z) {
					Vector3[] path = pathfinding.FindPath (chosenUnit.transform.position, chosenUnit.target);
					foreach (Vector3 v in path) {
						foreach (Unit u in playerUnits) {
							if (u.transform.position.x == v.x && u.transform.position.z == v.z && u.health > 0) {
								i++;
							}
						}
					}
				}
				if (i > 0) {
					BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYATTACK;
				} else {
					BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYCHOICE;
				}
			} else {
					if (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ) > 1) {
						BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYCHOICE;

					} else if (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ) == 1) {
						BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYATTACK;
					}
				}
            }
            else
            {
                BattleStateMachine.currentState = BattleStateMachine.BattleStates.PLAYERCHOICEMOVE;
            }
           // blocker = 1;           

		return playerUnit;
		//Debug.Log ("Done");
		
	}
	
	Unit selectTarget(EnemyUnit enemy1){
		float highestDistance = 1000;
		float highestHealth = 1000;
		Unit closest = null;
		if (chargeCheck == true) {
			closest = tempForCharge;
		}
		//Vector3 position = gameObject.transform.position;
		else {
			foreach (Unit u in playerUnits) {
				//if u.health is less than basic dam + range then closest = u
				float distance = Mathf.Abs (enemy1.targetNode.worldPosition.x - u.transform.position.x)
				                + Mathf.Abs (enemy1.targetNode.worldPosition.z - u.transform.position.z);
				//if (enemy1.fatigue >= BattleStateMachine.EnemyStandardFCost 
				//	+ pathfinding.FindPath(enemy1.transform.position, u.transform.position).Length - 1 &&
				//	u.health <= enemy1.thisEnemyStats.attack - u.thisUnitStats.constitution) {
				//	closest = u;
				//}
				//else 
				if (u.health > 0 && u.gameObject.activeInHierarchy == true) {
					if (distance < highestDistance) {
						closest = u;
						highestDistance = distance;
					} else if (distance == highestDistance) {
						if (u.health < highestHealth) {
							closest = u;
							highestHealth = u.health;
						}
					}
				}
			}
		}
        return closest;
	}

	Unit ArcherSelectTarget(EnemyUnit enemy1){
		float highestDistance = 1000;
		Unit closest = null;
		Vector3 temp1 = new Vector3 (enemy1.transform.position.x, enemy1.transform.position.y + 1f, enemy1.transform.position.z);
		Vector3 temp3 = new Vector3 (enemy1.targetNode.worldPosition.x, enemy1.targetNode.worldPosition.y + 1f, enemy1.targetNode.worldPosition.z);
		//Vector3 position = gameObject.transform.position;
		foreach (Unit u in playerUnits) {
			u.colliderBox.SetActive (false);
			if (enemy1.enemyArcherAttack.standingInSight == 1) {
				float distanceX = Mathf.Abs (enemy1.transform.position.x - u.transform.position.x);
				float distanceZ = Mathf.Abs (enemy1.transform.position.z - u.transform.position.z);
				float distance = distanceX + distanceZ;
				Vector3 temp2 = new Vector3 (u.transform.position.x, u.transform.position.y + 1f, u.transform.position.z);
				if (u.health > 0 && u.gameObject.activeInHierarchy == true && Physics.Linecast (temp1, temp2) == false) {
					if (distance < highestDistance) {
						closest = u;
						highestDistance = distance;
					} else if (distance == highestDistance) {
						if (Random.Range (1, 10) > 5) {
							closest = u;
						}
					}
				}

			}
			else {
				float distanceX = Mathf.Abs (enemy1.targetNode.worldPosition.x - u.transform.position.x);
				float distanceZ = Mathf.Abs (enemy1.targetNode.worldPosition.z - u.transform.position.z);
				float distance = distanceX + distanceZ;
				Vector3 temp2 = new Vector3 (u.transform.position.x, u.transform.position.y + 1f, u.transform.position.z);
				if (u.health > 0 && u.gameObject.activeInHierarchy == true && Physics.Linecast (temp3, temp2) == false) {
					if (distance < highestDistance) {
						closest = u;
						highestDistance = distance;
					} else if (distance == highestDistance) {
						if (Random.Range (1, 10) > 5) {
							closest = u;
						}
					}
				}
			}
			u.colliderBox.SetActive (true);
		}
		if (closest == null) {
			closest = selectTarget (enemy1);
		}
		return closest;
	}

	public Node archerSelectTargetNode (EnemyUnit enemy1){
		Node targetNode = null;
		enemy1.canAttack = false;
		float highestNodeScore = -5000;
		List<Node> nodesInRange = new List<Node> ();
		foreach (Node n in gameGrid.grid) {
			if ((Mathf.Abs (n.worldPosition.x - enemy1.transform.position.x) + Mathf.Abs (n.worldPosition.z - enemy1.transform.position.z)
				<= enemy1.moveRange / 2) 
				&& n.walkable == true
				&& n.hCost < 10000
				&& n.worldPosition != enemy1.transform.position
				&& checkForPlayerUnits(n) == true
				&& checkForEnemyUnits(n) == true){
			//	if (n.worldPosition.x == .5 && n.worldPosition.z == -6.5) {
			//		Debug.Log ("TargetedNode1");
			//	}
				nodesInRange.Add (n);
				n.NodeScore = 0;
			}
		}
	
		for (int i = 0; i < nodesInRange.Count; i++) {
			Vector3[] path = pathfinding.FindPath (enemy1.transform.position, nodesInRange [i].worldPosition);
			List<Node> NodePath = new List<Node>();
		//	if (nodesInRange [i].worldPosition.x == .5 && nodesInRange [i].worldPosition.z == -6.5) {
		//		Debug.Log ("TargetedNode");
		//	}
			nodesInRange [i].NodeScore -= path.Length*10;
			int x = 0;
		
			foreach (Vector3 v in path) {
				Node n = gameGrid.NodeFromWorldPoint (v);
				NodePath.Add (n);
			
			}
			foreach (Node n in NodePath) {				
				if (checkForEnemyUnits (n) == false) {
					x = 11000;
					nodesInRange [i].NodeScore -= 1000;
				}
				if (checkForPlayerUnits(n) == false){
					x += 11000;
					nodesInRange [i].NodeScore -= 1000;
				}
			
			}

			if (path.Length > enemy1.moveRange / 2 || x >= 10000){
				//nodesInRange.RemoveAt (i);
				nodesInRange[i].NodeScore -= 1000;
			}
		}
		for (int i = 0; i < nodesInRange.Count; i++) {
			Vector3 temp = new Vector3 (nodesInRange[i].worldPosition.x, nodesInRange[i].worldPosition.y + 1f, nodesInRange[i].worldPosition.z);
			foreach (Unit u in playerUnits) {
				u.colliderBox.SetActive (false);
				Vector3 temp2 = new Vector3 (u.transform.position.x, u.transform.position.y +1f, u.transform.position.z);
				if (u.health > 0
				    && u.gameObject.activeInHierarchy == true
				    && (Mathf.Abs (nodesInRange [i].worldPosition.x - u.transform.position.x)
						+ Mathf.Abs (nodesInRange [i].worldPosition.z - u.transform.position.z)) <= enemy1.enemyArcherAttack.standardShotRange
					&& Physics.Linecast (nodesInRange [i].worldPosition, u.transform.position) == false) {
					Vector3[] path = pathfinding.FindPath (enemy1.transform.position, nodesInRange [i].worldPosition);
					if (enemy1.fatigue >= path.Length + BattleStateMachine.EnemyStandardFCost) {
						if (u.health < enemy1.thisEnemyStats.attack - u.thisUnitStats.constitution) {
							u.colliderBox.SetActive (true);
							nodesInRange [i].NodeScore += 100;
							enemy1.canAttack = true;
							break;
						} else {
							u.colliderBox.SetActive (true);
							nodesInRange [i].NodeScore += 70;
							enemy1.canAttack = true;
							break;
						}
					}
				}
				u.colliderBox.SetActive (true);
			}
			if (nodesInRange [i].worldPosition.x == .5
				&& nodesInRange [i].worldPosition.z == 1.5) {
			}

			nodesInRange [i].NodeScore += 5 * enemy1.fatigue;
		}
		foreach (Node n in nodesInRange) {
		//??Maybe this will work:
			if (enemy1.fatigue < 2) {
				n.NodeScore -= 500;
			}
			if (n.NodeScore > highestNodeScore) {
				targetNode = n;
				highestNodeScore = n.NodeScore;
			}
		}
		//enemy1.targetNode = targetNode;

		return targetNode;
	}

	public Node meleeSelectTargetNode(EnemyUnit enemy1){
		Node targetNode = null;
		float highestNodeScore = -5000;
		List<Node> nodesInRange = new List<Node> ();
		enemy1.canAttack = false;
		//Find Nodes in Move Range
		foreach (Node n in gameGrid.grid) {
			if ((Mathf.Abs (n.worldPosition.x - enemy1.transform.position.x) + Mathf.Abs (n.worldPosition.z - enemy1.transform.position.z)
				<= enemy1.moveRange / 2) 
				&& n.walkable == true
				&& n.hCost < 10000
				&& n.worldPosition != enemy1.transform.position
				&& checkForPlayerUnits(n) == true
				&& checkForEnemyUnits(n) == true){
				nodesInRange.Add (n);
				n.NodeScore = 0;
			}
		}
		for (int i = 0; i < nodesInRange.Count; i++) {
			Vector3[] path = pathfinding.FindPath (enemy1.transform.position, nodesInRange [i].worldPosition);
			List<Node> NodePath = new List<Node>();
			int x = 0;
			nodesInRange [i].NodeScore -= path.Length * 10;
			foreach (Vector3 v in path) {
				Node n = gameGrid.NodeFromWorldPoint (v);
				NodePath.Add (n);

			}
			foreach (Node n in NodePath) {
				//x += n.hCost;
				if (checkForEnemyUnits (n) == false) {
					//Debug.Log ("false" + n.worldPosition);
					x += 11000;
					nodesInRange [i].NodeScore -= 1000;
				}
				if (checkForPlayerUnits(n) == false){
					x += 11000;
					nodesInRange [i].NodeScore -= 1000;
				}
				if (checkForUnwalkables (n) == false) {
					x += 110000;
					nodesInRange [i].NodeScore -= 1000;
				}
			}
			if (path.Length > enemy1.moveRange / 2 || x >= 10000){
				nodesInRange.RemoveAt (i);
			}
		

		}
		//Assign Node Score
		for (int i = 0; i < nodesInRange.Count; i++) {
			//nodesInRange [i].NodeScore = 0;
			int x = 100;
			foreach (Unit u in playerUnits) {
				//int x = 100;
				//find nodes adjacent to player units
				if (u.health > 0
				    && u.gameObject.activeInHierarchy == true
				    && (Mathf.Abs (nodesInRange [i].worldPosition.x - u.transform.position.x)
				    + Mathf.Abs (nodesInRange [i].worldPosition.z - u.transform.position.z)) == 1) {
					Vector3[] path = pathfinding.FindPath (enemy1.transform.position, nodesInRange [i].worldPosition);

					if (enemy1.fatigue >= path.Length + BattleStateMachine.EnemyStandardFCost
						&& path.Length < x) {
						if (u.health < enemy1.thisEnemyStats.attack - u.thisUnitStats.constitution) {
							nodesInRange [i].NodeScore += 230;
							enemy1.canAttack = true;
							//break;
						//	nodesInRange [i].NodeScore -= Mathf.RoundToInt(5 * (Mathf.Abs (nodesInRange [i].worldPosition.x - u.transform.position.x)
						//		+ Mathf.Abs (nodesInRange [i].worldPosition.z - u.transform.position.z)));
						} 
						else if(u.health < enemy1.thisEnemyStats.attack - u.thisUnitStats.constitution + BattleStateMachine.powerAttackDam
							&& enemy1.fatigue >= path.Length + BattleStateMachine.EnemyPowerFCost
							&& enemy1.Type == "HEAVY INFANTRY"){
							nodesInRange [i].NodeScore += 100;
							enemy1.canAttack = true;
						}
						else {
							nodesInRange [i].NodeScore += 70;
							enemy1.canAttack = true;
						//		nodesInRange [i].NodeScore -= Mathf.RoundToInt(5 * (Mathf.Abs (nodesInRange [i].worldPosition.x - u.transform.position.x)
						//		+ Mathf.Abs (nodesInRange [i].worldPosition.z - u.transform.position.z)));
							//break;
						}
						//x++;
					}
					x = path.Length;
				} else {
					//find the node distance from unit
					if (u.health > 0 && u.gameObject.activeInHierarchy == true) {
					//	Debug.Log (u.transform.position);
					//	Debug.Log (nodesInRange[i].worldPosition);
						Vector3[] path = pathfinding.FindPath (nodesInRange [i].worldPosition, u.transform.position);
						if (enemy1.Type == "LIGHT INFANTRY" || enemy1.Type == "SKELETON") {
							Vector3[] path2 = pathfinding.FindPath (enemy1.transform.position, u.transform.position);
							if (nodesInRange [i].worldPosition.x == u.transform.position.x
								|| nodesInRange [i].worldPosition.y == u.transform.position.z) {
								Vector3[] path3 = pathfinding.FindPath (enemy1.transform.position, nodesInRange [i].worldPosition);
								if (path.Length <= enemy1.enemyLightInfantryAttack.chargeRange
									&& path2.Length >= enemy1.moveRange/2
									&& enemy1.fatigue >= enemy1.enemyLightInfantryAttack.ChargeFCost + path3.Length) {
									nodesInRange [i].NodeScore += 10*path.Length;
									enemy1.canAttack = true;
								} else {
									//Debug.Log ("Check2");
									nodesInRange [i].NodeScore -= path.Length * NodeScoreModifier;
								}
							} else {
								//Debug.Log ("Check3");
								nodesInRange [i].NodeScore -= path.Length * NodeScoreModifier;
							}
						} else {
							nodesInRange [i].NodeScore -= path.Length * NodeScoreModifier;
						}
					}
				}
				
			}

			//nodesInRange [i].NodeScore -= (Mathf.RoundToInt(10 * (Mathf.Abs (nodesInRange [i].worldPosition.x - enemy1.transform.position.x)
			//	+ Mathf.Abs (nodesInRange [i].worldPosition.z - enemy1.transform.position.z))));
			nodesInRange [i].NodeScore += 20 * enemy1.fatigue;
			if (nodesInRange [i].worldPosition.x == 5.5 && nodesInRange [i].worldPosition.z == .5) {
		//		Debug.Log (nodesInRange[i].NodeScore +enemy1.name);
			}
		}

		foreach (Node n in nodesInRange) {
			if (n.worldPosition.x == .5 && n.worldPosition.z == .5 && BattleIVswitch == 1) {
				n.NodeScore -= 100000;
			}
			if (n.NodeScore > highestNodeScore) {
				targetNode = n;
				highestNodeScore = n.NodeScore;
			}
		}
	//	Debug.Log (targetNode.worldPosition + enemy1.name);
	//	Debug.Log (targetNode.NodeScore + enemy1.name);
		//enemy1.targetNode = targetNode;
		if (enemy1.fatigue < 2) {
			targetNode.NodeScore -= 600; //trying to favor moving far away units into position
		}
		return targetNode;
	}
		

	EnemyUnit[] findCanAttack(){
		List<EnemyUnit> temp = new List<EnemyUnit>();
        
		int a = 0;
        foreach (EnemyUnit e in enemies)
        {	
			if (e.health > 0 && e.thisEnemyStats.tripped == 0 && e.fatigue > 1){
						
				if (e.Type == "ARCHER") {
					if (checkForPlayerUnitsInSight (e) == true && e.fatigue >= e.enemyArcherAttack.standardShotFCost) {
						e.targetNode.NodeScore += e.fatigue * 5;
						temp.Add (e);
						e.enemyArcherAttack.playerInSight = 1;
					//	Debug.Log (e.targetNode.NodeScore);
					} else if (checkForPlayerUnitsInSight (e) == true && e.fatigue < e.enemyArcherAttack.standardShotFCost) {
						e.targetNode.NodeScore = -150;
					} else {
						e.targetNode = archerSelectTargetNode (e);
						float s = e.targetNode.NodeScore;
						if (e.canAttack == true && e.thisEnemyStats.tripped == 0 && e.health > 0) {
							temp.Add (e);
						}
						a++;
					}
				} else if (e.Type == "LIGHT INFANTRY" || e.Type == "SKELETON") {
					if (checkForPlayerUnitsInCharge (e) == true && e.fatigue >= e.enemyLightInfantryAttack.ChargeFCost) {
						e.targetNode.NodeScore += e.fatigue * 5;
						temp.Add (e);
					} else if (checkForAdjacentPlayerUnits (e) == true && e.fatigue >= BattleStateMachine.EnemyStandardFCost) {
						e.targetNode.NodeScore += e.fatigue * 5;
						temp.Add (e);
					} else if (checkForAdjacentPlayerUnits (e) == true && e.fatigue < BattleStateMachine.EnemyStandardFCost) {
						if (e.Type != "MURMILLO") {
							e.targetNode.NodeScore = 0;
							continue;
						} else {
							e.targetNode.NodeScore = 10;
						}
					} else {
						e.targetNode = meleeSelectTargetNode (e);
						if (e.canAttack == true && e.thisEnemyStats.tripped == 0 && e.health > 0) {
							temp.Add (e);
						}
					}	
				} else if (e.Type == "LEADER") {
					if (e.enemyLeaderAttack.RallyCheck () == true
					    && e.fatigue >= e.enemyLeaderAttack.rallyFCost) {
						e.targetNode = pathfinding.grid.NodeFromWorldPoint (e.transform.position);
						e.targetNode.NodeScore += 300;
						temp.Add (e);
					} else if (checkForAdjacentPlayerUnits (e) == true && e.fatigue >= BattleStateMachine.EnemyStandardFCost) {
						e.targetNode.NodeScore += e.fatigue * 4;
					//	Debug.Log ("Adjacent");
					//	Debug.Log (e.targetNode.NodeScore);
						temp.Add (e);
					} else if (checkForAdjacentPlayerUnits (e) == true && e.fatigue < BattleStateMachine.EnemyStandardFCost) {
						if (e.Type != "MURMILLO") {
							e.targetNode.NodeScore = 0;
							continue;
						} else {
							e.targetNode.NodeScore = 10;
						}
					} else {
						e.targetNode = meleeSelectTargetNode (e);
						if (e.canAttack == true && e.thisEnemyStats.tripped == 0 && e.health > 0) {
							temp.Add (e);
						}
					}	
				}
				else {
					if (checkForAdjacentPlayerUnits(e) == true && e.fatigue >= BattleStateMachine.EnemyStandardFCost) {
						e.targetNode.NodeScore += e.fatigue * 5;
					//	Debug.Log ("Adjacent");
					//	Debug.Log (e.targetNode.NodeScore);
						temp.Add (e);
					} else if (checkForAdjacentPlayerUnits(e) == true && e.fatigue < BattleStateMachine.EnemyStandardFCost){
					//	Debug.Log ("adjacent");
					//	e.targetNode = gameGrid.NodeFromWorldPoint (e.transform.position);
						if (e.Type != "MURMILLO") {
							e.targetNode.NodeScore -= 50; //It was 0 before if this causes things to fuck up
							continue;
						} else if (e.Type == "MURMILLO" && e.thisEnemyStats.CounterAttackActivated == 3) {
							e.targetNode.NodeScore = 0;
						} else {
							e.targetNode.NodeScore -= 40;
						}
					} else {
						e.targetNode = meleeSelectTargetNode (e);
						if (e.canAttack == true && e.thisEnemyStats.tripped == 0 && e.health > 0) {
							temp.Add (e);
						}
					}               
				} 			
			}
		}        
	
		return temp.ToArray();
	}

	EnemyUnit chooseFromCanAttacks(){
		float highestNodeScore = -5000;
		EnemyUnit highest = null;
		foreach (EnemyUnit e in canAttacks) {
			if (e.targetNode.NodeScore > highestNodeScore){
				highestNodeScore = e.targetNode.NodeScore;
				highest = e;
			}
			else if (e.targetNode.NodeScore == highestNodeScore){
				int x = Random.Range(1, 10);
				if (x > 5){
					highest = e;
				}
			}
		}
		return highest;
	}

	EnemyUnit chooseFromCannotAttacks(){
		float highestNodeScore = -50000;
		EnemyUnit highest = null;
		foreach (EnemyUnit e in enemies) {
		//	Debug.Log (e.targetNode.worldPosition);
			if (e.thisEnemyStats.health > 0 && e.thisEnemyStats.tripped == 0) {
				// consider for skeleton battle && e.fatigue > 1) {
				if (e.fatigue < 2) {
					e.targetNode.NodeScore -= 500;
				}
				if (e.targetNode.NodeScore > highestNodeScore && e.thisEnemyStats.health > 0){
					highestNodeScore = e.targetNode.NodeScore;
					highest = e;
				}
				else if (e.targetNode.NodeScore == highestNodeScore){
					int x = Random.Range(1, 10);
					if (x > 5){
						highest = e;
					}
				}
			}

		}
		//Debug.Log (highest.thisEnemyStats.enemyName);
		return highest;
	}     

	bool checkForEnemySoldiers(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (e.Type == "MURMILLO" && e.health > 0 && e.enemySoldierAttack.skeleton == false) {
				i++;
				
			}
		}
		if (i > 0) {
			return true;
		} else {
			return false;
		}
	}

	EnemyUnit ChooseEnemySoldier(){
		EnemyUnit soldier = null;
		foreach (EnemyUnit e in enemies) {
			if (e.Type == "MURMILLO" && e.health > 0 && e.thisEnemyStats.tripped == 0 && e.fatigue > 0 
				&& e.thisEnemyStats.CounterAttackActivated == 0) {
				soldier = e;
			}
		}
		if (soldier != null) {
			return soldier;
		} else {
			return chooseFromCannotAttacks ();
		}
	}

	//Health Checks
	public bool checkEnemiesHealth(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (e.health > 0 && e.gameObject.activeInHierarchy == true){
				i++;
			}
		}
		if (i == 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool checkPlayerHealth(){
		int i = 0;
		foreach (Unit u in playerUnits) {
			if (u.health > 0 && u.gameObject.activeInHierarchy == true){
				i++;
			}
		}
		if (i == 0) {
			return true;
		} else {
			return false;
		}
	}

	bool checkForPlayerUnits(Node n){
		int i = 0;
		foreach (Unit u in playerUnits) {
			if (n.worldPosition.x == u.transform.position.x
			    && n.worldPosition.z == u.transform.position.z
				&& u.health > 0
				&& u.gameObject.activeInHierarchy == true) {
				i++;
			}
		}
		if (i > 0) {
			return false;
		} else {
			return true;
		}
	}

	public bool checkForEnemyUnits (Node n){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (n.worldPosition.x == e.transform.position.x
				&& n.worldPosition.z == e.transform.position.z) {
				i++;
			}
		}
		if (i > 0) {
			return false;
		} else {
			return true;
		}
	}

	bool checkForUnwalkables(Node n){
		int i = 0;
		foreach (GameObject g in unwalkables) {
			if (n.worldPosition.x == g.transform.position.x
				&& n.worldPosition.z == g.transform.position.z) {
				i++;
			}
		}
		if (i > 0) {
			return false;
		} else {
			return true;
		}
	}

	bool checkForAdjacentPlayerUnits(EnemyUnit enemyUnit){
		int i = 0;
		enemyUnit.targetNode = gameGrid.NodeFromWorldPoint (enemyUnit.transform.position);
		foreach (Unit u in playerUnits) {
			if (Mathf.Abs(enemyUnit.transform.position.x - u.transform.position.x) 
				+ Mathf.Abs(enemyUnit.transform.position.z - u.transform.position.z) == 1
				&& u.health > 0){
				i++;
				if (u.health <= enemyUnit.thisEnemyStats.attack - u.thisUnitStats.constitution) {
					enemyUnit.targetNode.NodeScore = 200;
				} else {
					enemyUnit.targetNode.NodeScore = 70;
				}
				break;
			}
		}
		if (i > 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool checkForPlayerUnitsInCharge(EnemyUnit enemyUnit){
		int i = 0;
		Vector3 temp = new Vector3 (enemyUnit.transform.position.x, enemyUnit.transform.position.y + .1f, enemyUnit.transform.position.z);
		enemyUnit.targetNode = gameGrid.NodeFromWorldPoint (enemyUnit.transform.position);
		enemyUnit.colliderBox.SetActive (false);
		//turnOffWalkables.TurnOffUnwalkables ();
		//turnOffWalkables.TurnOffPlayerUnwalkables ();
		foreach (Unit u in playerUnits) {
			u.colliderBox.SetActive (false);
			Vector3 temp2 = new Vector3 (u.transform.position.x, u.transform.position.y +.1f, u.transform.position.z);
			float distanceX = Mathf.Abs (enemyUnit.transform.position.x - u.transform.position.x);
			float distanceZ = Mathf.Abs (enemyUnit.transform.position.z - u.transform.position.z);
			if (distanceX == 0 && distanceZ > 1 && distanceZ <= enemyUnit.enemyLightInfantryAttack.chargeRange
			    && Physics.Linecast (temp, temp2) == false
			    && u.health > 0
				&& enemyUnit.fatigue >= enemyUnit.enemyLightInfantryAttack.ChargeFCost
				&& u.gameObject.activeInHierarchy == true
                && enemyUnit.enemyLightInfantryAttack.finalChargeCheck(u, enemyUnit) == true){
				i++;
				tempForCharge = u;
				if (u.health <= enemyUnit.thisEnemyStats.attack - u.thisUnitStats.constitution) {
					enemyUnit.targetNode.NodeScore = 200;
					u.colliderBox.SetActive (true);
				} else {
					u.colliderBox.SetActive (true);
					enemyUnit.targetNode.NodeScore = 200;
				}
				enemyUnit.colliderBox.SetActive (true);
				break;
			} else if (distanceZ == 0 && distanceX > 1 && distanceX <= enemyUnit.enemyLightInfantryAttack.chargeRange
			    && Physics.Linecast (temp, temp2) == false
			    && u.health > 0
				&& enemyUnit.fatigue >= enemyUnit.enemyLightInfantryAttack.ChargeFCost
				&& u.gameObject.activeInHierarchy == true
                && enemyUnit.enemyLightInfantryAttack.finalChargeCheck(u, enemyUnit) == true) {
				i++;
				tempForCharge = u;
				if (u.health <= enemyUnit.thisEnemyStats.attack - u.thisUnitStats.constitution) {
					enemyUnit.targetNode.NodeScore = 200;
					u.colliderBox.SetActive (true);
				} else {
					u.colliderBox.SetActive (true);
					enemyUnit.targetNode.NodeScore = 200;
				}
				enemyUnit.colliderBox.SetActive (true);
				break;
			}
			u.colliderBox.SetActive (true);
		}
		enemyUnit.colliderBox.SetActive (true);
		//turnOffWalkables.TurnOnUnwalkables ();
		//turnOffWalkables.TurnOnPlayerUnwalkables ();
		if (i > 0) {
			enemyUnit.enemyLightInfantryAttack.inCharge = 1;
			chargeCheck = true;
			return true;
		} else {
			return false;
		}
	}

	bool checkForPlayerUnitsInSight(EnemyUnit enemyUnit){
		int i = 0;
		Vector3 temp = new Vector3 (enemyUnit.transform.position.x, enemyUnit.transform.position.y + .75f, enemyUnit.transform.position.z);
		enemyUnit.targetNode = gameGrid.NodeFromWorldPoint (enemyUnit.transform.position);
		foreach (Unit u in playerUnits) {
			u.colliderBox.SetActive (false);
			Vector3 temp2 = new Vector3 (u.transform.position.x, u.transform.position.y + .75f, u.transform.position.z);
			if (Mathf.Abs (enemyUnit.transform.position.x - u.transform.position.x)
			    + Mathf.Abs (enemyUnit.transform.position.z - u.transform.position.z) <= enemyUnit.enemyArcherAttack.standardShotRange
			    && Mathf.Abs (enemyUnit.transform.position.x - u.transform.position.x)
			    + Mathf.Abs (enemyUnit.transform.position.z - u.transform.position.z) > 1
				&& Physics.Linecast(temp, temp2) == false
				&& u.health > 0) {
				i++;
				if (u.health <= enemyUnit.thisEnemyStats.attack - u.thisUnitStats.constitution) {
					enemyUnit.targetNode.NodeScore = 200;
					u.colliderBox.SetActive (true);
				} else {
					u.colliderBox.SetActive (true);
					enemyUnit.targetNode.NodeScore = 70;
				}
				break;
			}
			u.colliderBox.SetActive (true);
		}
		if (i > 0) {
			enemyUnit.enemyArcherAttack.standingInSight = 1;
			return true;
		} else {
			return false;
		}
	}
}
