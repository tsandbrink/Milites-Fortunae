using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour {
	public Vector3 target;

	public Transform selectionCube;
	//public Transform thisEnemyUnit;
	public float speed = 1;
	public int maxMoveRange = 9;
	public int moveRange = 9;
	public int fatigue = 9;
	//public int isSelected;
	public Vector3[] path;
	public int targetIndex;
	public Text fatigueText;
	public Text enemyMoveRangeText;
	public int isSelected = 0;
	public EnemyStats thisEnemyStats;
	public int maxFatigue;
	public float health;

	//public GameObject ColliderBox;

	public Pathfinding pathfinding;

	public string Type;
    public Text typeText;

    public Text Abilities;
    public Text Abilities1;
    public Text Abilities2;
    public Text Abilities3;

	public Text Cost1;
	public Text Cost2;
	public Text Cost3;

	public int Ability1Cost;
	public int Ability2Cost;
	public int Ability3Cost;

    public string Ability1;
    public string Ability2;
    public string Ability3;

	public SkinnedMeshRenderer[] thisMesh;

	public DualSwordAttack dualSwordAttack = null;
	public EnemySoldierAttack enemySoldierAttack = null;
	public EnemyArcherAttack enemyArcherAttack = null;
	public EnemyLightInfantryAttack enemyLightInfantryAttack = null;
	public EnemyLeaderAttack enemyLeaderAttack = null;
	public BoarAttack boarAttack = null;
	public EnemySkeletonAttack skeletonAttack = null;

	public int isMoving;

	public Node targetNode = null;

	public int acc;
	public int pow;
	public int def;

	public Image accImage;
	public Image powImage;
	public Image defImage;

	public GameObject colliderBox;
	public GameObject mine;

	public Text fatigueDamageText;

	public int inRange = 0;

	public bool canAttack = false;
	public bool mined = false;

	public Light selectedLight;

	void Start(){
		acc = 1;
		pow = 0;
		def = 0;
		fatigue = maxFatigue;
		moveRange = maxMoveRange;
        Abilities.gameObject.SetActive(false);
		isMoving = 0;
		selectedLight.gameObject.SetActive (false);
	}

	void Update() {
		 
		health = thisEnemyStats.health;
		if (health < 0) {
			health = 0;
		}
		if (health == 0) {
			fatigue = 0;
			StartCoroutine ("turnOffEnemyStats");
		}
		if (selectionCube.position.x == transform.position.x && selectionCube.position.z == transform.position.z) {
			DisplayFatigue ();
			typeText.text = Type.ToString ();
			Abilities.gameObject.SetActive (true);
			Abilities1.text = Ability1;
			Abilities2.text = Ability2;
			Abilities3.text = Ability3;
			Cost1.text = Ability1Cost.ToString ();
			Cost2.text = Ability2Cost.ToString ();
			Cost3.text = Ability3Cost.ToString ();
		}
		if (fatigue > maxFatigue) {
			fatigue = maxFatigue;
		}
		if (isMoving == 1 || BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEMOVE 
			|| BattleStateMachine.currentState == BattleStateMachine.BattleStates.PLAYERCHOICEATTACK) {
			if (transform.position.x == mine.transform.position.x
				&& transform.position.z == mine.transform.position.z && mined == false) {
				thisEnemyStats.health -= ThiefAttack.mineDam;
				pathfinding.BattleStateMachine.ExplosionAudioSource.Play ();
				thisEnemyStats.SetHealthBar ();
				selectedLight.gameObject.SetActive (false);
				playExplosion();
				mined = true;
			// ExplosiveAnimation
				thisEnemyStats.Anim.Play ("GetHit");
				if (isMoving == 1) {
					thisEnemyStats.enemyDamageText.text = ThiefAttack.mineDam.ToString ();
				} else {
					Invoke ("mineDamageText", 1);
				}
				Debug.Log ("hit");
				StopCoroutine ("FollowPath");
				if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYDECREASEFATIGUE) {
					BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
				}
				Invoke ("clearText", 1.5f);
				Invoke ("moveMine", 1.5f);
			}
		}	



		//	float targetX = target.transform.localPosition.x;
		//	float targetZ = target.transform.localPosition.z;
	
	}

	void playExplosion(){
	//	for (int x = 0; x < pathfinding.mineExplosion.Length; x++) {
	//		pathfinding.mineExplosion [x].Play ();
	//	}
			//for (int i = 0; i < nodesInRange.Count; i++)
		foreach (ParticleSystem p in pathfinding.mineExplosion) {
			p.Play ();
		}
	}

	void moveMine(){
		Vector3 v = new Vector3 (100, mine.transform.position.y, 0);
		mine.transform.position = Vector3.MoveTowards (mine.transform.position, v, 1000 * Time.deltaTime);
	}

	void clearText(){
		thisEnemyStats.enemyDamageText.text = null;
	}

	void mineDamageText(){
		thisEnemyStats.enemyDamageText.text = ThiefAttack.mineDam.ToString ();
	}

	public void EnemyUnitMove1(){
		Invoke ("EnemyUnitMove", 0f); //keep at 0
	//	StartCoroutine ("EnemyUnitMove");
	//	isMoving = 0;
	}

	//IEnumerator EnemyUnitMove(){
	public void EnemyUnitMove(){
		//yield return new WaitForSeconds(1);
		if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYCHOICE
		    && isSelected == 1
		    && (Mathf.Abs (target.x - transform.position.x) + Mathf.Abs (target.z - transform.position.z)) > 0) {
			//PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
			target = targetNode.worldPosition;
			path = pathfinding.FindPath (transform.position, target);
			StartCoroutine ("FollowPath");
			targetIndex = 0;

			if (fatigue < 0) {
				fatigue = 0;
			}

		} else {
			BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYATTACK;
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
		//yield return new WaitForSeconds (1); Don't enable this
		Vector3 currentWaypoint = path[0];
        //thisEnemyStats.BattleStateMachine.mainCamera.transform.parent = transform;
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYATTACK;
				
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			isMoving = 1;
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			transform.LookAt (currentWaypoint);
		
			BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYDECREASEFATIGUE;
			yield return null;
			
		}
	

	}
		

	void DisplayFatigue(){
		fatigueText.text = fatigue.ToString () + "/" + maxFatigue.ToString();

		if (fatigue >= maxMoveRange/2){
			enemyMoveRangeText.text = (moveRange / 2).ToString ();
		}
		else if (fatigue < maxMoveRange/2){
			enemyMoveRangeText.text = fatigue.ToString();
		
		}
	}
	
	IEnumerator turnOffEnemyStats(){
		yield return new WaitForSeconds(1);
		thisEnemyStats.enabled = false;
	}

	public void accuracy(){
		if (thisEnemyStats.health > 0 && acc == 0) {
			thisEnemyStats.accuracy += 2;
			if (pow == 1) {
				thisEnemyStats.attack -= 1;
			} else if (def == 1) {
				thisEnemyStats.defense -= 1;
			}
			acc = 1;
			pow = 0;
			def = 0;
			accImage.gameObject.SetActive (true);
			powImage.gameObject.SetActive (false);
			defImage.gameObject.SetActive (false);
		}
	}

	public void power(){
		if (thisEnemyStats.health > 0 && pow == 0) {
			thisEnemyStats.attack += 1;
			if (acc == 1) {
				thisEnemyStats.accuracy -= 2;
			} else if (def == 1) {
				thisEnemyStats.defense -= 1;
			}
			acc = 0;
			pow = 1;
			def = 0;
			accImage.gameObject.SetActive (false);
			powImage.gameObject.SetActive (true);
			defImage.gameObject.SetActive (false);
		}
	}

	public void defense(){
		if (thisEnemyStats.health > 0 && def == 0){
			thisEnemyStats.defense += 1;
			if (acc == 1) {
				thisEnemyStats.accuracy -= 2;
			} else if (pow == 1) {
				thisEnemyStats.attack -= 1;
			}
			acc = 0;
			pow = 0;
			def = 1;
			accImage.gameObject.SetActive (false);
			powImage.gameObject.SetActive (false);
			defImage.gameObject.SetActive (true);
		}
	}
}
