using UnityEngine;
using System.Collections;

public class Battles2 : MonoBehaviour {
	public enum BattlesEnum{
		One,
		Two,
		Three
	}

	public SelectUnits rosterManager;
	public Transform[] unwalkables;
	public EnemyUnit[] enemies;
	public static BattlesEnum currentBattle;
	public EnemyTarget EnemyAI;

	public static int EnemiesInFight = 0;


	void Awake(){
		if (currentBattle == BattlesEnum.One) {
			//Sergius.isOnRoster = 0;
		
			enemies[3].thisEnemyStats.maxHealth = 0;
			enemies[3].maxFatigue = 0;
			enemies[4].thisEnemyStats.maxHealth = 0;
			enemies[4].maxFatigue = 0;

			foreach (Transform t in unwalkables){
				t.gameObject.SetActive (false);
			}
			foreach (EnemyUnit e in enemies){
				if (e.thisEnemyStats.maxHealth == 0){
					foreach (SkinnedMeshRenderer s in e.thisMesh){
						s.gameObject.SetActive(false);
					}
				}
			}

			rosterManager.maxUnitsInScene = 3;
			EnemiesInFight = 3;
		}

		if (currentBattle == BattlesEnum.Two) {
			//Sergius.isOnRoster = 0;


			foreach (Transform t in unwalkables){
				t.gameObject.SetActive (false);
			}
			foreach (EnemyUnit e in enemies){
				if (e.thisEnemyStats.maxHealth == 0){
					foreach (SkinnedMeshRenderer s in e.thisMesh){
						s.gameObject.SetActive(false);
					}
				}
			}

			rosterManager.maxUnitsInScene = 5;
			EnemiesInFight = 5;
		}

		if (currentBattle == BattlesEnum.Three) {
			//Sergius.isOnRoster = 0;
			//Vector3 enemy0set = new Vector3 (-5.5f, 0, -2.5f);
			//enemies[0].transform.position = enemy0set;
			//enemies[0].transform.rotation = Quaternion.Euler (0, 90f, 0);
			//Vector3 enemy1set = new Vector3 (-5.5f, 0, -.5f);
			//enemies[2].transform.position = enemy1set;
			//enemies[2].transform.rotation = Quaternion.Euler (0, 90f, 0);
			Vector3 enemy3set = new Vector3 (-6.5f, 0, -.5f);
			enemies[3].transform.position = enemy3set;
			enemies[3].transform.rotation = Quaternion.Euler (0, 90f, 0);
			Vector3 enemy4set = new Vector3 (-7.5f, 0, -2.5f);
			enemies[4].transform.position = enemy4set;
			enemies[4].transform.rotation = Quaternion.Euler (0, 90f, 0);

			foreach (Transform t in unwalkables){
				t.gameObject.SetActive (true);
			}
			foreach (EnemyUnit e in enemies){
				if (e.thisEnemyStats.maxHealth == 0){
					foreach (SkinnedMeshRenderer s in e.thisMesh){
						s.gameObject.SetActive(false);
					}
				}
			}

			rosterManager.maxUnitsInScene = 4;
			EnemiesInFight = 5;
		}
	}
	// Use this for initialization

	
	// Update is called once per frame

}
