using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour {

	public Unit thisUnit;
	public Transform selectionCube;
	public Transform deathCube;
	public string Type;
	public string unitName;
	public RawImage unitImage;
	public Texture thisUnitTexture;
	public Text nameText;
	public Text healthText;
	public Text healthNumberText;

	public Unit[] notThisUnit;
	public GameObject healthBar;
	public float maxHealth;
	public float health;

	public Armor armor;
	public Weapon weapon;

	public static int strength = 3;
	public int attack;
	public float defense; 
	public float accuracy;
	public int constitution = 4;

	public int counterAttackActivated = 3;
	public int tripped;
	
	public float chanceToHit;

	public EnemyStats[] EnemiesStats;
	public EnemyStats targetEnemyStats;

	public Animator Anim;

	public bool predeceased = false;

    private Transform damageTextCanvas;

	// Use this for initialization
	void Start () {
		attack = strength + weapon.attack;
		health = maxHealth;
		defense = 4 + armor.defense;
		Anim = GetComponent<Animator> ();
		tripped = 0;
		unitImage.texture = null;
        damageTextCanvas = thisUnit.damageText.transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (thisUnit.isSelected == 1) {
			targetEnemyStats = determineTargetEnemyStats ();
			if (targetEnemyStats != null) {
				chanceToHit = Mathf.RoundToInt (accuracy / targetEnemyStats.defense * 100);
			}
		}

		if ((thisUnit.health > 0
            && selectionCube.position.x == thisUnit.transform.position.x 
		    && selectionCube.position.z == thisUnit.transform.position.z 
		    && CheckOtherUnitsSelected()==true)
		    || thisUnit.isSelected == 1) {
			nameText.text = unitName.ToString ();
			unitImage.texture = thisUnitTexture;
			healthText.text = "HEALTH";
			healthNumberText.text = health.ToString () + "/" + maxHealth.ToString ();
		}
		//SetHealthBar ();
		if (health < 0) {
			health = 0;
		}
		if (health == 0) {
			Anim.Play ("Die");
			die ();
		}
		if (tripped == 1)
		{
            
            transform.rotation = Quaternion.Euler(75f, 0, 0);
			defense = 1;
			tripped = 2;
		}
		if (tripped == 4)
		{
            
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
			defense = 4 + armor.defense;
			//statusText.text = null; 
			tripped = 0;
		}
	}

	public void SetHealthBar(){
		healthBar.transform.localScale = new Vector3 (health/maxHealth, healthBar.transform.localScale.y, 
		                                              healthBar.transform.localScale.z);
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

	EnemyStats determineTargetEnemyStats(){
		EnemyStats target = null;
		foreach (EnemyStats e in EnemiesStats) {
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

	public void die(){
        Vector3 deadPosition = new Vector3(deathCube.position.x + .5f, deathCube.position.y, deathCube.position.z);
		transform.position = Vector3.MoveTowards(transform.position, deadPosition, Time.deltaTime);
		predeceased = true;
	}

	public void displayImage(){
		nameText.text = unitName.ToString ();
		unitImage.texture = thisUnitTexture;
		healthText.text = "HEALTH";
		healthNumberText.text = health.ToString () + "/" + maxHealth.ToString ();
		thisUnit.fatigueText.text = "STAMINA";
		thisUnit.fatigueNumber.text = thisUnit.fatigue.ToString () + "/" + thisUnit.maxFatigue.ToString ();
		thisUnit.typeText.text = thisUnit.type;
		if (thisUnit.fatigue >= thisUnit.moveRange / 2) {
			thisUnit.movementText.text = (thisUnit.moveRange / 2).ToString ();
		} else if (thisUnit.fatigue < thisUnit.moveRange / 2) {
			thisUnit.movementText.text = thisUnit.fatigue.ToString();
		}
	}
}
