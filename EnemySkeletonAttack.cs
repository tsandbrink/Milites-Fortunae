using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySkeletonAttack : MonoBehaviour {

	Animator Anim;
	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;
	public Unit playerUnit;
	UnitStats TargetUnitStats;
	public EnemyTarget EnemyAI;

	int EnemyStandardFCost;
	public int whirlWindFCost;
	public int WhirlwindDamage;

	public int ChargeFCost;
	public int chargeDamage;
	public int chargeAcc;
	public int chargeRange;

	public int regenerateCost;


	public int inCharge = 0;
	public TurnOffWalkables turnOffWalkables;


	// Use this for initialization
	void Start () {
		EnemyStandardFCost = BattleStateMachine.EnemyStandardFCost;
		Anim = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	public int skeletonChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;

		if (thisEnemyUnit.fatigue >= regenerateCost
		    && thisEnemyUnit.health <= (thisEnemyUnit.thisEnemyStats.maxHealth / 2)) {
			return 4;
		} else if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution
		           && (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1
		           && WhirlwindAttackCheck () == false) {
			return 1;
		} else if (WhirlwindAttackCheck () == true && thisEnemyUnit.fatigue >= whirlWindFCost) {
			return 2;		
		} else if (chargeCheck (playerUnit, thisEnemyUnit) == true && thisEnemyUnit.fatigue >= ChargeFCost) {
			return 3;
		} else if (thisEnemyUnit.health < thisEnemyUnit.thisEnemyStats.maxHealth) {
			return 4;
		} else {
			return 0;
		}
	}

	bool WhirlwindAttackCheck(){
		int i = 0;
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) 
				+ Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) <= 4
				&& Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) < 3
				&& Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) < 3
				&& u.transform.position.y >= -1
				&& u.health > 0) {
				i++;
			}

		}
		Debug.Log (i);
		if (i >= 2) {
			return true;
		} else if (i == 1 && Mathf.Abs (playerUnit.transform.position.x - thisEnemyUnit.transform.position.x)
			+ Mathf.Abs (playerUnit.transform.position.z - thisEnemyUnit.transform.position.z) == 2
			&& Mathf.Abs (playerUnit.transform.position.x - thisEnemyUnit.transform.position.x) < 2
			&& Mathf.Abs (playerUnit.transform.position.z - thisEnemyUnit.transform.position.z) < 2
			&& playerUnit.transform.position.y >= -1) {
			return true;
		}
		else {
			return false;
		}
	}

	public void WhirlwindAttack(){
		Invoke ("WhirlwindAttack1", 1f);
		return;
	}

	void WhirlwindAttack1(){
		Anim.Play ("Whirlwind");
		List<Unit> UnitsInRange = new List<Unit> ();
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) 
				+ Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) <= 4
				&& Mathf.Abs (u.transform.position.x - thisEnemyUnit.transform.position.x) < 3
				&& Mathf.Abs (u.transform.position.z - thisEnemyUnit.transform.position.z) < 3
				&& u.transform.position.y >= 0
				&& u.health >= 0) {
				UnitsInRange.Add(u);
			}
		}
		foreach (Unit u in UnitsInRange) {
			if (BattleStateMachine.AccuracyTest (BattleStateMachine.standardAttackAcc) == true) {
				int damageDealt = thisEnemyUnit.thisEnemyStats.attack - u.thisUnitStats.constitution + WhirlwindDamage;
				u.thisUnitStats.health -= damageDealt;
				u.thisUnitStats.SetHealthBar();
				u.damageText.text = damageDealt.ToString();
			} 
			else
			{
				Color c = new Color(0, 0, 255);
				u.damageText.color = c;
				u.damageText.text = "Miss!";
			}
		}
		thisEnemyUnit.fatigue -= whirlWindFCost;
		foreach (Unit u in UnitsInRange) {
			StartCoroutine(ClearText(u, 1));
		}
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

	public bool chargeCheck(Unit u, EnemyUnit enemyUnit){
		int i = 0;
		Vector3 temp = new Vector3 (enemyUnit.transform.position.x, enemyUnit.transform.position.y +.1f, enemyUnit.transform.position.z);
		enemyUnit.colliderBox.SetActive (false);
		turnOffWalkables.TurnOffUnwalkables ();
		turnOffWalkables.TurnOffPlayerUnwalkables ();
		u.colliderBox.SetActive (false);
		Vector3 temp2 = new Vector3 (u.transform.position.x, u.transform.position.y +.1f, u.transform.position.z);
		float distanceX = Mathf.Abs (enemyUnit.transform.position.x - u.transform.position.x);
		float distanceZ = Mathf.Abs (enemyUnit.transform.position.z - u.transform.position.z);
		if (distanceX == 0 && distanceZ > 1 && distanceZ <= enemyUnit.enemyLightInfantryAttack.chargeRange
			&& Physics.Linecast (temp, temp2) == false
			&& u.health > 0) {
			i++;
			u.colliderBox.SetActive (true);
		} else if (distanceZ == 0 && distanceX > 1 && distanceX <= enemyUnit.enemyLightInfantryAttack.chargeRange
			&& Physics.Linecast (temp, temp2) == false
			&& u.health > 0) {
			i++;
			u.colliderBox.SetActive (true);
		}
		u.colliderBox.SetActive (true);
		turnOffWalkables.TurnOnUnwalkables ();
		turnOffWalkables.TurnOnPlayerUnwalkables ();
		enemyUnit.colliderBox.SetActive (true);
		Debug.Log (i);
		if (i > 0) {
			enemyUnit.enemyLightInfantryAttack.inCharge = 1;
			return true;
		} else {
			return false;
		}

	}

	public void chargeAttack1(){
		Invoke ("chargeAttack", .5f);
	}

	void chargeAttack(){
		transform.LookAt (playerUnit.transform);
		Anim.Play ("Charge");
		//thisEnemyUnit.fatigue -= ChargeFCost;
		float distanceZ = Mathf.Abs(thisEnemyUnit.transform.position.z - playerUnit.transform.position.z);
		float distanceX = Mathf.Abs (thisEnemyUnit.transform.position.x - playerUnit.transform.position.x);
		if (transform.rotation.y == 0 || distanceX == 0) {
			Debug.Log ("a");
			transform.Translate (0, 0, distanceZ - 1, transform);
			float x = transform.position.x * 2;
			float z = transform.position.z * 2;
			float xx = Mathf.RoundToInt (x);
			float zz = Mathf.RoundToInt (z);
			float xxx = xx / 2;
			float zzz = zz / 2;
			Vector3 newPosition = new Vector3 (xxx, transform.position.y, zzz);
			transform.position = newPosition;
			//coroutine = moveTowards(newPosition);
			//	StartCoroutine (coroutine);
		} else if (transform.rotation.y == 180 || distanceX == 0) {
			Debug.Log ("b");
			transform.Translate (0, 0, distanceZ - 1, transform);
			float x = transform.position.x * 2;
			float z = transform.position.z * 2;
			float xx = Mathf.RoundToInt (x);
			float zz = Mathf.RoundToInt (z);
			float xxx = xx / 2;
			float zzz = zz / 2;
			Vector3 newPosition = new Vector3 (xxx, transform.position.y, zzz);
			transform.position = newPosition;
			//coroutine = moveTowards(newPosition);
			//	StartCoroutine (coroutine);
		}
		else {//if (transform.rotation.y == 90 || transform.rotation.y == 270) {
			Debug.Log("c");
			transform.Translate (0, 0, distanceX - 1, transform);
			float x = transform.position.x * 2;
			float z = transform.position.z * 2;
			float xx = Mathf.RoundToInt (x);
			float zz = Mathf.RoundToInt (z);
			float xxx = xx / 2;
			float zzz = zz / 2;
			Vector3 newPosition = new Vector3 (xxx, transform.position.y, zzz);
			transform.position = newPosition;
			//coroutine = moveTowards(newPosition);
			//StartCoroutine (coroutine);
		}
		//Camera mainCamera = BattleStateMachine.mainCamera;
		//Vector3 newCamPos = new Vector3(playerUnit.transform.position.x, mainCamera.transform.position.y, 
		//	playerUnit.transform.position.z + (mainCamera.transform.position.y 
		//		* (Mathf.Tan (BattleStateMachine.mainCameraRotX)))*(BattleStateMachine.mainCameraFollowY));
		//mainCamera.transform.position = newCamPos;
		//mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, newCamPos, 10*Time.deltaTime);
		StartCoroutine ("followCam");
		if (BattleStateMachine.AccuracyTest (chargeAcc) == true) {
			BattleStateMachine.DamageTest (chargeDamage, thisEnemyUnit.thisEnemyStats);
			playerUnit.thisUnitStats.SetHealthBar ();
			if (playerUnit.thisUnitStats.tripped == 0) {
				if (tripTest () == true) {
					playerUnit.thisUnitStats.tripped = 1;
					BattleStateMachine.attackLog.text = BattleStateMachine.attackLog.text + " <color=blue>"
						+ playerUnit.thisUnitStats.unitName + "</color> is knocked over by the attack.";
				}
			}
		}
		thisEnemyUnit.fatigue -= ChargeFCost;
		StartCoroutine (ClearText (playerUnit, 2));
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
		Invoke ("stopFollowCam", 1.5f);
	}

	IEnumerator moveTowards(Vector3 target){
		while (true) {
			if (transform.position == target) {
				yield break;
			} else {
				transform.position = Vector3.MoveTowards (transform.position, target, 1 * Time.deltaTime);
			}
			yield return null;
		}
	}

	void stopFollowCam(){
		StopCoroutine ("followCam");
	}

	public IEnumerator followCam(){
		Camera mainCamera = BattleStateMachine.mainCamera;
		Vector3 newCamPos = new Vector3(playerUnit.transform.position.x, mainCamera.transform.position.y, 
			playerUnit.transform.position.z + (mainCamera.transform.position.y 
				* (Mathf.Tan (BattleStateMachine.mainCameraRotX)))*(BattleStateMachine.mainCameraFollowY));
		//mainCamera.transform.position = newCamPos;
		while (true) {
			if (transform.position == newCamPos) {
				yield break;
			} else {
				mainCamera.transform.position = Vector3.MoveTowards (mainCamera.transform.position, newCamPos, 7 * Time.deltaTime);
			}
			yield return null;
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

	public void regenerate1(){
		Invoke ("regenerate", .5f);
	}

	void regenerate(){
		thisEnemyUnit.fatigue -= regenerateCost;
		thisEnemyUnit.thisEnemyStats.health += regenerateCost;
		thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Health +" + regenerateCost.ToString();
		Invoke("clearEnemyText", 1);
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

	IEnumerator ClearText (Unit unit, float delay){
		yield return new WaitForSeconds(delay);
		unit.damageText.text = null;
	}

	void clearEnemyText()
	{
		thisEnemyUnit.thisEnemyStats.enemyDamageText.text = null;
	}
}
