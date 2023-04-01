using UnityEngine;
using System.Collections;

public class EnemySoldierAttack : MonoBehaviour {

	public BattleStateMachine BattleStateMachine;
	public EnemyUnit thisEnemyUnit;
	public Unit playerUnit = null;
	UnitStats TargetUnitStats;
	public SelectionCubeColorChanger selectionCube;

	//public Battles Battles;

	int EnemyStandardFCost;

	public int ShieldAttackFCost;
	public int ShieldAttackDamage;

	public int CounterAttackFCost;
	public int CounterAttackDamage;

	public float shieldAttackAcc;

	public TileMap TileMap;
	public GameObject shieldAttackStopper;

	public bool skeleton;
	public int regenerateCost;

	Animator Anim;
	// Use this for initialization
	void Start () {
		EnemyStandardFCost = BattleStateMachine.EnemyStandardFCost;
		Anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	//void Update () {
		//thisEnemyUnit = BattleStateMachine.thisEnemyUnit;
      
            //playerUnit = BattleStateMachine.playerUnit;
            //TargetUnitStats = BattleStateMachine.TargetUnitStats;
        
		//playerUnit = BattleStateMachine.playerUnit;
		//TargetUnitStats = BattleStateMachine.TargetUnitStats;
	//}

	public int EnemySoldierChooseAttack(){
		playerUnit = BattleStateMachine.playerUnit;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
		float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
		TargetUnitStats = BattleStateMachine.TargetUnitStats;
		transform.LookAt (TargetUnitStats.transform);
		//Debug.Log(Mathf.Abs(distanceX) + Mathf.Abs(distanceZ));
		if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution 
            && thisEnemyUnit.fatigue >= EnemyStandardFCost) {
			return 1;
		}
		else if (shieldAttackCheck () == true && ShieldCheck () == true && thisEnemyUnit.fatigue >= ShieldAttackFCost) {
			return 2;
		}
        else if (thisEnemyUnit.fatigue < EnemyStandardFCost || (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ)) > 1)
        {
           // Debug.Log(Mathf.Abs(distanceX) + Mathf.Abs(distanceZ));
			if (skeleton == false) {
				return 0;
			} else {
				return 3;
			}
        }      
		else {
			return 1;
		}

	}

	bool ShieldCheck(){
		int i = 0;
		foreach (Unit u in BattleStateMachine.units) {
			if (Mathf.Abs(u.transform.position.x - thisEnemyUnit.transform.position.x) 
			    + Mathf.Abs(u.transform.position.z - thisEnemyUnit.transform.position.z) == 1
			    && u.transform.position.y >= 0){
				i++;
			}
		}
		if (i != 0){
			return true;
		} else{
			return false;
		}
	}
	
	bool shieldAttackCheck(){
		int i = 0;
		foreach (Unit u in BattleStateMachine.units) {
			if (shieldAttackStopper.transform.position.x <= (u.transform.position.x + .1f)
				&& shieldAttackStopper.transform.position.x >= (u.transform.position.x - .1f)
				&& shieldAttackStopper.transform.position.z <= (u.transform.position.z + .1f)
				&& shieldAttackStopper.transform.position.z >= (u.transform.position.z - .1f)) {
				i++;
			} 
		}
		foreach (EnemyUnit e in BattleStateMachine.enemyUnits) {
			if (shieldAttackStopper.transform.position.x <= (e.transform.position.x + .1f)
				&& shieldAttackStopper.transform.position.x >= (e.transform.position.x - .1f)
				&& shieldAttackStopper.transform.position.z <= (e.transform.position.z + .1f)
				&& shieldAttackStopper.transform.position.z >= (e.transform.position.z - .1f)
				&& e != thisEnemyUnit) {
				i++;
			} 
		}
		foreach (Transform t in selectionCube.unwalkables) {
            if (shieldAttackStopper.transform.position.x <= (t.position.x + .1f)
                && shieldAttackStopper.transform.position.x >= (t.position.x - .1f)
                && shieldAttackStopper.transform.position.z <= (t.position.z + .1f)
                && shieldAttackStopper.transform.position.z >= (t.position.z - .1f))
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
	public void shieldAttack(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Shield Attack";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
        Invoke ("shieldAttack1", 1f);
		return;
	}

	void shieldAttack1(){
	//	Vector3 current = TargetUnitStats.transform.position;
        Anim.Play("ShieldAttack");
		if (BattleStateMachine.AccuracyTest (BattleStateMachine.standardAttackAcc) == true) {
			int damageDealt = thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution + ShieldAttackDamage;
			playerUnit.thisUnitStats.health -= damageDealt;
			playerUnit.thisUnitStats.SetHealthBar ();
			playerUnit.damageText.text = damageDealt.ToString ();
			if (shieldAttackCheck () == true 
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
			//	playerUnit.transform.position = Vector3.MoveTowards(transform.position, newPosition, 3*Time.deltaTime);
			}
		}
		thisEnemyUnit.fatigue -= ShieldAttackFCost;
		foreach (Unit u in BattleStateMachine.units) {
			StartCoroutine (ClearText (u, 2.0f));
		}
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

    public void ActivateCounterAttack()
    {
        if (thisEnemyUnit.fatigue > CounterAttackFCost)
        {
            thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Counter";
            thisEnemyUnit.thisEnemyStats.CounterAttackActivated = 1;
            thisEnemyUnit.fatigue -= CounterAttackFCost;
            Invoke("clearEnemyText", 2);
        }      
        BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
    }

	public void regenerate1(){
		Invoke ("regenerate", .5f);
	}

	void regenerate(){
		
		thisEnemyUnit.fatigue += 1;
		thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Fatigue +1";
		Invoke("clearEnemyText", 1);
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}


	IEnumerator ClearText (Unit unit, float delay){
		yield return new WaitForSeconds(2.5f);
		unit.damageText.text = null;
	}

    void clearEnemyText()
    {
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = null;
              
    }
}
