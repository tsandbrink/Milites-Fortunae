using UnityEngine;
using System.Collections;

public class Battles3 : MonoBehaviour {

	public enum BattlesEnum{
		One,
		Two
	}

	public EnemyUnit[] enemies;
	public static BattlesEnum currentBattle;

	public static int EnemiesInFight = 0;

	BattleStateMachine battleStateMachine;

	void Awake(){
		battleStateMachine = gameObject.GetComponent<BattleStateMachine> ();
		if (currentBattle == BattlesEnum.One) {
			enemies [1].thisEnemyStats.maxHealth = 0;
			enemies [1].maxFatigue = 0;
			enemies [2].thisEnemyStats.maxHealth = 0;
			enemies [2].maxFatigue = 0;

			foreach (EnemyUnit e in enemies) {
				if (e.thisEnemyStats.maxHealth == 0) {
					foreach (SkinnedMeshRenderer s in e.thisMesh) {
						s.gameObject.SetActive (false);
					}
				}
			}

			Vector3 enemy0set = new Vector3 (-2.5f, 0, 3.5f);
			enemies [0].transform.position = enemy0set;
			enemies [0].transform.rotation = Quaternion.Euler (0, 90f, 0);

			Vector3 enemy3set = new Vector3 (-1.5f, 0, -.5f);
			enemies [3].transform.position = enemy3set;
			enemies [3].transform.rotation = Quaternion.Euler (0, 90f, 0);

			EnemiesInFight = 3;
		} else if (currentBattle == BattlesEnum.Two) {
            return;
		}
	}
}
