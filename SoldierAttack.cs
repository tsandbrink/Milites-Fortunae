using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoldierAttack : MonoBehaviour {
	public EnemyUnit[] enemies;
	public EnemyStats[] enemiesStats;
	public EnemyStats EnemyStats;
	//public Transform enemy;
	public Transform playerUnit;
	public Transform selectionCube;
	public int attackSelected = 0;
	public Unit Unit;
	public UnitStats UnitStats;
	public static int hasAttacked = 0;

    public BattleStateMachine battleStateMachine;

	public int fCost;
	public int counterFCost;
	public int shieldAttackFCost;

	public float standardAttackAcc;
	public float counterAttackAcc;
	public float shieldAttackAcc;
	
	public int standardAttackDam;
	public int counterAttackDam; 
	public int shieldAttackDam;
	
	public int standardAttackRange;
	public int counterAttackRange;
	public int shieldAttackRange;

	public Button basicAttack;
	public Button counterAttack;
	public Button shieldAttack;
	
	Color d = new Color (255, 255, 255);

	public Text standardfCostText;
	public Text counterFCostText;
	public Text shieldFCostText;
	public Text chanceToHitText;
	public Text Abilities;

	public Text attackDescription;
	public Text attackDescription2;
	public Text attackModifiers;
	public Text attackModifiers2;

	public GameObject attackRange;
	public GameObject shieldAttackStopper;
	public TileMap TileMap;

	public Battles Battles;
	public SelectionCubeColorChanger selectionCube1;

	Animator Anim;
	int isAttacking = 0;

	public Text damageText;

	public Pathfinding A_pathfinding;

	public EndTurn endTurn;

	// Use this for initialization
	void Awake () {
		standardfCostText.text = fCost.ToString ();
		counterFCostText.text = counterFCost.ToString ();
		shieldFCostText.text = shieldAttackFCost.ToString ();
		basicAttack.gameObject.SetActive (false);
		counterAttack.gameObject.SetActive (false);
		shieldAttack.gameObject.SetActive (false);
		Abilities.text = null;
		standardfCostText.gameObject.SetActive (false);
		counterFCostText.gameObject.SetActive (false);
		shieldFCostText.gameObject.SetActive (false);
		Anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking == 0 && hasAttacked == 0) {
			if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
			    BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
				EnemyStats = determineEnemyTarget ();
			}
		}

		if (Unit.isSelected == 1 && hasAttacked == 0) {
			basicAttack.gameObject.SetActive (true);
			counterAttack.gameObject.SetActive (true);
			shieldAttack.gameObject.SetActive (true);
			Abilities.text = "ABILITIES";
			standardfCostText.gameObject.SetActive (true);
			counterFCostText.gameObject.SetActive (true);
			shieldFCostText.gameObject.SetActive (true);
		} else {
			basicAttack.gameObject.SetActive (false);
			counterAttack.gameObject.SetActive (false);
			shieldAttack.gameObject.SetActive (false);
			Abilities.text = "";
			standardfCostText.gameObject.SetActive (false);
			counterFCostText.gameObject.SetActive (false);
			shieldFCostText.gameObject.SetActive (false);
		}
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
		    BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE) {
			hasAttacked = 0;
		}

		// make attacks interactable
		if (Unit.fatigue < fCost || StandardAndShieldCheck() == false || hasAttacked != 0) {
			basicAttack.interactable = false;
		}
		if (Unit.fatigue < counterFCost || hasAttacked != 0 
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.START) {
			counterAttack.interactable = false;
		}
		if (Unit.fatigue >= fCost && StandardAndShieldCheck() == true && hasAttacked == 0) {
			basicAttack.interactable = true;
		}
		if (Unit.fatigue >= counterFCost && hasAttacked == 0 && Unit.isSelected == 1
		    && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			counterAttack.interactable = true;
		}
		if (Unit.fatigue < shieldAttackFCost || StandardAndShieldCheck() == false || hasAttacked != 0) {
			shieldAttack.interactable = false;
		}
		if (Unit.fatigue >= shieldAttackFCost && StandardAndShieldCheck() == true && hasAttacked == 0) {
			shieldAttack.interactable = true;
		}
	
		// Call Attacks
		if (attackSelected == 1 
		    && CheckSelectionCube() == true
		    && Input.GetMouseButtonDown (0) 
            && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			StartCoroutine ("standardAttack");
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
			//isAttacking = 0;
        } else if (attackSelected == 1 
		           && CheckSelectionCube() == false
		           && Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (standardAttackRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		} else if (attackSelected == 2 
		           && CheckSelectionCube() == true
		           && Input.GetMouseButtonDown (0)
                    && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) {
			CounterAttackFunction ();
			attackSelected = 0;
			setAttackRange (counterAttackRange);
			attackRange.SetActive (false);
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
		} else if (attackSelected == 2 
		           && CheckSelectionCube() == false
		           && Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (counterAttackRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		} else if (attackSelected == 3
		           && CheckSelectionCube() == true
		           && Input.GetMouseButtonDown (0)
                    && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START) { 
			StartCoroutine ("shieldAttackFunction");
			shieldAttackFunction ();
			attackSelected = 0;
			setAttackRange (shieldAttackRange);
			attackRange.SetActive (false);
            if (EnemyStats.CounterAttackActivated < 3 
                && EnemyStats.health > 0)
            {
                battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
                battleStateMachine.playerUnit = Unit;
                battleStateMachine.invokeEnemyCounterAttack();
            }
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
        } else if (attackSelected == 3 
		           && CheckSelectionCube() == false
		           && Input.GetMouseButtonDown (0)) {
			attackSelected = 0;
			setAttackRange (shieldAttackRange);
			attackRange.SetActive (false);
			Unit.selectedLight.gameObject.SetActive (false);
		}

		//Display Chance To Hit
		if (attackSelected == 1
		    && CheckSelectionCube() == true) {
            if (UnitStats.chanceToHit <= 100) {
                chanceToHitText.text = UnitStats.chanceToHit.ToString() + "%";
				damageText.text = (UnitStats.attack - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 100) {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack - UnitStats.targetEnemyStats.constitution).ToString();
            }
		} else if (attackSelected == 2
		           && CheckSelectionCube() == true) {
			chanceToHitText.text = (UnitStats.chanceToHit + counterAttackAcc).ToString () + "%";
		} else if (attackSelected == 3
		           && CheckSelectionCube() == true) {
            if (UnitStats.chanceToHit <= 110) {
                chanceToHitText.text = (UnitStats.chanceToHit + shieldAttackAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + shieldAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 110) {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + shieldAttackDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
		}
		else {
			chanceToHitText.text = "";
			damageText.text = null;
		}
	}

	IEnumerator standardAttack () {
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
			isAttacking = 1;
			yield return new WaitForSeconds(.5f);
		//	AccuracyTest (standardAttackAcc);
			if (AccuracyTest (standardAttackAcc) == true) {
				DamageTest (standardAttackDam);
				EnemyStats.SetHealthBar();
			} 
			Unit.fatigue -= fCost;
			hasAttacked ++;
			Invoke("clearText", 2);
			isAttacking = 0;
		}
	}

	public void CounterAttackFunction(){
		if (Unit.fatigue >= counterFCost
		    && hasAttacked == 0
		    && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
		    || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {

		}
		UnitStats.counterAttackActivated = 1;
		Unit.fatigue -= counterFCost;
		hasAttacked++;
        Color c = new Color(0, 255, 0);
        Unit.damageText.color = c;
        Unit.damageText.text = "Set Counter";
        Unit.fadeDamageTextFunction();
        Invoke("clearText", 3);
		battleStateMachine.turnCounter += 1;
		battleStateMachine.attackLogScrollyBar.value = 0;
		battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() 
			+ ". <color=blue>" + UnitStats.unitName + " </color> sets Counter Attack.";
		battleStateMachine.attackLogScrollyBar.value = 0;
		endTurn.endTurnFunction1 ();
		endTurn.endTurnButton.interactable = false;
	}

	IEnumerator shieldAttackFunction (){
		Vector3 current = EnemyStats.transform.position; 
		if (Unit.fatigue >= shieldAttackFCost
			&& hasAttacked == 0
			&& (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
			|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE)) {
			transform.LookAt (EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Shield Attack";
            Unit.fadeDamageTextFunction();
            Anim.Play ("ShieldAttack");
			yield return new WaitForSeconds (0f);
			//AccuracyTest (shieldAttackAcc);
			if (AccuracyTest (standardAttackAcc) == true){
				DamageTest (standardAttackDam);
				EnemyStats.SetHealthBar();
				if (shieldAttackCheck() == true){ 
				 //   && Mathf.Abs(EnemyStats.gameObject.transform.position.x) < TileMap.size_x/2 - 1
				 //   && Mathf.Abs(EnemyStats.gameObject.transform.position.z) < TileMap.size_z/2 - 1){
				EnemyStats.transform.Translate (0, 0, 1, playerUnit);
					float x = EnemyStats.transform.position.x*2;
					float z = EnemyStats.transform.position.z*2;
					float xx = Mathf.RoundToInt(x);
					float zz = Mathf.RoundToInt(z);
					float xxx = xx/2;
					float zzz = zz/2;
				Vector3 newPosition = new Vector3(xxx, EnemyStats.transform.position.y, zzz);
					EnemyStats.transform.position = newPosition;
				}
			}
			Unit.fatigue -= shieldAttackFCost;
			hasAttacked ++;
			Invoke("clearText", 2);
		}
	}

	//Tests
	public bool AccuracyTest (float AttackType){
		if (UnitStats.chanceToHit > 100) {
			UnitStats.chanceToHit =100;
		}
		if (UnitStats.chanceToHit < 0) {
			UnitStats.chanceToHit = 0;
		}
		if (EnemyStats.tripped != 0) {
            Invoke("GetHit", .25f); 
            return true;
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

	public void DamageTest(int AttackType){
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

	//CleanUpFunctions
	void clearText(){
		foreach (EnemyStats EnemyStats in enemiesStats) {
			EnemyStats.enemyDamageText.color = d;
			EnemyStats.enemyDamageText.text = null;
		}
		attackDescription2.text = null;
		attackModifiers2.text = null;
	}

	void setAttackRange(int attackRange1){
		attackRange.transform.Translate (attackRange1 / 2, 0, attackRange1 / 2);
	}

	//Select Attack
	public void attackSelectedFunction(){
		attackSelected = 1;
		attackDescription2.text = "Standard low cost attack";
		attackModifiers2.text = null;
	}
	
	public void CounterAttackSelectedFunction(){
		attackSelected = 2;
		attackDescription2.text = "Perform a counter attack the next 2 times this unit gets attacked";
		attackModifiers2.text = null;
	}
	
	public void ShieldAttackSelectedFunction(){
		attackSelected = 3;
		attackDescription2.text = "Knock opponent backwards 1 sqaure";
		attackModifiers2.text = "-10 Acc";
	}

	//Checks
	bool CheckSelectionCube(){
		int i = 0;
		foreach (EnemyUnit e in enemies){
			if (selectionCube.position.x == e.transform.position.x
			    && selectionCube.position.z == e.transform.position.z
			    && Mathf.Abs(e.transform.position.x - playerUnit.position.x) 
			    + Mathf.Abs(e.transform.position.z - playerUnit.position.z) == 1){
				i++;
			}
		}
		if (i != 0) {
			return true;
		} else {
			return false;
		}
	}
	
	bool StandardAndShieldCheck(){
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

	bool shieldAttackCheck(){
		int i = 0;
		foreach (EnemyUnit e in enemies) {
			if (shieldAttackStopper.transform.position.x <= (e.transform.position.x + .1f)
				&& shieldAttackStopper.transform.position.x >= (e.transform.position.x - .1f)
				&& shieldAttackStopper.transform.position.z <= (e.transform.position.z + .1f)
				&& shieldAttackStopper.transform.position.z >= (e.transform.position.z - .1f)
				&& e.health > 0){
				i++;
			}
		}
		foreach (Unit u in Unit.notThisUnit) {
			if (shieldAttackStopper.transform.position.x <= (u.transform.position.x + .1f)
				&& shieldAttackStopper.transform.position.x >= (u.transform.position.x - .1f)
				&& shieldAttackStopper.transform.position.z <= (u.transform.position.z + .1f)
				&& shieldAttackStopper.transform.position.z >= (u.transform.position.z - .1f)
				&& u.health > 0
				&& u.gameObject.activeInHierarchy == true){
				i++;
			}
		}
		foreach (Transform t in selectionCube1.unwalkables) {
			if (shieldAttackStopper.transform.position.x <= (t.position.x + .1f)
				&& shieldAttackStopper.transform.position.x >= (t.position.x - .1f)
				&& shieldAttackStopper.transform.position.z <= (t.position.z + .1f)
				&& shieldAttackStopper.transform.position.z >= (t.position.z - .1f)){
				i++;
			}
		}
		if (EnemyStats.transform.position.x == UnitStats.transform.position.x
			&& Mathf.Abs (EnemyStats.gameObject.transform.position.z) >= (A_pathfinding.grid.gridWorldSize.y/ 2 -1)) {
			Debug.Log (EnemyStats);
			Debug.Log ("xxx");
			i++;
		}
		else if (EnemyStats.transform.position.z == UnitStats.transform.position.z
			&& Mathf.Abs(EnemyStats.gameObject.transform.position.x) >= (A_pathfinding.grid.gridWorldSize.x/2 -1)){
			i++;
			Debug.Log ("zzz");
		}
			
		if (i == 0) {
			return true;
		} else {
			return false;
		}

	}

	EnemyStats determineEnemyTarget(){
		EnemyStats target = null;
		foreach (EnemyStats e in enemiesStats) {
			if (selectionCube.position.x == e.gameObject.transform.position.x
			    && selectionCube.position.z == e.gameObject.transform.position.z){
				target = e;
				break;
			}
			else {
			target = null;
			}
		}
		return target;
	}

	public void standardDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Standard low cost attack";
			attackModifiers.text = null;
		}
	}

	public void counterDescription(){
		if (attackSelected == 0){
		attackDescription.text = "Perform a counter attack the next 2 times this unit gets attacked";
		attackModifiers.text = null;
		}
	}


	public void shieldDescription(){
		if (attackSelected == 0) {
			attackDescription.text = "Knock opponent backwards 1 sqaure";
			attackModifiers.text = "-10 Acc";
		}
	}

	public void clearDescription(){
		attackDescription.text = null;
		attackModifiers.text = null;
	}
}
