using UnityEngine;
using System.Collections;

public class SetBattle : MonoBehaviour {
	//public GameObject RosterManager;


	public void BackToTitleScreen(){
		Application.LoadLevel (0);
	}

	//Arena
	public void setBattleOne(){
		Battles.currentBattle = Battles.BattlesEnum.One;
		Application.LoadLevel (2);
	}
	
	public void setBattleTwo(){
		Battles.currentBattle = Battles.BattlesEnum.Two;
		Application.LoadLevel (2);
	}

	public void setBattleThree(){
		Battles.currentBattle = Battles.BattlesEnum.Three;
		Application.LoadLevel (2);
	}

	public void setBattleFour(){
		Battles.currentBattle = Battles.BattlesEnum.Four;
		Application.LoadLevel (2);
	}

	//Escape
	public void setBattleFive(){
		Application.LoadLevel (5);
	}

	//FlashBack
	public void setBattleSixA(){
		Battles3.currentBattle = Battles3.BattlesEnum.One;
		Application.LoadLevel (9);
	}

	public void setBattleSixB(){
		Battles3.currentBattle = Battles3.BattlesEnum.Two;
		Application.LoadLevel (9);
	}

	//TownRaid
	public void setBattleSeven(){
		Application.LoadLevel (18);
	}

	//King's Stables
	public void setBattleEight(){
		Application.LoadLevel (13);
		Battles2.currentBattle = Battles2.BattlesEnum.One;
	}

	public void setBattleNine(){
		Application.LoadLevel (13);
		Battles2.currentBattle = Battles2.BattlesEnum.Two;
	}

	public void setBattleTen(){
		Application.LoadLevel (13);
		Battles2.currentBattle = Battles2.BattlesEnum.Three;
	}

	//Amazon
	public void setBattleEleven(){
		Application.LoadLevel (19);
	}

    //Siege
    public void setBattleTwelve()
    {
        Application.LoadLevel(20);
    }

	// Boar Hunt
	public void setBattleBoar(){
		Application.LoadLevel (7);
	}

	public void setBattleTest(){
		Application.LoadLevel (10);
		Battles2.currentBattle = Battles2.BattlesEnum.Three;
	}

	public void LoadLevelOneLoadScreen(){
		Application.LoadLevel (3);
	}
}
