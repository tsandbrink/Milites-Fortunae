using UnityEngine;
using System.Collections;

public class EnemyAnimationWalk : MonoBehaviour {

	Animator Anim;
	public EnemyUnit thisEnemyUnit;
	public int switcher;

	// Use this for initialization
	void Start () {
		Anim = GetComponent<Animator>();
		switcher = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (thisEnemyUnit.isMoving == 1 && thisEnemyUnit.targetIndex < thisEnemyUnit.path.Length && switcher == 0) {
			Anim.Play ("Move");
			switcher = 1;
		}
		else if (thisEnemyUnit.targetIndex >= thisEnemyUnit.path.Length && switcher == 1){
			Anim.Play ("HumanoidIdle");
			switcher = 0;
		}
	}
}
