using UnityEngine;
using System.Collections;

public class EnemyLightInfantryAttack : MonoBehaviour {

	Animator Anim;
	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;
	public Unit playerUnit;
	UnitStats TargetUnitStats;
	public EnemyTarget EnemyAI;

	public bool skeleton;

	int EnemyStandardFCost;

	public int breakDefenseFCost;
	public int breakDefenseDamage;
	public int breakDefenseAcc;
	public int breakDefenseDefenseDamage;

	public int ChargeFCost;
	public int chargeDamage;
	public int chargeAcc;
	public int chargeRange;

	public int regenerateCost;

	public int inCharge = 0;
	public TurnOffWalkables turnOffWalkables;

	Camera mainCamera;

	IEnumerator coroutine;
	// Use this for initialization
	void Start () {
		EnemyStandardFCost = BattleStateMachine.EnemyStandardFCost;
		Anim = GetComponent<Animator> ();
		mainCamera = BattleStateMachine.mainCamera;
	}
	
	// Update is called once per frame
	public int lightChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution
		    && (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1
		    && thisEnemyUnit.fatigue >= EnemyStandardFCost) {
			return 1;
		} else if (thisEnemyUnit.fatigue >= ChargeFCost && inCharge == 1 && inChargeChargeCheck(playerUnit, thisEnemyUnit) == true) {
            Debug.Log("inCharge");
            return 2;
		} else if (chargeCheck (playerUnit, thisEnemyUnit) == true && finalChargeCheck(playerUnit, thisEnemyUnit) == true && thisEnemyUnit.fatigue >= ChargeFCost) {
            Debug.Log("charge");
            return 2;
		} else if (thisEnemyUnit.fatigue >= breakDefenseFCost
		           && (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) == 1
		           && TargetUnitStats.defense >= 7
		           && skeleton == false) {
			return 3;
		} else if (thisEnemyUnit.fatigue >= regenerateCost
		           && skeleton == true
		           && thisEnemyUnit.health <= (thisEnemyUnit.thisEnemyStats.maxHealth / 2)) {
			return 4;
		} else if  (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ) == 1 
			&& thisEnemyUnit.fatigue >= EnemyStandardFCost) {
			return 1;
		} else {
			return 0;
		}
	}

    public bool inChargeChargeCheck(Unit u, EnemyUnit enemyUnit)
    {
        int i = 0;
        float distanceX = Mathf.Abs(enemyUnit.transform.position.x - u.transform.position.x);
        float distanceZ = Mathf.Abs(enemyUnit.transform.position.z - u.transform.position.z);
        if (distanceX == 0 && distanceZ > 1 && distanceZ <= enemyUnit.enemyLightInfantryAttack.chargeRange
            && u.health > 0)
        {
            i++;
            u.colliderBox.SetActive(true);
        }
        else if (distanceZ == 0 && distanceX > 1 && distanceX <= enemyUnit.enemyLightInfantryAttack.chargeRange
            && u.health > 0)
        {
            i++;
           
        }
        if (i > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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
	//		enemyUnit.enemyLightInfantryAttack.inCharge = 1;
			return true;
		} else {
			return false;
		}

	}

    public bool finalChargeCheck(Unit u, EnemyUnit e) {
        float distanceX = Mathf.Abs (e.transform.position.x - u.transform.position.x);
		float distanceZ = Mathf.Abs (e.transform.position.z - u.transform.position.z);
        int i = 0;
        foreach (Unit notTargetUnit in BattleStateMachine.units){
            float distanceXX = Mathf.Abs (e.transform.position.x - notTargetUnit.transform.position.x);
            float distanceZZ = Mathf.Abs (e.transform.position.z - notTargetUnit.transform.position.z);
            if (notTargetUnit.gameObject.activeInHierarchy == true && u.thisUnitStats.unitName != notTargetUnit.thisUnitStats.unitName
                && notTargetUnit.transform.position.x == e.transform.position.x && distanceZZ < distanceZ){
                i++;
            }
            else if (notTargetUnit.gameObject.activeInHierarchy == true && u.thisUnitStats.unitName != notTargetUnit.thisUnitStats.unitName
                && notTargetUnit.transform.position.z == e.transform.position.z && distanceXX < distanceX){
                i++;
            }
        }
        if (i > 0){
            return false;
        }
        else  {
            return true;
        }
    }

	public void breakDefense1(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Break Defense";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("breakDefenseAttack", .6f);
	}

	void breakDefenseAttack(){
		Anim.Play ("BreakAttack");
		if (BattleStateMachine.AccuracyTest (breakDefenseAcc) == true) {
			BattleStateMachine.DamageTest (breakDefenseDamage, thisEnemyUnit.thisEnemyStats);
			playerUnit.thisUnitStats.SetHealthBar ();
			playerUnit.thisUnitStats.defense -= breakDefenseDefenseDamage;
			playerUnit.thisUnitStats.armor.defense -= breakDefenseDefenseDamage;
			playerUnit.fatigueDamageText.text = "Def -" +breakDefenseDefenseDamage.ToString ();
		}
		BattleStateMachine.attackLog.text = BattleStateMachine.attackLog.text + " <color=blue>"
			+ playerUnit.thisUnitStats.unitName + "</color> 's defense is reduced by 1.";
		thisEnemyUnit.fatigue -= breakDefenseFCost;
		StartCoroutine (ClearText (playerUnit, 2));
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

	public void chargeAttack1(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Charge Attack";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("chargeAttack", .6f);
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
		yield return new WaitForSeconds(2.5f);
		unit.damageText.text = null;
		unit.fatigueDamageText.text = null;
	}

	void clearEnemyText()
	{
		thisEnemyUnit.thisEnemyStats.enemyDamageText.text = null;
	}
}
