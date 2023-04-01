using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {
	//Tells the AI whether it's the AI or Player's Turn
    //Calls AI Functions used to determine the AI's move
    //Automatically moves the Camera on the AI's turn
    //Executes functions if the player beats the level or loses
    
    public Transform enemy = null;
	public Unit[] units;
	public Unit playerUnit; //target
	public EnemyTarget EnemyTarget;
	public EnemyUnit thisEnemyUnit;
	public EnemyUnit[] enemyUnits;
	//public Text[] playerDamageTexts;
	public Text playerDamageText;
	public Text WinText;
    public Text RewardText;
	Color d = new Color(255, 255 ,255);
	public TileMapMouse startAreaMap;
	public UnitStats[] Units;
	public UnitStats TargetUnitStats;
		
	public int EnemyStandardFCost = 2;
	public int EnemyPowerFCost;
	public float standardAttackAcc = 0;
	public float powerAttackAcc = -10;
	
	public int standardAttackDam = 0;
	public int powerAttackDam = 2; 
	int i;

	public static int hasRecalculated = 0;

	public Image characterSelectionMenu;

	public Button startButton;
	public Button endTurnButton;

	public SelectUnits rosterManager;

	public TurnOffWalkables turnOffWalkables;

	public Text attackLog;
	public GameObject attackLogPanel;

	public AudioClip Hit;
	public AudioClip Miss;
    public AudioClip restSound;
	public AudioSource FxAudioSource;
	public AudioSource Music;
	public AudioSource ExplosionAudioSource;

	public static AudioSource ButtonSource;
    public AudioClip error;
    public AudioClip buttonSound;

	public Camera mainCamera;
	//Quaternion mainCameraRotation;

	public int turnCounter = 0;
	public Scrollbar attackLogScrollyBar;
	public RawImage turnSword;
	Texture turnSwordTexture1;
	public Texture turnSwordTexture2;

	bool PlayerChoiceMoveFunctionBlock;
    private bool enemyMoved;

    private int ifPlayerCounterAttacked;

	public enum BattleStates{
		START,
		PLAYERCHOICEMOVE,
		PLAYERCHOICEATTACK,
		ENEMYFINDTARGET,
		ENEMYCHOICE,
		ENEMYDECREASEFATIGUE,
		ENEMYATTACK,
		ENEMYRECALCULATE,
		LOSE,
		WIN
	}

	public static BattleStates currentState;

    public enum BattleNumbers
    {
        BATTLE1,
        BATTLE2,
        BATTLE3,
        BATTLE4,
        BATTLE5,
        BATTLE6,
        BATTLE7,
        BATTLE8,
        BATTLE9,
        BATTLE10,
        BATTLE11,
        BATTLE12,
        BATTLE13,
        BATTLE14
    }

    public static BattleNumbers currentBattle;
    private int battleNumbersInt;

	public float mainCameraRotX;
	public float mainCameraFollowY = -2.6f;

	public float counterWaitTime = .9f;

	public int battleMenuLoad;

    public Vector3 currentMainCameraPos;

    public static int battleButtonLock = 0;
    private int goldLock;

    public static int healthBoost = 0;
    public static int staminaBoost = 0;
    public static int moveRangeBoost = 0;
	//float mainCameraCenteringDistZ;

	// Use this for initialization
	void Start () {
        foreach (Unit u in units)
        {
            u.maxFatigue += staminaBoost;
            u.thisUnitStats.maxHealth += healthBoost;
            u.moveRange += moveRangeBoost;
            u.fatigue = u.maxFatigue;
            u.health = u.thisUnitStats.maxHealth;
            u.thisUnitStats.health = u.thisUnitStats.maxHealth;
        }
        ifPlayerCounterAttacked = 0;
        goldLock = 0;
        enemyMoved = false;
        ButtonSource = gameObject.GetComponent<AudioSource> ();
        ButtonSource.volume = .8f;
		turnSwordTexture1 = turnSword.texture;
		currentState = BattleStates.START;
		WinText.gameObject.SetActive (false);
        RewardText.gameObject.SetActive(false);
		attackLog.gameObject.SetActive (false);
		attackLogPanel.gameObject.SetActive (false);
		PlayerChoiceMoveFunctionBlock = false;
		//enemyMoveRangeText.text = (EnemyUnit.moveRange / 2).ToString ();
		i = 0;
		mainCamera = gameObject.GetComponent<Camera> ();
		//mainCameraRotation = mainCamera.transform.rotation;
		mainCameraRotX = Mathf.Deg2Rad * mainCamera.transform.rotation.eulerAngles.x;
		//mainCameraCenteringDistZ = mainCamera.transform.position.y * (Mathf.Tan (mainCameraRotX));
        currentMainCameraPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.z, mainCamera.transform.position.y);

        // set battle number int
        if (currentBattle == BattleNumbers.BATTLE1)
        {
            battleNumbersInt = 0;
        } else if (currentBattle == BattleNumbers.BATTLE2){
            battleNumbersInt = 1;
        }
        else if (currentBattle == BattleNumbers.BATTLE3)
        {
            battleNumbersInt = 2;
        }
        else if (currentBattle == BattleNumbers.BATTLE4)
        {
            battleNumbersInt = 3;
        }
        else if (currentBattle == BattleNumbers.BATTLE5)
        {
            battleNumbersInt = 4;
        }
        else if (currentBattle == BattleNumbers.BATTLE6)
        {
            battleNumbersInt = 5;
        }
        else if (currentBattle == BattleNumbers.BATTLE7)
        {
            battleNumbersInt = 6;
        }
        else if (currentBattle == BattleNumbers.BATTLE8)
        {
            battleNumbersInt = 7;
        }
        else if (currentBattle == BattleNumbers.BATTLE9)
        {
            battleNumbersInt = 8;
        }
        else if (currentBattle == BattleNumbers.BATTLE10)
        {
            battleNumbersInt = 9;
        }
        else if (currentBattle == BattleNumbers.BATTLE11)
        {
            battleNumbersInt = 10;
        }
        else if (currentBattle == BattleNumbers.BATTLE12)
        {
            battleNumbersInt = 11;
        }
        else if (currentBattle == BattleNumbers.BATTLE13)
        {
            battleNumbersInt = 12;
        }
        else if (currentBattle == BattleNumbers.BATTLE14)
        {
            battleNumbersInt = 13;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (rosterManager.unitsInScene.Count == 0) {
			startButton.interactable = false;
		} else {
			startButton.interactable = true;
		}

		if (currentState == BattleStates.START) {
			endTurnButton.interactable = false;
		} else {
			endTurnButton.interactable = true;
		}
	
	
		switch (currentState) {
		case (BattleStates.START):
			break;
		case(BattleStates.PLAYERCHOICEMOVE):
            if (turnSword.texture != turnSwordTexture1 && enemyMoved == true){
                StartCoroutine(resetCamera2()); //Use this to reset the Camera after Enemy moves
                
            }
            	//turnSword.texture = turnSwordTexture1;
			//if (PlayerChoiceMoveFunctionBlock == false) {
			if (mainCamera.transform.position == currentMainCameraPos){
                turnSword.texture = turnSwordTexture1;
            }
			if (EnemyTarget.checkPlayerHealth () == true) {
					currentState = BattleStates.LOSE;
					EnemyTarget.EndTurnButton.interactable = false;
				}
				turnOffWalkables.TurnOnUnwalkables ();
			if (PlayerChoiceMoveFunctionBlock == false){
				foreach (EnemyUnit e in enemyUnits) {
					e.mined = false;
				}
				PlayerChoiceMoveFunctionBlock = true;
			}
		//	Invoke ("resetCamera", 1.0f);
			break;
		case(BattleStates.PLAYERCHOICEATTACK):
            int abilityCheck = 0;
            foreach (Button b in rosterManager.attackButtons)
            {
                if (b.interactable == true && b.gameObject.activeInHierarchy == true)
                {
                    abilityCheck += 1; 
                }
            }
            if (abilityCheck == 0)
            {
                currentState = BattleStates.ENEMYFINDTARGET;
            }
            break;
		case(BattleStates.ENEMYFINDTARGET): //Enemy Selects its Unit & Picks Player Target
			//if (EnemyTarget.checkEnemiesHealth () == true) {
			//	currentState = BattleStates.WIN;
			//	EnemyTarget.EndTurnButton.interactable = false;
			//}
            ifPlayerCounterAttacked = 0;
            enemyMoved = false;
            foreach (Unit u in units)
            {
                Color c = new Color(255, 0, 0, 255);
                u.damageText.color = c;
                u.damageText.text = null;
            }
            StartCoroutine(changeColor2()); 
            currentMainCameraPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
			turnSword.texture = turnSwordTexture2;
			if (enemyTrippedContingency () != 0) {
				//Skips Enemy Functions if Enemy is Tripped
                thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Pass";
                thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
                currentState = BattleStates.ENEMYRECALCULATE;
			} else if (EnemyTarget.i == 0) { //i is an integer used so the MakeMove function only gets called once - MakeMove function will increase it
				//enemySelectedLightTrue ();
				thisEnemyUnit = EnemyTarget.MakeMove (); //enemy picks its unit
				//StartCoroutine ("enemySelectedLightTrue");
				enemy = thisEnemyUnit.transform;
				playerUnit = EnemyTarget.continueMakingMove (thisEnemyUnit); //enemyUnit finds player target
                float distanceX = thisEnemyUnit.transform.position.x - playerUnit.transform.position.x;
                float distanceZ = thisEnemyUnit.transform.position.z - playerUnit.transform.position.z;
                if (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ) > 1.0f)
                {
                    StartCoroutine("enemySelectedLightTrue");
                    enemyMoved = true;
                }
                else
                {
                    StartCoroutine("enemySelectedLightTrue1");
                }
                //playerUnit = EnemyTarget.playerUnit;
			}

			//Invoke ("enemySelectedLightTrue", 1);
			break;
		case (BattleStates.ENEMYCHOICE): //Enemy Finds & Follows Path
			Vector3 newCamPos1 = new Vector3 (thisEnemyUnit.transform.position.x, mainCamera.transform.position.y, thisEnemyUnit.transform.position.z 
                + (mainCamera.transform.position.y * (Mathf.Tan (mainCameraRotX))) / 2); //don't adjust this
			if (mainCamera.transform.position == newCamPos1 || enemyMoved == false) {
				if (thisEnemyUnit.Type == "ARCHER" && thisEnemyUnit.enemyArcherAttack.playerInSight == 1) {
					currentState = BattleStates.ENEMYATTACK;
				} else if (thisEnemyUnit.Type == "LIGHT INFANTRY" && thisEnemyUnit.enemyLightInfantryAttack.inCharge == 1
				       && thisEnemyUnit.enemyLightInfantryAttack.chargeCheck (playerUnit, thisEnemyUnit) == true) {
					currentState = BattleStates.ENEMYATTACK;
				} else if (thisEnemyUnit.Type == "SKELETON" && thisEnemyUnit.skeletonAttack.inCharge == 1
					&& thisEnemyUnit.skeletonAttack.chargeCheck (playerUnit, thisEnemyUnit) == true){
					currentState = BattleStates.ENEMYATTACK;
				} else {
					thisEnemyUnit.EnemyUnitMove1 ();
                    enemyMoved = true;
				}
			}
			//thisEnemyUnit.fatigue -= thisEnemyUnit.path.Length;
			break;
		case (BattleStates.ENEMYDECREASEFATIGUE): //Decrease Enemy Fatigue from Movement
            Vector3 newCamPos = new Vector3(thisEnemyUnit.transform.position.x, mainCamera.transform.position.y, 
				thisEnemyUnit.transform.position.z + (mainCamera.transform.position.y * (Mathf.Tan (mainCameraRotX)))*(mainCameraFollowY) -1);
			mainCamera.transform.parent = null;
			mainCamera.transform.position = Vector3.MoveTowards(transform.position, newCamPos, 1*Time.deltaTime); //May need to reenable this after testing
			if (i ==0){
				i = 1;
			}
			break;
		case (BattleStates.ENEMYATTACK): //Enemy chooses and executes Attack

			thisEnemyUnit = determineThisEnemyUnit ();
				//thisEnemyUnit.fatigue -= thisEnemyUnit.path.Length;
			TargetUnitStats = playerUnit.thisUnitStats;
			playerDamageText = TargetUnitStats.thisUnit.damageText;
			//Enemy Chooses Acc Pow or Def
            int a = determineAccPowDef (thisEnemyUnit);
			if (a == 0) {
				thisEnemyUnit.accuracy ();
			} else if (a == 1) {
				thisEnemyUnit.power ();
			} else if (a == 2) {
				thisEnemyUnit.defense ();
			}
			if (thisEnemyUnit.Type.Contains ("HEAVY INFANTRY")) {
				int x = ChooseAttack ();
				if (x == 0) {
					Invoke ("enemyWait", .25f);
					turnCounter += 1;
					attackLog.text = attackLog.text + "\n" + turnCounter.ToString ()
					+ ".  <color=red>" + thisEnemyUnit.thisEnemyStats.enemyName + "</color> uses Rest.";
				} else if (x == 1) {
					if (thisEnemyUnit.isMoving == 1) {
						thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
						Invoke ("enemyAttack", 1f);
						if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
							Invoke ("CounterAttackFunction", 1.5f);
						}
					} else if (thisEnemyUnit.isMoving == 0) {
						thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
						Invoke ("enemyAttack", 1.25f);
						if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
							Invoke ("CounterAttackFunction", 1.5f);
						}

					}
				} else if (x == 2) {
					if (thisEnemyUnit.isMoving == 1) {
						thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
						Invoke ("enemyPowerAttack", .5f);
						if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
							Invoke ("CounterAttackFunction", 1.5f);
						}
					} else if (thisEnemyUnit.isMoving == 0) {
						thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
						Invoke ("enemyPowerAttack", 1.25f);
						if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
							Invoke ("CounterAttackFunction", 1.5f);
						}
					} 
				}
			} else if (thisEnemyUnit.Type.Contains ("DUAL-SWORDS")) {
				int x = thisEnemyUnit.dualSwordAttack.dualChooseAttack ();
				if (x == 0) {
					float distanceX = enemy.position.x - playerUnit.transform.position.x;
					float distanceZ = enemy.position.z - playerUnit.transform.position.z;
					//	Invoke ("enemyAttack", .5f);
					//	if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0
					//	    && Mathf.Abs (distanceX) + Mathf.Abs (distanceZ) == 1) {
					//		Invoke ("CounterAttackFunction", 1);
					//	}
				} else if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.dualSwordAttack.WhirlwindAttack ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 3) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.dualSwordAttack.tripAttack ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0
					    && TargetUnitStats.tripped == 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				}
			} else if (thisEnemyUnit.Type.Contains ("MURMILLO")) {
				int x = thisEnemyUnit.enemySoldierAttack.EnemySoldierChooseAttack ();
				if (x == 0) {
					thisEnemyUnit.enemySoldierAttack.ActivateCounterAttack ();
				} else if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.enemySoldierAttack.shieldAttack ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 3) {
					thisEnemyUnit.enemyLightInfantryAttack.regenerate1 ();
				}
			} else if (thisEnemyUnit.Type.Contains ("ARCHER")) {
				int x = thisEnemyUnit.enemyArcherAttack.archerChooseAttack ();
				//	Debug.Log ("x" + x);
				if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.enemyArcherAttack.poisonShot ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 3) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.enemyArcherAttack.cripplingShot ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				}
			} else if (thisEnemyUnit.Type == "LIGHT INFANTRY") {
				int x = thisEnemyUnit.enemyLightInfantryAttack.lightChooseAttack ();
				if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.enemyLightInfantryAttack.chargeAttack1 ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 3) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.enemyLightInfantryAttack.breakDefense1 ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 4) {
					thisEnemyUnit.enemyLightInfantryAttack.regenerate1 ();
				}
			} else if (thisEnemyUnit.Type == "SKELETON") {
				int x = thisEnemyUnit.skeletonAttack.skeletonChooseAttack ();
				if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.dualSwordAttack.WhirlwindAttack ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 3) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.skeletonAttack.chargeAttack1 ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				}  else if (x == 4) {
					thisEnemyUnit.skeletonAttack.regenerate1 ();
				}
			}

			else if (thisEnemyUnit.Type == "LEADER") {
				int x = thisEnemyUnit.enemyLeaderAttack.LeaderChooseAttack ();
				if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					//thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.enemyLeaderAttack.Rally1 ();
				} else if (x == 3) {
					thisEnemyUnit.enemyLeaderAttack.Reinforce1 ();
				}
			} else if (thisEnemyUnit.Type == "PEASANT") {
				thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
				Invoke ("enemyAttack", 1f);
				if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
					Invoke ("CounterAttackFunction", 1.5f);
				}
			} else if (thisEnemyUnit.Type == "BOAR") {
				int x = thisEnemyUnit.boarAttack.boarChooseAttack ();
				if (x == 1) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					Invoke ("enemyAttack", 1f);
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 2) {
					thisEnemyUnit.boarAttack.roar ();
				} else if (x == 3) {
					thisEnemyUnit.transform.LookAt (playerUnit.transform.position);
					thisEnemyUnit.boarAttack.goreAttack ();
					if (TargetUnitStats.counterAttackActivated < 3 && TargetUnitStats.health > 0) {
						Invoke ("CounterAttackFunction", 1.5f);
					}
				} else if (x == 0) {
					Invoke ("enemyWait", 0);
				}

			}

			if (thisEnemyUnit.isMoving == 1) {
				enemySelectedLightFalse ();
			} else if (thisEnemyUnit.isMoving == 0) {
				Invoke ("enemySelectedLightFalse", 2.0f);
			}
				currentState = BattleStates.ENEMYRECALCULATE;
				break;
			
		case (BattleStates.ENEMYRECALCULATE): //Enemy adds 1 fatigue
            Invoke ("Recalculate", 1);
			//foreach (Unit u in units) {
			//	if (u.gameObject.transform.eulerAngles.y >= 1 && u.gameObject.transform.eulerAngles.y <= 89){
			//		u.transform.rotation = Quaternion.Euler (0, 90f, 0);
			//	}
			//	else if (u.gameObject.transform.eulerAngles.y >= 91 && u.gameObject.transform.eulerAngles.y <= 179){
			//		u.transform.rotation = Quaternion.Euler (0, 180f, 0);
			//	}
			//	else if (u.gameObject.transform.eulerAngles.y >= 181 && u.gameObject.transform.eulerAngles.y < 269){
			//		u.transform.rotation = Quaternion.Euler (0, 270f, 0);
			//	}
			//	else if (u.gameObject.transform.eulerAngles.y > 271 && u.gameObject.transform.eulerAngles.y < 359){
			//		u.transform.rotation = Quaternion.Euler(0, 0f, 0);
			//	}
			//}
          //  mainCamera.transform.position = Vector3.MoveTowards(transform.position, currentMainCameraPos, 3 * Time.deltaTime);
			break;
		case(BattleStates.LOSE):
			WinText.gameObject.SetActive(true);
			WinText.text = "DEFEAT";
            StartCoroutine(AudioFadeOut.FadeOut(Music, 4));
            Invoke ("returnToStart", 3);
			break;
		case(BattleStates.WIN):
			WinText.gameObject.SetActive(true);
            //battleButtonLock += 1;
            RewardText.gameObject.SetActive(true);
            if (goldLock == 0)
            {
                int players = determineSurvivingUnits();
                int reward = (10 * EndTurn.turns + 50 * players + 50*Battles.EnemiesInFight);
                GoldScript.Gold += reward;
                RewardText.text = "Winnings: " + reward.ToString();
                StartCoroutine(AudioFadeOut.FadeOut(Music, 4));
                goldLock += 1;
            }
			Invoke ("goToNextLevel", 3); //Actually goes to Battle Menu
            currentState = BattleStates.START;
			break;
		}
	}


	void resetCamera(){
        if (turnSword.texture != turnSwordTexture1)
        {
            mainCamera.transform.position = Vector3.MoveTowards(transform.position, currentMainCameraPos, 9 * Time.deltaTime);
        }
	}

    int determineSurvivingUnits()
    {
        int i = 0;
        int x;
        for (x = 0; x < rosterManager.unitsInScene.Count; x++)
        {
            if(rosterManager.unitsInScene[x].health > 0)
            {
                i++;
            }
        }
        return i;
    }

	public void StartBattle(){
		if (currentState == BattleStates.START) {
			currentState = BattleStates.PLAYERCHOICEMOVE;
			startAreaMap.gameObject.SetActive (false);
			startButton.gameObject.SetActive(false);
			characterSelectionMenu.gameObject.SetActive(false);
			attackLog.gameObject.SetActive (true);
			attackLogPanel.gameObject.SetActive (true);
			Music.Play ();
			FxAudioSource.Play ();
			}
			
		foreach (Unit u in units) {
			u.isSelected = 0;
			}
	}

	int ChooseAttack(){
		float distanceX = enemy.position.x - playerUnit.transform.position.x;
		float distanceZ = enemy.position.z - playerUnit.transform.position.z;
		if (thisEnemyUnit.fatigue < EnemyStandardFCost || (Mathf.Abs (distanceX) + Mathf.Abs(distanceZ)) > 1) {
			return 0;
		}
		else if (TargetUnitStats.health <= thisEnemyUnit.thisEnemyStats.attack - TargetUnitStats.constitution) {
			return 1;
		}
		else if (thisEnemyUnit.fatigue >= EnemyPowerFCost && EnemyStats.chanceToHit > 60) {
			return 2;
		}
		else {
			return 1;
		}
	}

	void enemyAttack(){
        float distanceX = enemy.position.x - playerUnit.transform.position.x;
		float distanceZ = enemy.position.z - playerUnit.transform.position.z;
        if (thisEnemyUnit.Type == "ARCHER" && thisEnemyUnit.fatigue >= EnemyStandardFCost) {
            thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Basic Attack";
            thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction(); 
            thisEnemyUnit.thisEnemyStats.Anim.Play("Attack");
			Invoke ("followCam1", .33f);
			if (AccuracyTest (thisEnemyUnit.enemyArcherAttack.standardAttackAcc) == true) {
				DamageTest (standardAttackDam, thisEnemyUnit.thisEnemyStats);
			} 
			thisEnemyUnit.fatigue -= EnemyStandardFCost;
		}
		else if (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ) == 1 
		    && thisEnemyUnit.fatigue >= EnemyStandardFCost) {
            thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Basic Attack";
            thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction(); 
            thisEnemyUnit.thisEnemyStats.Anim.Play("Attack");
			if (AccuracyTest (standardAttackAcc) == true) {
				DamageTest (standardAttackDam, thisEnemyUnit.thisEnemyStats);
			} 
			thisEnemyUnit.fatigue -= EnemyStandardFCost;
			
		}
		Invoke ("clearText", 2.5f);
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
		Invoke ("stopFollowCam", 1.0f);
	}

	void enemyPowerAttack(){
        thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Power Attack";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction(); 
		float distanceX = enemy.position.x - playerUnit.transform.position.x;
		float distanceZ = enemy.position.z - playerUnit.transform.position.z;
		if (Mathf.Abs (distanceX) + Mathf.Abs (distanceZ) == 1 
		    && thisEnemyUnit.fatigue >= EnemyPowerFCost) {

			thisEnemyUnit.thisEnemyStats.Anim.Play("PowerAttack");
			if (AccuracyTest (powerAttackAcc) == true) {
				DamageTest (powerAttackDam, thisEnemyUnit.thisEnemyStats);
			} 
			thisEnemyUnit.fatigue -= EnemyPowerFCost;
			
		}
		Invoke ("clearText", 2.5f);
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

	void enemyWait(){
		thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Rest";
        thisEnemyUnit.thisEnemyStats.fadeDamageTextFunction();
		thisEnemyUnit.fatigue++;
        FxAudioSource.clip = restSound;
        FxAudioSource.Play();
		Invoke ("clearEnemyText", 1);
		BattleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYRECALCULATE;
	}

    public void invokeEnemyCounterAttack()
    {
        Invoke("enemyCounterAttack", 1.5f);        
        return;
    }

	void stopFollowCam(){
		StopCoroutine ("followCam");
	}

	void followCam1(){
		StartCoroutine ("followCam");
	}

	IEnumerator followCam(){
		yield return new WaitForSeconds(.25f); //archerShotAnimation
		Vector3 newCamPos = new Vector3(playerUnit.transform.position.x, mainCamera.transform.position.y, 
			playerUnit.transform.position.z + (mainCamera.transform.position.y 
				* (Mathf.Tan (mainCameraRotX)))*(mainCameraFollowY));
		//mainCamera.transform.position = newCamPos;
		while (true) {
			if (mainCamera.transform.position == newCamPos) {
				yield break;
			} else {
				mainCamera.transform.position = Vector3.MoveTowards (mainCamera.transform.position, newCamPos, 14 * Time.deltaTime);
			}
			yield return null;
		}
	}


    void enemyCounterAttack()
    {
        float distanceX = enemy.position.x - playerUnit.transform.position.x;
        float distanceZ = enemy.position.z - playerUnit.transform.position.z;
        if (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ) == 1 && thisEnemyUnit.thisEnemyStats.tripped == 0
			&& thisEnemyUnit.thisEnemyStats.health > 0)
        {
            foreach (Unit u in units)
            {
                if (u.isSelected == 1)
                {
                    TargetUnitStats = u.thisUnitStats;
                    playerDamageText = u.damageText;
                }
            }
            thisEnemyUnit.transform.LookAt(TargetUnitStats.transform);
            thisEnemyUnit.thisEnemyStats.Anim.Play("Attack");
            if (AccuracyTest(standardAttackAcc) == true)
            {
				DamageTest(standardAttackDam - 1, thisEnemyUnit.thisEnemyStats);
            }
            thisEnemyUnit.thisEnemyStats.CounterAttackActivated++;
            thisEnemyUnit.fatigue -= thisEnemyUnit.enemySoldierAttack.CounterAttackFCost;
            Invoke("clearText", 1);
        }        
    }

	public bool AccuracyTest(float AttackType){
		if (EnemyStats.chanceToHit > 100) {
			EnemyStats.chanceToHit =100;
		}
		if (EnemyStats.chanceToHit < 0) {
			EnemyStats.chanceToHit = 0;
		}
	//	Debug.Log (EnemyStats.chanceToHit);
		int temp = Random.Range (1, 100);
	//	Debug.Log (temp);
		if (TargetUnitStats.tripped != 0) {
			EnemyStats.chanceToHit = 100;
		}
		if (temp <= EnemyStats.chanceToHit + AttackType) {
			Invoke ("GetHit", .25f);
			return true;
		} 
		else {
			playerUnit.transform.LookAt (thisEnemyUnit.transform);
			Invoke ("Block", .25f);
			Color c = new Color(0, 0 ,255);
			playerDamageText.color = c;
            StartCoroutine (displayMiss());
			//playerDamageText.text = "Miss!";
			turnCounter += 1;
			attackLog.text = attackLog.text + "\n" + turnCounter.ToString() +". <color=blue>" + playerUnit.thisUnitStats.unitName 
				+ " </color>blocks <color=red>" + thisEnemyUnit.thisEnemyStats.enemyName + "</color>'s attack.";
			attackLogScrollyBar.value = 0;
			return false;
		}
	}

    IEnumerator displayMiss()
    {
        yield return new WaitForSeconds(0.75f);
        playerDamageText.text = "Miss!";
    }

	void GetHit(){
		playerUnit.thisUnitStats.Anim.Play ("GetHit");
		FxAudioSource.clip = Hit;
		FxAudioSource.Play ();
	}

	void Block(){
		playerUnit.thisUnitStats.Anim.Play ("Block");
		FxAudioSource.clip = Miss;
		FxAudioSource.Play ();
	}

	public void DamageTest (int AttackType, EnemyStats enemyStats){
		int damageDealt = enemyStats.attack - TargetUnitStats.constitution + AttackType;
		TargetUnitStats.health -= damageDealt;
		TargetUnitStats.SetHealthBar ();
        StartCoroutine (displayDamage(damageDealt)); 
        //playerDamageText.text = damageDealt.ToString();
		turnCounter += 1;
		attackLogScrollyBar.value = 0;
		attackLog.text = attackLog.text + "\n" +turnCounter.ToString() + ". <color=red>" + thisEnemyUnit.thisEnemyStats.enemyName 
			+ " </color>hits <color=blue>" + TargetUnitStats.unitName + " </color>for " + damageDealt.ToString () + " damage.";
		attackLogScrollyBar.value = 0;
	}

    IEnumerator displayDamage(int i)
    {
        yield return new WaitForSeconds(0.75f);
        playerDamageText.text = i.ToString();
    }

	void CounterAttackFunction(){
        ifPlayerCounterAttacked = 1;
        StartCoroutine("CounterAttackFunction1");
	}

	IEnumerator CounterAttackFunction1(){
        yield return new WaitForSeconds (1);
        float distanceX = enemy.position.x - playerUnit.transform.position.x;
        float distanceZ = enemy.position.z - playerUnit.transform.position.z;
        int damageDealt = (TargetUnitStats.attack - thisEnemyUnit.thisEnemyStats.constitution) - 1;
		int x = Random.Range (1, 100);
		int z = Mathf.RoundToInt (TargetUnitStats.accuracy / thisEnemyUnit.thisEnemyStats.defense * 100);
	//	Debug.Log (x + "CounterAttack");
	//	Debug.Log (z);
		if (TargetUnitStats.health > 0
            && Mathf.Abs(distanceX) + Mathf.Abs(distanceZ) == 1
			&& TargetUnitStats.tripped == 0) {
			TargetUnitStats.transform.LookAt (thisEnemyUnit.transform);
            Color g = new Color(0, 255, 0);
            TargetUnitStats.thisUnit.damageText.color = g;
            TargetUnitStats.thisUnit.damageText.text = "Counter Attack";
            TargetUnitStats.thisUnit.fadeDamageTextFunction();
			TargetUnitStats.Anim.Play("Attack");
			if (x <= z) {
				thisEnemyUnit.thisEnemyStats.health -= damageDealt;
                Color r = new Color(255, 0, 0);
                thisEnemyUnit.thisEnemyStats.enemyDamageText.color = r;
                thisEnemyUnit.thisEnemyStats.enemyDamageText.text = damageDealt.ToString ();
				thisEnemyUnit.thisEnemyStats.SetHealthBar();
				FxAudioSource.clip = Hit;
				FxAudioSource.Play ();
				turnCounter += 1;
				attackLogScrollyBar.value = 0;
				attackLog.text = attackLog.text + " <color=blue>" + TargetUnitStats.unitName + "</color> counters for " 
					+ damageDealt.ToString() + " damage.";
				attackLogScrollyBar.value = 0;
			} else {
				Color c = new Color (0, 0, 255);
				thisEnemyUnit.thisEnemyStats.enemyDamageText.color = c;
				thisEnemyUnit.thisEnemyStats.enemyDamageText.text = "Miss!";
				FxAudioSource.clip = Miss;
				FxAudioSource.Play ();
			}
			Invoke ("clearEnemyText", 1);
			TargetUnitStats.counterAttackActivated++;
			playerUnit.fatigue -= 1;
		}
	}

	void Recalculate(){
		if (hasRecalculated == 0) {
			foreach (EnemyUnit e in enemyUnits){
			e.fatigue += 1;
			if (e.fatigue < e.maxMoveRange/2){
				e.moveRange = e.fatigue * 2 + 1;
			}
	
			//if (e.fatigue >= e.moveRange/2){
			//	enemyMoveRangeText.text = (e.moveRange/2).ToString();
			//}
			//else if (e.fatigue < e.moveRange/2){
			//	enemyMoveRangeText.text = e.fatigue.ToString();
			//}
			hasRecalculated ++;
			}
		}
		foreach (EnemyUnit e in enemyUnits) {
			e.isSelected = 0;
			//e.mined = false;
			if (e.gameObject.transform.eulerAngles.y >= 1 && e.gameObject.transform.eulerAngles.y <= 89){
				e.transform.rotation = Quaternion.Euler (0, 90f, 0);
			}
			else if (e.gameObject.transform.eulerAngles.y >= 91 && e.gameObject.transform.eulerAngles.y <= 179){
				e.transform.rotation = Quaternion.Euler (0, 180f, 0);
			}
			else if (e.gameObject.transform.eulerAngles.y >= 181 && e.gameObject.transform.eulerAngles.y < 269){
				e.transform.rotation = Quaternion.Euler (0, 270f, 0);
			}
			else if (e.gameObject.transform.eulerAngles.y > 271 && e.gameObject.transform.eulerAngles.y < 359){
				e.transform.rotation = Quaternion.Euler(0, 0f, 0);
			}
		}
		foreach (Unit u in units) {
			u.isSelected = 0;
			if (u.thisUnitStats.tripped == 3) {
				u.thisUnitStats.tripped = 4;
			}
		}
		i = 0;
        StartCoroutine (changeColor());
		EnemyTarget.chargeCheck = false;
		currentState = BattleStateMachine.BattleStates.PLAYERCHOICEMOVE;
	}

    IEnumerator changeColor()
    {
        yield return new WaitForSeconds(5);
        foreach (EnemyUnit e in enemyUnits)
        {
            Color r = new Color(255, 0, 0);
            e.thisEnemyStats.enemyDamageText.color = r;
            e.thisEnemyStats.enemyDamageText.text = null;
        }
    }

    IEnumerator changeColor2()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (EnemyUnit e in enemyUnits)
        {
            Color g = new Color(0, 255, 0);
            e.thisEnemyStats.enemyDamageText.color = g;
        }
    }

	public void clearText(){
		playerDamageText.color = d;
		playerDamageText.text = null;
	}

	void clearEnemyText(){
		thisEnemyUnit.thisEnemyStats.enemyDamageText.text = null;
		thisEnemyUnit.thisEnemyStats.enemyDamageText.color = d;
	}

	EnemyUnit determineThisEnemyUnit(){
		EnemyUnit selectedEnemyUnit = null;
		foreach (EnemyUnit e in enemyUnits) {
			if (e.isSelected == 1){
				selectedEnemyUnit = e;
			}
		}
		return selectedEnemyUnit;
	}

	int enemyTrippedContingency(){
		int i = 0;
		int x = 0;
		foreach (EnemyUnit e in enemyUnits) {
			if (e.health > 0) {
				i++;
			}
		}
		if (i == 1) {
			foreach (EnemyUnit e in enemyUnits) {
				if (e.health > 0 && e.thisEnemyStats.tripped != 0) {
					x++;
				}
			}
		}
		return x;
	}

    IEnumerator resetCamera2()
    {
        if (ifPlayerCounterAttacked == 1){
            yield return new WaitForSeconds(3.0f);
        }
        else{
            yield return new WaitForSeconds(1.0f);
        }
        mainCamera.transform.position = Vector3.MoveTowards(transform.position, currentMainCameraPos, 4 * Time.deltaTime);
    }

	IEnumerator enemySelectedLightTrue(){
		thisEnemyUnit.selectedLight.gameObject.SetActive (true);
		Vector3 newCamPos = new Vector3(thisEnemyUnit.transform.position.x, mainCamera.transform.position.y, thisEnemyUnit.transform.position.z + (mainCamera.transform.position.y * (Mathf.Tan (mainCameraRotX)))/2);
      //  if (enemyMoved == true) || thisEnemyUnit.Type == "ARCHER")
      //  {
            while (mainCamera.transform.position != newCamPos)
            {
                mainCamera.transform.position = Vector3.MoveTowards(transform.position, newCamPos, 9 * Time.deltaTime);
                yield return null;
            }
       // }    
		yield return new WaitForSeconds (2);
		//yield return null;
	}

    IEnumerator enemySelectedLightTrue1()
    {
        thisEnemyUnit.selectedLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        //yield return null;
    }

	void enemySelectedLightFalse(){
		thisEnemyUnit.selectedLight.gameObject.SetActive (false);
	}

	int determineAccPowDef (EnemyUnit selectedEnemyUnit){
		int x = 0;
		if (selectedEnemyUnit.thisEnemyStats.health <= TargetUnitStats.attack - selectedEnemyUnit.thisEnemyStats.constitution) {
			x = 2;
		} else if (selectedEnemyUnit.thisEnemyStats.accuracy / TargetUnitStats.defense * 100 >= 100) {
			x = 1;
		}
		return x;
	}
		

	void returnToStart(){
       Application.LoadLevel(battleMenuLoad);
      
	}

	void goToNextLevel(){
		int x = Application.loadedLevel;
        if (battleNumbersInt == battleButtonLock)
        {
            battleButtonLock += 1;
        }
       // int players = determineSurvivingUnits();
        //int reward = (10 * EndTurn.turns + 50 * players + 50 * Battles.EnemiesInFight);
        //GoldScript.Gold += reward;
        //RewardText.text = "Winnings: " + reward.ToString();
		//Application.LoadLevel (x + 1); Switch to this function to have levels go in order
        if (currentBattle == BattleNumbers.BATTLE14)
        {
            Application.LoadLevel(21);
        }
        else
        {
            Application.LoadLevel(battleMenuLoad + 1);
        }
	}

    public void playButtonSource()
    {
        ButtonSource.Play();
    }
		
}
