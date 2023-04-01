using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
	Animator Anim;
	public Unit thisUnit;
	public int switcher;
	// Use this for initialization
	void Start () {
		Anim = GetComponent<Animator> ();
		switcher = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (thisUnit.isMoving == 1 && thisUnit.targetIndex < thisUnit.path.Length && switcher == 0) {
			Anim.Play ("Move");
			switcher = 1;
		}
		else if (thisUnit.targetIndex >= thisUnit.path.Length && switcher == 1){
			Anim.Play ("HumanoidIdle");
			switcher = 0;
		}
	}
}
