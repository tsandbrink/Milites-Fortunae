using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Battles : MonoBehaviour {
	public enum BattlesEnum{
		One,
		Two,
		Three,
		Four,
		Five
	}

	public SelectUnits rosterManager;
	public Transform[] unwalkables;
	public EnemyUnit[] enemies;
	public static BattlesEnum currentBattle;
	public Unit Magnus;
	public Unit Sergius;
	public EnemyTarget EnemyAI;

    public GameObject[] TutorialTexts;
    public Button[] Abilities;

    public static int EnemiesInFight = 0;

	Vector3 temp;
	Vector3 temp1;
	Vector3 unwalkablesTemp;
	Vector3 unwalkablesTemp1;

    bool tutorialSwitch = true;
    bool tutorialSwitch3 = true;
    bool tutorialSwitch6 = true;

	public TileMap startAreaTileMap;
	Vector3 StartAreaTemp;
	// Use this for initialization
	void Awake () {
		if (PlayerPrefs.GetInt ("Magnus") == 1) {
			Magnus.isOnRoster = 1;
		} else {
			Magnus.isOnRoster = 0;
		}
        foreach (GameObject g in TutorialTexts)
        {
            g.SetActive(false);
        }
		if (currentBattle == BattlesEnum.One) {
			//Sergius.isOnRoster = 0;
            TutorialTexts[0].SetActive(true); //turn first Tutorial Text on
            TutorialTexts[1].SetActive(true);
            TutorialTexts[8].SetActive(true);
			temp.x = -.5f;
			temp.z = .5f;
			enemies[1].thisEnemyStats.maxHealth = 0;
			enemies[1].maxFatigue = 0;
			enemies[2].thisEnemyStats.maxHealth = 0;
			enemies[2].maxFatigue = 0;
			enemies[3].thisEnemyStats.maxHealth = 0;
			enemies[3].maxFatigue = 0;
			enemies[0].transform.position = temp;
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
			StartAreaTemp.x = -1;
			StartAreaTemp.z = 2;
			StartAreaTemp.y = .01f;
			startAreaTileMap.transform.position = StartAreaTemp;
			startAreaTileMap.size_x = 2;
			startAreaTileMap.size_z = 2;
			rosterManager.maxUnitsInScene = 1;
            EnemiesInFight = 1;
		}

		if (currentBattle == BattlesEnum.Two) {
			//Magnus.isOnRoster = 1;
			enemies[2].thisEnemyStats.maxHealth = 0;
			enemies[2].maxFatigue = 0;
			enemies[3].thisEnemyStats.maxHealth = 0;
			enemies[3].maxFatigue = 0;
			foreach (SkinnedMeshRenderer s in enemies[2].thisMesh){
				s.gameObject.SetActive(false);
			}

			temp.x = -3.5f;
			temp.z = -3.5f;
			enemies[0].transform.position = temp;


			temp1.x = -1.5f;
			temp1.z = -3.5f;
			enemies[1].transform.position = temp1;

		
			unwalkablesTemp.x = .5f;
			unwalkablesTemp.z = -2.5f;
			unwalkablesTemp.y = .35f;
			unwalkables[0].position = unwalkablesTemp;


			unwalkablesTemp1.x = -1.5f;
			unwalkablesTemp1.z = -1.5f;
			unwalkablesTemp1.y = .5f;
			unwalkables[1].position = unwalkablesTemp1;

			for (int x = 2; x < unwalkables.Length; x++){
				unwalkables[x].gameObject.SetActive(false);
			}
			rosterManager.maxUnitsInScene = 2;
            EnemiesInFight = 2;
            Vector3 TileMapSet = new Vector3(-3, .1f, 2);
            startAreaTileMap.transform.position = TileMapSet;
            startAreaTileMap.size_x = 4;
            startAreaTileMap.size_z = 2;
        }

		if (currentBattle == BattlesEnum.Three) {
			enemies[3].thisEnemyStats.maxHealth = 0;
			enemies[3].maxFatigue = 0;

			Vector3 unwalkable0set = new Vector3 (.5f, .35f, .5f);
			unwalkables[0].gameObject.transform.position = unwalkable0set;
			Vector3 unwalkable1set=new Vector3(-1.5f, .5f, .5f);
			unwalkables[1].gameObject.transform.position = unwalkable1set;
			Vector3 unwalkable2set=new Vector3(-2.5f, .35f, .5f);
			unwalkables[2].gameObject.transform.position = unwalkable2set;
			Vector3 unwalkable3set=new Vector3(1.5f, .35f, .5f);
			unwalkables[3].gameObject.transform.position = unwalkable3set;
			Vector3 unwalkable4set=new Vector3(2.5f, .5f, .5f);
			unwalkables[4].gameObject.transform.position = unwalkable4set;
			unwalkables[5].gameObject.SetActive(false);
			unwalkables [6].gameObject.SetActive (false);

			Vector3 enemy0set = new Vector3 (3.5f, 0, -3.5f);
			enemies[0].transform.position = enemy0set;
			Vector3 enemy1set = new Vector3 (.5f, 0, -2.5f);
			enemies[1].transform.position = enemy1set;
			Vector3 enemy2set = new Vector3 (2.5f, 0, -1.5f);
			enemies[2].transform.position = enemy2set;

			Vector3 TileMapSet = new Vector3 (-3, .1f, 2); 
			startAreaTileMap.transform.position = TileMapSet;
			startAreaTileMap.size_x = 7;
			startAreaTileMap.size_z = 1;
			rosterManager.maxUnitsInScene = 3;
            EnemiesInFight = 3;
		}

		if (currentBattle == BattlesEnum.Four) {
			EnemyAI.BattleIVswitch = 1;
			Vector3 TileMapSet = new Vector3 (-1, .1f, -1); 
			startAreaTileMap.transform.position = TileMapSet;
			startAreaTileMap.size_x = 2;
			startAreaTileMap.size_z = 2;
			rosterManager.maxUnitsInScene = 3;
            EnemiesInFight = 4;

			//enemies
			Vector3 enemy0set = new Vector3 (3.5f, 0, 2.5f);
			enemies[0].transform.position = enemy0set;
			enemies[0].transform.rotation = Quaternion.Euler (0, 180f, 0);
			Vector3 enemy1set = new Vector3 (-2.5f, 0, 2.5f);
			enemies[1].transform.position = enemy1set;
			enemies[1].transform.rotation = Quaternion.Euler (0, 180f, 0);
			Vector3 enemy2set = new Vector3 (2.5f, 0, -2.5f);
			enemies[2].transform.position = enemy2set;
			Vector3 enemy3set = new Vector3 (-1.5f, 0, -1.5f);
			enemies[3].transform.position = enemy3set;

			//unwalkables
			Vector3 unwalkable0set = new Vector3 (.5f, .35f, -1.5f);
			unwalkables[0].gameObject.transform.position = unwalkable0set;
			Vector3 unwalkable1set=new Vector3(-1.5f, .5f, .5f);
			unwalkables[1].gameObject.transform.position = unwalkable1set;
			Vector3 unwalkable2set=new Vector3(-1.5f, .35f, -.5f);
			unwalkables[2].gameObject.transform.position = unwalkable2set;
			Vector3 unwalkable3set=new Vector3(1.5f, .35f, .5f);
			unwalkables[3].gameObject.transform.position = unwalkable3set;
			Vector3 unwalkable4set=new Vector3(-.5f, .5f, 1.5f);
			unwalkables[4].gameObject.transform.position = unwalkable4set;
			Vector3 unwalkable5set = new Vector3 (.5f , .5f, 1.5f);
			unwalkables[5].gameObject.transform.position = unwalkable5set;
			Vector3 unwalkable6set = new Vector3 (2.5f, .5f, .5f);
			unwalkables [6].gameObject.transform.position = unwalkable6set;
		}


	}
	
	// Update is called once per frame
    void Update()
    {
        if (currentBattle == BattlesEnum.One)
        {
            if (TutorialTexts[3].activeInHierarchy == true)
            {
                if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET ||
                    BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYATTACK)
                {
                    TutorialTexts[3].SetActive(false);
                    //       TutorialTexts[4].SetActive(true);
                }
            }
            foreach (Button b in Abilities)
            {
                if (b.interactable == true && b.gameObject.activeInHierarchy == true && tutorialSwitch == true)
                {
                    TutorialTexts[4].SetActive(true);
                    TutorialTexts[7].SetActive(true);
                    TutorialTexts[9].SetActive(true);
                    TutorialTexts[3].SetActive(false);
                    tutorialSwitch = false;
                }
            }
            if (BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYFINDTARGET ||
                    BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYATTACK)
            {
                TutorialTexts[4].SetActive(false);
                TutorialTexts[5].SetActive(false);
                TutorialTexts[7].SetActive(false);
                TutorialTexts[9].SetActive(false);
            }
            if (tutorialSwitch == false && BattleStateMachine.currentState == BattleStateMachine.BattleStates.ENEMYRECALCULATE)
            {
                StartCoroutine(displayTutorial6());
            }
        }
    }

    IEnumerator displayTutorial6()
    {
        yield return new WaitForSeconds(3.5f);
        if (tutorialSwitch6 == true)
        {
            TutorialTexts[6].SetActive(true);
            TutorialTexts[10].SetActive(true);
            tutorialSwitch6 = false;
        }
    }

	public void Battle1SetCharacter(){
		if (currentBattle == BattlesEnum.One) {
			int i = 0;
			do {
				Vector3 playerPositionTemp;
				playerPositionTemp.x = .5f;
				playerPositionTemp.z = 2.5f;
				playerPositionTemp.y = 0;
				rosterManager.unitsInScene[0].transform.position = playerPositionTemp;
				i++;
			} while (i == 0);
		}
	}

	public void Battle2SetCharacter(){
		if (currentBattle == BattlesEnum.Two) {
			int i = 0;
			do {
				Vector3 playerPositionTemp;
				playerPositionTemp.x = .5f;
				playerPositionTemp.z = 2.5f;
				playerPositionTemp.y = 0;
				rosterManager.unitsInScene[0].transform.position = playerPositionTemp;
				Vector3 playerPositionTemp2;
				playerPositionTemp2.x = -.5f;
				playerPositionTemp2.z = 2.5f;
				playerPositionTemp2.y = 0;
				rosterManager.unitsInScene[1].transform.position = playerPositionTemp2;
				i ++;
			} while (i == 0);
		}
	}

	public void Battle3SetCharacter(){
		if (currentBattle == BattlesEnum.Three) {
			int i = 0;
			do {
				Vector3 playerPositionTemp;
				playerPositionTemp.x = .5f;
				playerPositionTemp.z = 2.5f;
				playerPositionTemp.y = 0;
				rosterManager.unitsInScene[0].transform.position = playerPositionTemp;
				Vector3 playerPositionTemp2;
				playerPositionTemp2.x = 1.5f;
				playerPositionTemp2.z = 2.5f;
				playerPositionTemp2.y = 0;
				rosterManager.unitsInScene[1].transform.position = playerPositionTemp2;
				Vector3 playerPositonTemp3 = new Vector3 (2.5f, 0, 2.5f);
				rosterManager.unitsInScene[2].transform.position = playerPositonTemp3;
				i ++;
			} while (i == 0);
		}
	}

	public void Battle4SetCharacter(){
		if (currentBattle == BattlesEnum.Four) {
			int i = 0;
			do {
				Vector3 playerPositionTemp;
				playerPositionTemp.x = -.5f;
				playerPositionTemp.z = -.5f;
				playerPositionTemp.y = 0;
				rosterManager.unitsInScene[0].transform.position = playerPositionTemp;
				Vector3 playerPositionTemp2;
				playerPositionTemp2.x = -.5f;
				playerPositionTemp2.z = .5f;
				playerPositionTemp2.y = 0;
				rosterManager.unitsInScene[1].transform.position = playerPositionTemp2;
				Vector3 playerPositonTemp3 = new Vector3 (.5f, 0, .5f);
				rosterManager.unitsInScene[2].transform.position = playerPositonTemp3;
				i ++;
			} while (i == 0);
		}
	}

    public void TutorialTextSwitch1(){
        int i = 0;
        if (currentBattle == BattlesEnum.One && i == 0)
        {
            TutorialTexts[0].SetActive(false);
            TutorialTexts[1].SetActive(false);
            TutorialTexts[8].SetActive(false);
            TutorialTexts[2].SetActive(true);
            i++;
        }
    }

    public void TutorialTextSwitch2()
    {
        int i = 0;
        if (currentBattle == BattlesEnum.One && i == 0)
        {
            TutorialTexts[2].SetActive(false);
            TutorialTexts[3].SetActive(true);
            i++;
        }
    }

    public void TutorialTextSwitch3()
    {
        
        if (currentBattle == BattlesEnum.One && tutorialSwitch3 == true)
        {
            TutorialTexts[4].SetActive(false);
            TutorialTexts[5].SetActive(true);
            TutorialTexts[7].SetActive(false);
            TutorialTexts[9].SetActive(false);
            tutorialSwitch3 = false; 
        }
    }

    public void TutorialTextSwitch4()
    {
        int i = 0;
        if (currentBattle == BattlesEnum.One && i == 0)
        {
            TutorialTexts[6].SetActive(false);
            TutorialTexts[10].SetActive(false);
            i++;
        }
    }

}
