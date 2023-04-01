using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour {
	

	PathRequestManager requestManager;
	public BattleStateMachine BattleStateMachine;
	public Grid grid;
	public Unit[] Units;
	public EnemyUnit[] enemyUnits;
	//public EnemyUnit EnemyUnit;
	public SelectUnits RosterManager;
    public Vector3[] safetyVectors;
    public EnemyTarget EnemyAI;
    public List<int> NodeList;
  //  public int loopBreakInt;
	public TurnOffWalkables turnOffWalkables;
	public int CounterBreak;
	public GameObject mine;
	public ParticleSystem[] mineExplosion;
	public RawImage descriptionPanel;
        // = new List<Node>();
    
    //public Node[] NodeArray;
	public static AudioSource selectedSound;

    void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}

	void Start(){
		Units = RosterManager.roster;
		selectedSound = GetComponent<AudioSource> ();
        selectedSound.volume = .8f;
	}

	//void Update(){

       
        
		
	//}
	
	
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		 FindPath(startPos,targetPos);
	}
	
	public Vector3[] FindPath(Vector3 startPos, Vector3 targetPos) {
		
		Vector3[] waypoints = new Vector3[0];
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);
      //  List<Node> NodeList = new List<Node>();
        //bool pathsucess = false;
		//if (//startNode.walkable && 
		 //   targetNode.walkable) {
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0) {

				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET) {
					NodeList.Add (currentNode.gridX);
				}
				if (currentNode == targetNode) {
                // pathsucess = true;
                NodeList.Clear();
					break;
				}
          //  if (NodeList.Count > loopBreakInt 
       //          && BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET)
           //     if (openSet.Count == 0 && currentNode != startNode)
            //    {
          //      Debug.Log(NodeList.Count);
           //         Debug.Log("Pathfinding Break");
                //    return safetyVectors;
		//		Debug.Log("Broke");
		//		Debug.Log (NodeList.Count);
         //           break;
            //    }
           
				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				int newMovementCostToNeighbour;
					if (!neighbour.walkable){ 
						closedSet.Add(neighbour);
                   
					}
					if (closedSet.Contains(neighbour)){
						continue;
					}
				if (EnemyAI.checkForEnemyUnits (neighbour) == true) {
					newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
				} else {
					newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour) + 100;
				}
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						if (neighbour.worldPosition.x == mine.transform.position.x 
						&& neighbour.worldPosition.z == mine.transform.position.z) {
							if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYATTACK
						    	|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE
						    	|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET) {
								neighbour.gCost += 40;
							}
						}
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
           
                
            
			}

        
		//}
		//yield return null;
		//if (pathSuccess) {
      //  if (//openSet.Count == 0)
      //      NodeList.Count > loopBreakInt && BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET)
     //   {
          //  Debug.Log("Pathfinding Break");
      //      NodeList.Clear();
      //      return safetyVectors;
     //   }
    //    else
     //   {
            waypoints = RetracePath(startNode, targetNode);
            	//requestManager.FinishedProcessingPath(waypoints,pathSuccess);
            return waypoints;
    //    }
		//} 
		//requestManager.FinishedProcessingPath(waypoints,pathSuccess);
		//return waypoints;
	}
	
	Vector3[] RetracePath(Node startNode, Node endNode) {
		EnemyUnit EnemyUnit = null;
	
			EnemyUnit = BattleStateMachine.thisEnemyUnit;
		
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
	
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE
			&& EnemyUnit.Type != "ARCHER") {
		//	currentNode = endNode.parent;
		}
		//path.Add (startNode);
		path.Add (endNode);
        int counter = 0;
        do {
			path.Add (currentNode);
			currentNode = currentNode.parent;
            counter++;
			if (counter > CounterBreak)
            {
				Debug.Log("CounterBreak");
                break;
            }
		} while (currentNode != startNode);
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE) {
		
				while (path.Count > EnemyUnit.moveRange/2 + 1) {
					path.RemoveAt (0);
				}
		
		}
		//path.Add (startNode);
		//path.Add (endNode);

			Vector3[] waypoints = SimplifyPath (path);
			Array.Reverse (waypoints);
			return waypoints;

	}
	
	Vector3[] SimplifyPath(List<Node> path) {
		EnemyUnit EnemyUnit = null;
	
			EnemyUnit = BattleStateMachine.thisEnemyUnit;
		
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE){
            EnemyUnit = determineThisEnemyUnit();
			EnemyUnit.fatigue -= path.Count - 1;
			if (EnemyUnit.fatigue < 0){
				EnemyUnit.fatigue = 0;
			}
		}
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE) {
			foreach (Unit u in Units){
				
				if (u.fatigue < 0){
					u.fatigue = 0;
				}
			}
		}
		for (int i = 1; i < path.Count; i ++) {
			//Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
			//if (directionNew != directionOld && directionNew.x <= .5 || directionNew.y <= .5) {
				waypoints.Add(path[i].worldPosition);
			//}

			//directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);


		                                                                            
		
		if (dstX > dstY)
			return 14*dstY + 10 * (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
	
	EnemyUnit determineThisEnemyUnit(){
		EnemyUnit selectedEnemyUnit = null;
		foreach (EnemyUnit e in enemyUnits) {
			if (e.isSelected == 1){
				selectedEnemyUnit = e;
			}
		}
		return selectedEnemyUnit;
	}
}
