using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class BoarAttack : MonoBehaviour {

	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;
	public Unit playerUnit;
	UnitStats TargetUnitStats;

	public SelectionCubeColorChanger selectionCube;

	int EnemyStandardFCost;
	public int roarFCost;

	public int goreFCost;
	public int goreAcc;

	public Animator Anim;

	public TileMap TileMap;
	public GameObject goreAttackStopper;

	public Transform[] unwalkablesAndPlayerUnwalkables;
	public List<Unit> UnitsInRange = new List<Unit> ();

    private AudioSource fxAudioSource;
    public AudioClip boarRoar;

	// Use this for initialization
	void Start () {
		EnemyStandardFCost = BattleStateMachine.EnemyStandardFCost;
        fxAudioSource = BattleStateMachine.FxAudioSource;
		//Anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame

	public int boarChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution
		    && (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1) {
			return 1;
		} else if (thisEnemyUnit.fatigue >= goreFCost && playerUnit.thisUnitStats.tripped == 0
			&& (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1) {
			return 3;
		} else if (roarCheck () == true && thisEnemyUnit.fatigue >= roarFCost ) {
			return 2;
		} else if (thisEnemyUnit.fatigue < EnemyStandardFCost || (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) > 1) {
			return 0;
		} else {
			return 1;
		}
	}

	bool roarCheck(){
		int i = 0;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) 
				+ Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) <= 4
				&& Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) < 3
				&& Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) < 3
				&& u.transform.position.y >= -1
				&& u.health > 0
				&& (u.fatigue < 5 || (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) > 1)) {
				i++;
			}
		}
		if (i >= 2) {
			return true;
		} else if (i == 1 && Mathf.Abs (playerUnit.transform.position.x - thisEnemyUnit.transform.position.x)
			+ Mathf.Abs (playerUnit.transform.position.z - thisEnemyUnit.transform.position.z) == 1
			&& playerUnit.fatigue <= 2) {
			return true;
		}
		else {
			return false;
		}
	}

	public void roar(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Roar";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("roar1", .5f);
        fxAudioSource.clip = boarRoar;
        fxAudioSource.Play();
		return;
	}

	void roar1(){
		Anim.Play ("Roar");
		//List<Unit> UnitsInRange = new List<Unit> ();
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) 
				+ Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) <= 4
				&& Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) < 3
				&& Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) < 3
				&& u.transform.position.y >= 0
				&& u.health >0) {
				UnitsInRange.Add(u);
			}
		}
		Debug.Log (UnitsInRange.Count);
		foreach (Unit u in UnitsInRange) {
			Debug.Log ("UinRange");
			Vector3 newPosition = new Vector3 (u.transform.position.x, u.transform.position.y, u.transform.position.z-1);//0
			Vector3 newPosition1 = new Vector3 (u.transform.position.x, u.transform.position.y, u.transform.position.z + 1);//180
			Vector3 newPosition2 = new Vector3 (u.transform.position.x - 1, u.transform.position.y, u.transform.position.z);//90
			Vector3 newPosition3 = new Vector3 (u.transform.position.x + 1, u.transform.position.y, u.transform.position.z);//270

			//do based on position not rotation
			if (u.transform.position.z > thisEnemyUnit.transform.position.z) {
				if (roarBackCheck (newPosition1) == true) {
					u.transform.position = newPosition1;
				} else {
					if (u.transform.position.x < thisEnemyUnit.transform.position.x) {
						if (roarBackCheck (newPosition2) == true) {
							u.transform.position = newPosition2;
						}
					} else if (u.transform.position.x > thisEnemyUnit.transform.position.x) {
						if (roarBackCheck (newPosition3) == true) {
							u.transform.position = newPosition3;
						}
					}
				}
			} else if (u.transform.position.z < thisEnemyUnit.transform.position.z) {
				if (roarBackCheck (newPosition) == true) {
					u.transform.position = newPosition;
				} else {
					if (u.transform.position.x < thisEnemyUnit.transform.position.x) {
						if (roarBackCheck (newPosition2) == true) {
							u.transform.position = newPosition2;
						}
					} else if (u.transform.position.x > thisEnemyUnit.transform.position.x) {
						if (roarBackCheck (newPosition3) == true) {
							u.transform.position = newPosition3;
						}
					}
				}
			} else if (u.transform.position.z == thisEnemyUnit.transform.position.z
			           && u.transform.position.x < thisEnemyUnit.transform.position.x) {
				if (roarBackCheck (newPosition2) == true) {
					u.transform.position = newPosition2;
				}
			} else if (u.transform.position.z == thisEnemyUnit.transform.position.z
			           && u.transform.position.x > thisEnemyUnit.transform.position.x) {
				if (roarBackCheck (newPosition3) == true) {
					u.transform.position = newPosition3;
				}
			}


			//if (u.transform.rotation.eulerAngles.y >= -1 && u.transform.rotation.eulerAngles.y <= 1) {
			//	if (roarBackCheck (newPosition) == true) {
			//		u.transform.position = newPosition;
			//	}
			//} else if (u.transform.rotation.eulerAngles.y >= 179 && u.transform.rotation.eulerAngles.y <= 181) {
			//	Debug.Log ("madeIt");
			//	if (roarBackCheck (newPosition1) == true) {
			//		u.transform.position = newPosition1;
			//	}
			//} else if (u.transform.rotation.eulerAngles.y >= 89 && u.transform.rotation.eulerAngles.y <= 91) {
			//	if (roarBackCheck (newPosition2) == true) {
			//		u.transform.position = newPosition2;
			//	}
			//} else if (u.transform.rotation.eulerAngles.y >= 269 && u.transform.rotation.eulerAngles.y <= 271){
			//	if (roarBackCheck (newPosition3) == true) {
			//		u.transform.position = newPosition3;
			//	}
			//}
		}
		BattleStateMachine.turnCounter += 1;
		BattleStateMachine.attackLog.text = BattleStateMachine.attackLog.text + "\n" + BattleStateMachine.turnCounter.ToString() 
			+ ".  <color=red>BOAR</color> uses Roar.";
		thisEnemyUnit.fatigue -= roarFCost;
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

	bool roarBackCheck(Vector3 stopper){
		int i = 0;
		Debug.Log (stopper);
		foreach (Transform t in unwalkablesAndPlayerUnwalkables) {
			if (t.position.x == stopper.x && t.position.z == stopper.z) {
				Debug.Log ("a");
				i++;
			}
		}
		if (Mathf.Abs (stopper.x) > TileMap.size_x / 2) {
			Debug.Log ("b");
			i++;
		}
		if (Mathf.Abs (stopper.z) > TileMap.size_z / 2) {
			Debug.Log ("c");
			i++;
		}
		Debug.Log (i);
		if (i == 0) {
			return true;
		} else {
			return false;
		}
	}

	public void goreAttack(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Gore";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("goreAttack1", .5f);
		return;
	}

	void goreAttack1(){
		Anim.Play("GoreAttack");
		if (BattleStateMachine.AccuracyTest (BattleStateMachine.standardAttackAcc) == true) {
			int damageDealt = thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution;
			playerUnit.thisUnitStats.health -= damageDealt;
			playerUnit.thisUnitStats.SetHealthBar ();
			playerUnit.damageText.text = damageDealt.ToString ();
			BattleStateMachine.turnCounter += 1;
			BattleStateMachine.attackLogScrollyBar.value = 0;
			BattleStateMachine.attackLog.text = BattleStateMachine.attackLog.text + "\n" 
				+ BattleStateMachine.turnCounter.ToString() + ". <color=red>" + thisEnemyUnit.thisEnemyStats.enemyName 
				+ " </color>hits <color=blue>" + TargetUnitStats.unitName + " </color>for " + damageDealt.ToString () + " damage.";
			BattleStateMachine.attackLogScrollyBar.value = 0;
			if (goreAttackCheck () == true 
				&& Mathf.Abs (playerUnit.transform.position.x) < TileMap.size_x / 2 - 1
				&& Mathf.Abs (playerUnit.transform.position.z) < TileMap.size_z / 2 - 1) {
				playerUnit.transform.Translate (0, 0, 1, thisEnemyUnit.transform);
				float x = TargetUnitStats.transform.position.x * 2;
				float z = TargetUnitStats.transform.position.z * 2;
				float xx = Mathf.RoundToInt (x);
				float zz = Mathf.RoundToInt (z);
				float xxx = xx / 2;
				float zzz = zz / 2;
				Vector3 newPosition = new Vector3 (xxx, TargetUnitStats.transform.position.y, zzz);
				playerUnit.transform.position = newPosition;
				if (tripTest () == true) {
					playerUnit.thisUnitStats.tripped = 1;
				}
			}
		}
		thisEnemyUnit.fatigue -= goreFCost;
		foreach (Unit u in BattleStateMachine.units) {
			StartCoroutine (ClearText (u, 1));
		}
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;

	}

	bool goreAttackCheck(){
		int i = 0;
		foreach (Unit u in BattleStateMachine.units) {
			if (goreAttackStopper.transform.position.x <= (u.transform.position.x + .1f)
				&& goreAttackStopper.transform.position.x >= (u.transform.position.x - .1f)
				&& goreAttackStopper.transform.position.z <= (u.transform.position.z + .1f)
				&& goreAttackStopper.transform.position.z >= (u.transform.position.z - .1f)) {
				i++;
			} 
		}
		foreach (EnemyUnit e in BattleStateMachine.enemyUnits) {
			if (goreAttackStopper.transform.position.x <= (e.transform.position.x + .1f)
				&& goreAttackStopper.transform.position.x >= (e.transform.position.x - .1f)
				&& goreAttackStopper.transform.position.z <= (e.transform.position.z + .1f)
				&& goreAttackStopper.transform.position.z >= (e.transform.position.z - .1f)
				&& e != thisEnemyUnit) {
				i++;
			} 
		}
		foreach (Transform t in selectionCube.unwalkables) {
			if (goreAttackStopper.transform.position.x <= (t.position.x + .1f)
				&& goreAttackStopper.transform.position.x >= (t.position.x - .1f)
				&& goreAttackStopper.transform.position.z <= (t.position.z + .1f)
				&& goreAttackStopper.transform.position.z >= (t.position.z - .1f))
			{
				i++;
			}
		}

		if (i == 0) {
			return true;
		} else {
			return false;
		}
	}

	bool tripTest(){
		int temp = Random.Range(1, 100);
		if (temp <= EnemyStats.chanceToHit)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	IEnumerator ClearText (Unit unit, float delay){
		yield return new WaitForSeconds(delay);
		unit.damageText.text = null;
	}
}
