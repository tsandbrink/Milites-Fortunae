using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour {

	public Text nameText;
	public Text healthText;
	public Text healthNumberText;
    public Text statusText;
	public Transform selectionCube;
	//public Transform thisUnit;
	//public Transform deathCube;
	public Text enemyDamageText;
	public GameObject healthBar;
	Vector3 death = new Vector3 (-50f, -20f, 0);
	public BattleStateMachine BattleStateMachine;

	public RawImage enemyImage;
	public Texture thisEnemyTexture;

    public EnemyUnit thisEnemyUnit;

	public float maxHealth;
	public string enemyName;
	public float health;
	public Weapon weapon;

	public static int strength = 3;
	public int attack;
	public float defense;
	public float accuracy = 6;
	public int constitution = 4;
	
	public EnemyArmor EnemyArmor;
	public static float chanceToHit;

    public Animator Anim;

    public int tripped = 0;

    public int CounterAttackActivated;

   

	// Use this for initialization
	void Start () {
		attack = strength + weapon.attack;
        CounterAttackActivated = 3;
        statusText.text = null;
		health = maxHealth;
		defense = 4 + EnemyArmor.defense;
		chanceToHit = accuracy / BattleStateMachine.Units[0].defense * 100;
        Anim = GetComponent<Animator>();
		enemyImage.texture = null;
       // enemyDamageText.color = new Color(1, 0, 0, 1);
        enemyDamageText.fontSize = 3;
	}
	
	// Update is called once per frame
	void Update () {
		//chanceToHit = accuracy / BattleStateMachine.playerUnit.thisUnitStats.defense * 100;
		if (health < 0) {
			health = 0;
		}
		if (selectionCube.position.x == transform.position.x && selectionCube.position.z == transform.position.z) {
			nameText.text = enemyName.ToString ();
			healthText.text = "HEALTH:";
			healthNumberText.text = health.ToString () + "/" + maxHealth.ToString ();
			enemyImage.texture = thisEnemyTexture;
		}
        //else {
        //	nameText.text = " ";
        //	healthText.text = " ";
        //	healthNumberText.text = " ";
        //	}
        //SetHealthBar ();
        if (tripped == 1)
        {
            Debug.Log("Rotate");
            transform.rotation = Quaternion.Euler(75f, 0, 0);
            defense = 1;
            tripped = 2;
        }
        if (tripped == 4)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            defense = 4 + EnemyArmor.defense;
            statusText.text = null; 
            tripped = 0;
        }

		if (health == 0) {
            Anim.Play("Die");
			transform.position = Vector3.MoveTowards(transform.position, death, Time.deltaTime*100);

		}
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYATTACK) {
			chanceToHit = findChanceToHit();
		}

        if (enemyDamageText.color.a == 0.0f)
        {
            enemyDamageText.text = "";
            enemyDamageText.color = new Color(enemyDamageText.color.r, enemyDamageText.color.g, enemyDamageText.color.b, 1);
        }
	}

	public void SetHealthBar(){
		healthBar.transform.localScale = new Vector3 (health/maxHealth, healthBar.transform.localScale.y, 
		                                             healthBar.transform.localScale.z);
	}

	public float findChanceToHit(){
		float f = accuracy / BattleStateMachine.playerUnit.thisUnitStats.defense * 100;
		return f;
	}

   
    public IEnumerator fadeDamageText(float f, Text damageText)
    {
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1);
        while (damageText.color.a > 0.0f)
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, damageText.color.a - Time.deltaTime / f);
            yield return null;
        }
    }

    public void fadeDamageTextFunction()
    {
        StartCoroutine(fadeDamageText(4.5f, enemyDamageText));
    }
}