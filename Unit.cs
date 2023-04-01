using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour {

	Transform target;
	//public Transform enemy;
	public Transform thisUnit;
	//Transform selectionCube;
	public Unit[] notThisUnit;
	public float speed = 1;
	public int moveRange = 9;
	public int fatigue;
	public int maxFatigue;
	public Vector3[] path;
	public int targetIndex;
	public int hasMoved = 0;
	public int isSelected = 0;
	public UnitStats thisUnitStats;
	public float health;
	public TileMapMouse TileMapMouse;
	public TileMapMouse startAreaMap;
	public Light selectedLight;

	public Pathfinding pathfinding;
	public Text fatigueText;
	public Text fatigueNumber;
	public Text movementText;
	public Text damageText;
	public Text fatigueDamageText;

    public String type;
    public Text typeText;

	public Text descriptionText;
	public Text modifiersText;

	float targetX;
	float targetZ;

	public int isOnRoster;

	public LayerSwitch layerswitch;

	public int isMoving = 0;
	public GameObject colliderBox;

	public int acc;
	public int pow;
	public int def;

	public Image accImage;
	public Image powImage;
	public Image defImage;

	public bool fixedStartPoint = false;



	void Start() {
		acc = 1;
		pow = 0;
		def = 0;
		fatigue = maxFatigue;
		isSelected = 0;
		selectedLight.gameObject.SetActive (false);
		target = TileMapMouse.selectionCube;
        Color c = new Color(255, 0, 0);
        damageText.color = c;
        damageText.fontSize = 3;
        fatigueDamageText.fontSize = 3;
	}

	void Update() {
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.START) {
			health = thisUnitStats.maxHealth;
            thisUnitStats.health = thisUnitStats.maxHealth;
		}
		health = thisUnitStats.health;
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.START) {
			targetX = target.transform.position.x - gameObject.transform.position.x;
			targetZ = target.transform.position.z - gameObject.transform.position.z;
		}
		//selectionCube = TileMapMouse.selectionCube;
		//target = selectionCube;
		//Debug.Log ( targetX);
		//Debug.Log ( targetZ);


		if ((target.position.x == thisUnit.position.x && target.position.z == thisUnit.position.z 
		     && CheckOtherUnitsSelected()==true)|| 
		    isSelected == 1) { 
			thisUnitStats.nameText.text = thisUnitStats.unitName.ToString();
			thisUnitStats.healthNumberText.text = health.ToString() + "/" + thisUnitStats.maxHealth.ToString();
			fatigueText.text = "STAMINA";
			fatigueNumber.text = fatigue.ToString () + "/" + maxFatigue.ToString ();
            typeText.text = type;
			if (fatigue >= moveRange / 2) {
				movementText.text = (moveRange / 2).ToString ();
			} else if (fatigue < moveRange / 2) {
				movementText.text = fatigue.ToString();
			}
		}
		if (fatigue > maxFatigue) {
			fatigue = maxFatigue;
		}

		//select Unit
		if (Input.GetMouseButtonDown (0)
			&& Mathf.Abs (targetX) + Mathf.Abs (targetZ) == 0
		    && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE 
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.START)
		    && CheckOtherUnits() == true
            && StandardAttack.hasAttacked == 0
            && SoldierAttack.hasAttacked == 0
            && PlayerDualSwordsAttack.hasAttacked == 0
			&& ArcherAttack.hasAttacked == 0
			&& ThiefAttack.hasAttacked == 0
			&& AmazonAttack.hasAttacked == 0
			&& LightInfantryAttack.hasAttacked == 0
			&& LeaderAttack.hasAttacked == 0
			&& thisUnitStats.tripped == 0
		    ) {
			descriptionText.text = null;
			modifiersText.text = null;
			isSelected = 1;
			Pathfinding.selectedSound.Play ();
			foreach (Unit u in notThisUnit){
				u.isSelected = 0;
				u.selectedLight.gameObject.SetActive (false);
			}
		}
		if (isSelected == 1) {
			if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
			    BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK ||
			    BattleStateMachine.currentState == BattleStateMachine.BattleStates.START) {
				selectedLight.gameObject.SetActive (true);
			}
		} //else {
			//selectedLight.gameObject.SetActive (false);
		//}

	
		//Move
		if (Input.GetMouseButtonDown (0) 
		   	&& Mathf.RoundToInt((Mathf.Abs(targetX) + Mathf.Abs (targetZ))) <= fatigue
		    && (Mathf.Abs(targetX) + Mathf.Abs (targetZ) <= moveRange/2+.5)
		    && BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE
		    && (Mathf.Abs(targetX) + Mathf.Abs (targetZ) != 0)
		//    && (target.position.x !=  enemy.position.x || target.position.z != enemy.position.z)
		    && hasMoved == 0
		    && isSelected == 1
		   	&& TileMapMouse.isOnMap == 0
		    && StandardAttack.hasAttacked == 0
		    && SoldierAttack.hasAttacked == 0
            && PlayerDualSwordsAttack.hasAttacked == 0
			&& ArcherAttack.hasAttacked == 0
			&& ThiefAttack.hasAttacked == 0
			&& ThiefAttack.mineStop == false
			&& AmazonAttack.hasAttacked == 0
			&& LightInfantryAttack.hasAttacked == 0
			&& LeaderAttack.hasAttacked == 0
			&& EventSystem.current.IsPointerOverGameObject() == false
		    ) 
		    {
			//PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
			path = pathfinding.FindPath(transform.position, target.position);
			StartCoroutine ("FollowPath");
            if (hasMoved > 0)
            {
                fatigue -= path.Length;
            }
           
			targetIndex = 0;
		}

		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.START 
			&& startAreaMap.isOnMap == 0
		    && isSelected == 1
			&& fixedStartPoint == false
			) {
			Vector3 temp = new Vector3(target.position.x, 0, target.position.z);
			transform.position = temp;
            if (Input.GetMouseButtonDown(1)){
                if (checkOtherUnitsPosition(thisUnit) == true)
                {
                    isSelected = 0;
                    selectedLight.gameObject.SetActive(false);
                }
                else 
                {
                    ErrorSound.errorSource.Play();
                }
            }
		}

	}

    private bool checkOtherUnitsPosition(Transform currentUnitTransform)
    {
        int temp = 0;
        foreach (Unit u in notThisUnit)
        {
            if (u.gameObject.activeInHierarchy == true && u.transform.position.x == currentUnitTransform.position.x
                && u.transform.position.z == currentUnitTransform.position.z)
            {
                temp += 1;
            }
        }
        if (temp != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {

		if (pathSuccessful) {
				path = newPath;

			StopCoroutine ("FollowPath");
				StartCoroutine ("FollowPath");
				
			}

	}
		
	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];

			while (true) {
				if (transform.position == currentWaypoint) {
					targetIndex ++;
					if (targetIndex >= path.Length) {
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}
				if (path.Length <= moveRange / 2 + .5) {
					isMoving = 1;
					transform.position = Vector3.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
					transform.LookAt (currentWaypoint);
					hasMoved++;		
				}
				
				yield return null;
			
			}


	}

	bool CheckOtherUnits(){
		int tempCheck = 0;
		foreach (Unit u in notThisUnit) {
			if (u.hasMoved !=0){
				tempCheck++;
			}
		}
		if (tempCheck != 0) {
			return false;
		} else {
			return true;
		}
	}

	bool CheckOtherUnitsSelected(){
		int tempCheck = 0;
		foreach (Unit u in notThisUnit) {
			if (u.isSelected !=0){
				tempCheck++;
			}
		}
		if (tempCheck != 0) {
			return false;
		} else {
			return true;
		}
	}

    bool CheckForEnemies()
    {
        int tempCheck = 0;
        foreach (EnemyUnit e in pathfinding.enemyUnits)
        {
            if (e.transform.position.x == target.transform.position.x
                && e.transform.position.z == target.transform.position.z
                && e.transform.position.y > 0)
            {
                tempCheck++;
            }
        }
        if (tempCheck > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public IEnumerator fadeDamageText(float f, Text damageText)
    {
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
        while (damageText.color.a > 0.0f)
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, damageText.color.a - Time.deltaTime / f);
            if (damageText.color.a == 0.0f)
            {
                damageText.text = " ";
                Color c = new Color(255, 0, 0, 255);
                damageText.color = c;
            }
            yield return null;
        }
    }

    public void fadeDamageTextFunction()
    {
        StartCoroutine(fadeDamageText(3.5f, damageText));
    }
}
