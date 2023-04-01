using UnityEngine;
using System.Collections;

public class EnemyArcherAttack : MonoBehaviour {

	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;
	public Unit playerUnit;
	UnitStats TargetUnitStats;

	public int standardShotRange;
	public int standardShotFCost;

	public int poisonShotRange;
	public int poisonShotFCost;
	public int poisonShotFatigueDamage;
	public int poisonShotDam;
	public int poisonShotAcc;

	public int cripplingShotRange;
	public int cripplingShotFCost;
	public int cripplingShotDefenseDamage;
	public int cripplingShotConstitutionDamage;
	public int cripplingShotDam;
	public int cripplingShotAcc;
	public int cripplingShotAccuracyDamage;

	public int playerInSight = 0;
	public int standingInSight = 0;

	public float standardAttackAcc;

	Animator Anim;

	// Use this for initialization
	void Start () {
		Anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	//void Update () {
	//	thisEnemyUnit = BattleStateMachine.thisEnemyUnit;
	//	playerUnit = BattleStateMachine.playerUnit;
	//	TargetUnitStats = BattleStateMachine.TargetUnitStats;
	//}

	public int archerChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		playerUnit.colliderBox.SetActive (false);
		Vector3 temp1 = new Vector3 (transform.position.x, transform.position.y + 1f, transform.position.z);
		Vector3 temp2 = new Vector3 (playerUnit.transform.position.x, playerUnit.transform.position.y + 1f, playerUnit.transform.position.z);
		transform.LookAt (TargetUnitStats.transform);
		int x = Random.Range (1, 100);
		if ((Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) <= cripplingShotRange && thisEnemyUnit.fatigue >= cripplingShotFCost
			&& Physics.Linecast (temp1, temp2) == false && x < 50 
			&& playerUnit.health > thisEnemyUnit.thisEnemyStats.attack - playerUnit.thisUnitStats.constitution) {
			playerUnit.colliderBox.SetActive (true);
			return 3;
		}
		else if ((Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) <= poisonShotRange && thisEnemyUnit.fatigue >= poisonShotFCost
		    && Physics.Linecast (temp1, temp2) == false) {
			playerUnit.colliderBox.SetActive (true);
			return 2;
		} else if ((Mathf.Abs (distanceX) + Mathf.Abs (distanceZ)) <= standardShotRange && thisEnemyUnit.fatigue >= standardShotFCost
			&& Physics.Linecast(temp1, temp2) == false) {
			playerUnit.colliderBox.SetActive (true);
			return 1;
		} else {
			playerUnit.colliderBox.SetActive (true);
			return 0;
		}
	}

	public void poisonShot(){
		Anim.Play ("Attack");
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Poison Shot";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("poisonShot1", .33f);
		return;
	}

	void poisonShot1(){
	//	Anim.Play ("Attack");
		StartCoroutine ("followCam");


		if (BattleStateMachine.AccuracyTest (poisonShotAcc) == true) {
			BattleStateMachine.DamageTest (poisonShotDam, thisEnemyUnit.thisEnemyStats);
			playerUnit.thisUnitStats.SetHealthBar ();
			playerUnit.fatigue -= poisonShotFatigueDamage;
			playerUnit.fatigueDamageText.text = "Stamina -" + poisonShotFatigueDamage.ToString ();
		}
		thisEnemyUnit.fatigue -= poisonShotFCost;
		StartCoroutine (ClearText (playerUnit, 2));
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
		Invoke ("stopFollowCam", 1.5f);
	}

	public void cripplingShot(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Crippling Shot";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Anim.Play ("Attack");
		Invoke ("cripplingShot1", .33f);
		return;
	}

	void cripplingShot1(){
	//	Anim.Play ("Attack");
		StartCoroutine ("followCam");
		if (BattleStateMachine.AccuracyTest (cripplingShotAcc) == true) {
			BattleStateMachine.DamageTest (cripplingShotDam, thisEnemyUnit.thisEnemyStats);
			playerUnit.thisUnitStats.SetHealthBar ();
			playerUnit.thisUnitStats.constitution -= cripplingShotConstitutionDamage;
			playerUnit.thisUnitStats.accuracy -= cripplingShotAccuracyDamage;
			playerUnit.thisUnitStats.armor.defense -= cripplingShotDefenseDamage;
			playerUnit.thisUnitStats.defense -= cripplingShotDefenseDamage;
			playerUnit.fatigueDamageText.text = "Def -" + cripplingShotDefenseDamage.ToString () 
				+ "  Con -" + cripplingShotConstitutionDamage.ToString () + " Acc -" +cripplingShotAccuracyDamage.ToString();
		}
		thisEnemyUnit.fatigue -= cripplingShotFCost;
		StartCoroutine (ClearText (playerUnit, 2));
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
		Invoke ("stopFollowCam", 1.5f);
	}

	void stopFollowCam(){
		StopCoroutine ("followCam");
	}

	IEnumerator followCam(){
		Camera mainCamera = BattleStateMachine.mainCamera;
		Vector3 newCamPos = new Vector3(playerUnit.transform.position.x, mainCamera.transform.position.y, 
			playerUnit.transform.position.z + (mainCamera.transform.position.y 
				* (Mathf.Tan (BattleStateMachine.mainCameraRotX)))*(BattleStateMachine.mainCameraFollowY));
		//mainCamera.transform.position = newCamPos;
		while (true) {
			if (transform.position == newCamPos) {
                yield return new WaitForSeconds(1);
                yield break;
			} else {
				mainCamera.transform.position = Vector3.MoveTowards (mainCamera.transform.position, newCamPos, 8 * Time.deltaTime);
			}
			yield return null;
		}
	}

	IEnumerator ClearText (Unit unit, float delay){
		yield return new WaitForSeconds(2.5f);
		unit.damageText.text = null;
		unit.fatigueDamageText.text = null;
	}
}
