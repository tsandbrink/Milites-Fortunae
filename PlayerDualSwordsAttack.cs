using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerDualSwordsAttack : MonoBehaviour {
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
    public int whirlwindFCost;
    public int tripFCost;

    public float standardAttackAcc;
    public float whirlwindAcc;
    public float tripAcc;

    public int standardAttackDam;
    public int whirlwindDam;
    public int tripDam;

    public int standardAttackRange;
    public int whirlwindRange;
    public int tripRange;

    public Button basicAttack;
    public Button whirlwindAttack;
    public Button tripAttack;

    Color d = new Color(255, 255, 255);

    public Text standardfCostText;
    public Text whirlwindFCostText;
    public Text tripFCostText;
    public Text chanceToHitText;
    public Text Abilities;

    public Text attackDescription;
    public Text attackDescription2;
    public Text attackModifiers;
    public Text attackModifiers2;

    public GameObject attackRange;
    
	public Text damageText;

    Animator Anim;
    int isAttacking = 0;

	public EndTurn endTurn;

    // Use this for initialization
    void Awake () {
        standardfCostText.text = fCost.ToString();
        whirlwindFCostText.text = whirlwindFCost.ToString();
        tripFCostText.text = tripFCost.ToString();
        basicAttack.gameObject.SetActive(false);
        whirlwindAttack.gameObject.SetActive(false);
        tripAttack.gameObject.SetActive(false);
        Abilities.text = null;
        standardfCostText.gameObject.SetActive(false);
        whirlwindFCostText.gameObject.SetActive(false);
        tripFCostText.gameObject.SetActive(false);
        Anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if (isAttacking == 0 && hasAttacked == 0) {
			if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE ||
				BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
				EnemyStats = determineEnemyTarget ();
			}
		}

        if (Unit.isSelected == 1 && hasAttacked == 0)
        {
            basicAttack.gameObject.SetActive(true);
            whirlwindAttack.gameObject.SetActive(true);
            tripAttack.gameObject.SetActive(true);
            Abilities.text = "ABILITIES";
            standardfCostText.gameObject.SetActive(true);
            whirlwindFCostText.gameObject.SetActive(true);
            tripFCostText.gameObject.SetActive(true);
        }
        else {
            basicAttack.gameObject.SetActive(false);
            whirlwindAttack.gameObject.SetActive(false);
            tripAttack.gameObject.SetActive(false);
            Abilities.text = "";
            standardfCostText.gameObject.SetActive(false);
            whirlwindFCostText.gameObject.SetActive(false);
            tripFCostText.gameObject.SetActive(false);
        }
        if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.WIN ||
            BattleStateMachine.currentState == BattleStateMachine.BattleStates.LOSE)
        {
            hasAttacked = 0;
        }

        // make attacks interactable
        if (Unit.fatigue < fCost || StandardCheck() == false || hasAttacked != 0)
        {
            basicAttack.interactable = false;
        }
        if (Unit.fatigue < whirlwindFCost || hasAttacked != 0
            || WhirlwindCheck() == false)
        {
            whirlwindAttack.interactable = false;
        }
        if (Unit.fatigue >= fCost && StandardCheck() == true && hasAttacked == 0)
        {
            basicAttack.interactable = true;
        }
        if (Unit.fatigue >= whirlwindFCost && hasAttacked == 0 && WhirlwindCheck() == true)
        {
            whirlwindAttack.interactable = true;
        }
        if (Unit.fatigue < tripFCost || StandardCheck() == false || hasAttacked != 0)
        {
            tripAttack.interactable = false;
        }
        if (Unit.fatigue >= tripFCost && StandardCheck() == true && hasAttacked == 0)
        {
            tripAttack.interactable = true;
        }

        // Call Attacks
        if (attackSelected == 1
            && CheckSelectionCube() == true
            && Input.GetMouseButtonDown(0)
            && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START)
        {
            StartCoroutine("standardAttack");
            attackSelected = 0;
            setAttackRange(standardAttackRange);
            attackRange.SetActive(false);
            if (EnemyStats.CounterAttackActivated < 3
                && EnemyStats.health > 0)
            {
                battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
                battleStateMachine.playerUnit = Unit;
                battleStateMachine.invokeEnemyCounterAttack();
            }
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
        }
        else if (attackSelected == 1
                 && CheckSelectionCube() == false
                 && Input.GetMouseButtonDown(0))
        {
            attackSelected = 0;
            setAttackRange(standardAttackRange);
            attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
        }
        else if (attackSelected == 2
                 && CheckSelectionCube() == true
                 && Input.GetMouseButtonDown(0)
                 && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START)
        {
            StartCoroutine ("whirlwindFunction");
            attackSelected = 0;
            setAttackRange(whirlwindRange);
            attackRange.SetActive(false);
            if (EnemyStats.CounterAttackActivated < 3
                && EnemyStats.health > 0)
            {
                battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
                battleStateMachine.playerUnit = Unit;
                battleStateMachine.invokeEnemyCounterAttack();
            }
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
        }
        else if (attackSelected == 2
                 && CheckSelectionCube() == false
                 && Input.GetMouseButtonDown(0))
        {
            attackSelected = 0;
            setAttackRange(whirlwindRange);
            attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
        }
        else if (attackSelected == 3
                 && CheckSelectionCube() == true
                 && Input.GetMouseButtonDown(0)
                 && BattleStateMachine.currentState != BattleStateMachine.BattleStates.START)
        {
            StartCoroutine ("tripFunction");
            attackSelected = 0;
            setAttackRange(tripRange);
            attackRange.SetActive(false);
        //    if (EnemyStats.CounterAttackActivated < 3
		//		&& EnemyStats.health > 0 && EnemyStats.tripped == 0)
          //  {
            //    battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
              //  battleStateMachine.playerUnit = Unit;
                //battleStateMachine.invokeEnemyCounterAttack();
            //}
			endTurn.endTurnFunction1 ();
			endTurn.endTurnButton.interactable = false;
        }
        else if (attackSelected == 3
                 && CheckSelectionCube() == false
                 && Input.GetMouseButtonDown(0))
        {
            attackSelected = 0;
            setAttackRange(tripRange);
            attackRange.SetActive(false);
			Unit.selectedLight.gameObject.SetActive (false);
        }

        //DisplayChanceToHit
        if (attackSelected == 1
            && CheckSelectionCube() == true)
        {
            if (UnitStats.chanceToHit <= 100)
            {
                chanceToHitText.text = UnitStats.chanceToHit.ToString() + "%";
				damageText.text = (UnitStats.attack - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 100)
            {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack - UnitStats.targetEnemyStats.constitution).ToString();
            }
            

        }
        else if (attackSelected == 2
                 && CheckSelectionCube() == true)
        {
            if (UnitStats.chanceToHit <= 100)
            {
                chanceToHitText.text = (UnitStats.chanceToHit + whirlwindAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + whirlwindDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 100)
            {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + whirlwindDam - UnitStats.targetEnemyStats.constitution).ToString();
            }

        }
        else if (attackSelected == 3
                 && CheckSelectionCube() == true)
        {
            if (UnitStats.chanceToHit <= 105)
            {
                chanceToHitText.text = (UnitStats.chanceToHit + tripAcc).ToString() + "%";
				damageText.text = (UnitStats.attack + tripDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
            else if (UnitStats.chanceToHit > 105)
            {
                chanceToHitText.text = "100%";
				damageText.text = (UnitStats.attack + tripDam - UnitStats.targetEnemyStats.constitution).ToString();
            }
        }
        else {
            chanceToHitText.text = null;
			damageText.text = null;
        }
    }

    IEnumerator standardAttack()
    {
        if (Unit.fatigue >= fCost
            && hasAttacked == 0
            && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
            || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
        {
            transform.LookAt(EnemyStats.transform.position);
            Color c = new Color(0, 255, 0);
            Unit.damageText.color = c;
            Unit.damageText.text = "Basic Attack";
            Unit.fadeDamageTextFunction();
            Anim.Play("Attack");
            isAttacking = 1;
            yield return new WaitForSeconds(.5f);
            if (AccuracyTest(standardAttackAcc) == true)
            {
                DamageTest(standardAttackDam);
                EnemyStats.SetHealthBar();
            }
            Unit.fatigue -= fCost;
            hasAttacked++;
            Invoke("clearText", 1);
            isAttacking = 0;
        }
    }

	IEnumerator whirlwindFunction()
    {
		transform.LookAt(EnemyStats.transform.position);
        Color d = new Color(0, 255, 0);
        Unit.damageText.color = d;
        Unit.damageText.text = "Whirlwind";
        Unit.fadeDamageTextFunction();
        Anim.Play("Whirlwind");
		isAttacking = 1;
		yield return new WaitForSeconds(.5f);
        List<EnemyUnit> EnemyUnitsInRange = new List<EnemyUnit>();
        foreach (EnemyUnit e in enemies)
        {
            if (Mathf.Abs(e.transform.position.x - transform.position.x)
                + Mathf.Abs(e.transform.position.z - transform.position.z) <= 2
                && Mathf.Abs(e.transform.position.x - transform.position.x) < 2
                && Mathf.Abs(e.transform.position.z - transform.position.z) < 2
                && e.transform.position.y >= 0)
            {
                EnemyUnitsInRange.Add(e);
            }
        }
		battleStateMachine.turnCounter += 1;
		battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() 
			+ ". <color=blue>" + UnitStats.unitName + "</color> uses Whirlwind.";
        foreach (EnemyUnit e in EnemyUnitsInRange)
        {
            if (AccuracyTest(whirlwindAcc) == true)
            {
                int damageDealt = UnitStats.attack - e.thisEnemyStats.constitution + whirlwindDam;
                e.thisEnemyStats.health -= damageDealt;
                e.thisEnemyStats.SetHealthBar();
                e.thisEnemyStats.enemyDamageText.text = damageDealt.ToString();
				battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + " <color=RED>" 
					+ e.thisEnemyStats.enemyName + "</color> is hit for " + damageDealt.ToString() + " damage.";
            }
            else
            {
                Color c = new Color(0, 0, 255);
                e.thisEnemyStats.enemyDamageText.color = c;
                e.thisEnemyStats.enemyDamageText.text = "Miss!";
				battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + " <color=RED>"
				+ e.thisEnemyStats.enemyName + "</color> blocks the attack";
            }
        }
        Unit.fatigue -= whirlwindFCost;
        hasAttacked++;
        Invoke("clearText", 1);
		isAttacking = 0;
    
    }

	IEnumerator tripFunction()
    {
		if (Unit.fatigue >= tripFCost
           && hasAttacked == 0
           && (BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK
           || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE))
        {
            transform.LookAt(EnemyStats.transform.position);
            Color d = new Color(0, 255, 0);
            Unit.damageText.color = d;
            Unit.damageText.text = "Trip";
            Unit.fadeDamageTextFunction();
            Anim.Play ("Trip");
            isAttacking = 1;
			yield return new WaitForSeconds(.5f);
            if (AccuracyTest(tripAcc) == true)
            {
                DamageTest(tripDam);
                EnemyStats.SetHealthBar();
                if (EnemyStats.tripped == 0)
                {
                    if (tripTest() == true)
                    {
                        EnemyStats.tripped = 1;
						battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + " The attack knocks <color=RED>" + EnemyStats.enemyName + "</color> down.";
                    }
                }
            }
            Unit.fatigue -= tripFCost;
            hasAttacked++;
            Invoke("clearText", 1);
            isAttacking = 0;
			if (EnemyStats.CounterAttackActivated < 3
				&& EnemyStats.health > 0 && EnemyStats.tripped == 0)
			{
				battleStateMachine.thisEnemyUnit = EnemyStats.thisEnemyUnit;
				battleStateMachine.playerUnit = Unit;
				battleStateMachine.invokeEnemyCounterAttack();
			}
        }
   }

    //Tests
    public bool AccuracyTest(float AttackType)
    {
        if (UnitStats.chanceToHit > 100)
        {
            UnitStats.chanceToHit = 100;
        }
        if (UnitStats.chanceToHit < 0)
        {
            UnitStats.chanceToHit = 0;
        }
		if (EnemyStats.tripped != 0) {
            Invoke("GetHit", .25f);
            return true;
		}
        int temp = Random.Range(1, 100);
        if (temp <= UnitStats.chanceToHit + AttackType)
        {
			Invoke ("GetHit", .25f);
            return true;
        }
        else {
			EnemyStats.transform.LookAt (transform);
			Invoke ("Block", .25f);
            Color c = new Color(0, 0, 255);
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

    public void DamageTest(int AttackType)
    {
        int damageDealt = UnitStats.attack - EnemyStats.constitution + AttackType;
        EnemyStats.health -= damageDealt;
      //  EnemyStats.enemyDamageText.text = damageDealt.ToString();
        StartCoroutine(appearText(damageDealt));
        battleStateMachine.turnCounter += 1;
		battleStateMachine.attackLogScrollyBar.value = 0;
		battleStateMachine.attackLog.text = battleStateMachine.attackLog.text + "\n" + battleStateMachine.turnCounter.ToString() 
			+ ". <color=blue>" + UnitStats.unitName + " </color>hits <color=red>" 
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

    public bool tripTest()
    {
        int temp = Random.Range(1, 100);
        if (temp <= UnitStats.chanceToHit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    EnemyStats determineEnemyTarget()
    {
        EnemyStats target = null;
        foreach (EnemyStats e in enemiesStats)
        {
            if (selectionCube.position.x == e.gameObject.transform.position.x
                && selectionCube.position.z == e.gameObject.transform.position.z)
            {
                target = e;
                break;
            }
            else {
                target = null;
            }
        }
        return target;
    }

    void setAttackRange(int attackRange1)
    {
        attackRange.transform.Translate(attackRange1 / 2, 0, attackRange1 / 2);
    }

    //checks
    bool CheckSelectionCube()
    {
        int i = 0;
        foreach (EnemyUnit e in enemies)
        {
            if (selectionCube.position.x == e.transform.position.x
                && selectionCube.position.z == e.transform.position.z)
            {
				if (attackSelected == 1 || attackSelected == 3) {
					if (Mathf.Abs (e.transform.position.x - playerUnit.position.x)
						+ Mathf.Abs (e.transform.position.z - playerUnit.position.z) == 1) {
						i++;
					}
				}
                else if (attackSelected == 2 && Mathf.Abs(e.transform.position.x - playerUnit.position.x)
                         + Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= 2
                         && Mathf.Abs(e.transform.position.x - playerUnit.position.x) < 2
                         && Mathf.Abs(e.transform.position.z - playerUnit.position.z) < 2
                         && e.transform.position.y >= 0)
                {
                    i++;
                }
            }
        }
        if (i != 0)
        {
            return true;
        }
        else {
            return false;
        }
    }

    bool StandardCheck()
    {
        int i = 0;
        foreach (EnemyUnit e in enemies)
        {
            if (Mathf.Abs(e.transform.position.x - playerUnit.position.x)
                + Mathf.Abs(e.transform.position.z - playerUnit.position.z) == 1
                && e.transform.position.y >= 0)
            {
				
                i++;
            }
        }
        if (i != 0)
        {
            return true;
        }
        else {
            return false;
        }
    }

    bool WhirlwindCheck()
    {
        int i = 0;
        foreach (EnemyUnit e in enemies)
        {
            if (Mathf.Abs(e.transform.position.x - playerUnit.position.x)
                + Mathf.Abs(e.transform.position.z - playerUnit.position.z) <= 2
                && Mathf.Abs(e.transform.position.x - playerUnit.position.x) < 2
                && Mathf.Abs(e.transform.position.z - playerUnit.position.z) < 2
                && e.transform.position.y >= 0)
            {

                i++;
            }
        }
        if (i != 0)
        {
            return true;
        }
        else {
            return false;
        }
    }

    //Select Attack
    public void attackSelectedFunction()
    {
        attackSelected = 1;
        attackDescription2.text = "Standard low cost attack";
        attackModifiers2.text = null;
    }

    public void WhirlwindSelectedFunction()
    {
        attackSelected = 2;
        attackDescription2.text = "Spinning attack that hits multiple opponents";
        attackModifiers2.text = null;
    }

    public void TripSelectedFunction()
    {
        attackSelected = 3;
        attackDescription2.text = "Low-power attack aimed at opponent's legs with chance to set knock-down status";
        attackModifiers2.text = "-1 Dam";
    }

    //cleanUp
    void clearText()
    {
        foreach (EnemyStats EnemyStats in enemiesStats)
        {
            EnemyStats.enemyDamageText.color = d;
            EnemyStats.enemyDamageText.text = null;
        }
        attackDescription2.text = null;
        attackModifiers2.text = null;
    }

    public void standardDescription()
    {
		if (attackSelected == 0) {
			attackDescription.text = "Standard low cost attack";
			attackModifiers.text = null;
		}
    }

    public void whirlwindDescription()
    {
		if (attackSelected == 0) {
			attackDescription.text = "Spinning attack that hits multiple opponents";
			attackModifiers.text = null;
		}
    }

    public void tripDescription()
    {
		if (attackSelected == 0) {
			attackDescription.text = "Low-power attack aimed at opponent's legs with chance to set knock-down status";
			attackModifiers.text = "-1 Dam";
		}
    }

    public void clearDescription()
    {
        attackDescription.text = null;
        attackModifiers.text = null;
    }

}
