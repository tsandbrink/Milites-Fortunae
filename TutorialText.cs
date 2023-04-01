using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialText : MonoBehaviour {
	
	public Text speechbox;
	public Image speechBubble;

	string[] textArray;
	string text0;
	string text1;
	string text2;
	string text3;
	string text4;
	string text5;
	string text6;
	string text7;
	string text8;
	string text9;
	string text10;
	string text11;
	string text12;
	string text13;
	string text14;
	string text15;
	string text16;
	string text17;
	string text18;
	string text19;
	string text20;
	string text21;
	string text22;
	string text23;
	string text24;
	string text25;
	string text26;
	string text27;
	string text28;
	int x;
	

	// Use this for initialization
	void Start () {
		x = 0;
		if (Battles.currentBattle == Battles.BattlesEnum.One) {
			ActivateSpeech ();
		}
		List<string> texts = new List<string> ();
		//All buttons not interactable
		text0 = "Adressing the captives, the overseer begins: ";
		text1 = "Here, you will learn to fight, turning from prisoners into warriors of the arena. Today we will learn about moving and attacks.";
		text2 = "First you must select which fighter you wish to use in the battle. " +
		"You're available fighters are listed in the menu at the bottom of the screen.";
		text3 = "To have a fighter join the fight, simply click on his name.";
		//For this fight Volund and Sergius; make their buttons interactable and circle
		text4 = "For this fight we will use Volund and Sergius. Click on their name and they will appear in the arena.";
		text5 = "The number of units that can be used in a battle will vary and is shown at the top of the character selection menu.";
		text6 = "Once you have chosen your units, click the 'begin fight' button to start the battle.";
		text7 = "The battles are turn based. On each turn you and your opponent may select one fighter to perform an action. " +
		"A unit may move and perform one of its abilities in a turn. However, it cannot perform an ability and then move.";
		text8 = "You will always have the first turn. To get started let's have Volund perform an action. To select him, " +
		"move the red 'selection square' over him and left-click."; //have text skip automatically; make others non-selectable
		text9 = "Once a unit is selected he may now move or perform abilities." +
			"To have him move, simply place the red 'selection square' over the square you wish to move to and left click once again. " +
			"Go ahead and have Volund move to the highlighted square.";
		//make a square highlight and don't let player move anywhere else
		text10 = "Note, the maximum number of squares a character may move is displayed on the left panel."; //highlight/circle it
		text11 = "Furthermore, each square a character moves consumes 1 stamina. " +
		"Because we moved Volund four squares, he has used four stamina."; //highlight/circle it
		text12 = "Characters with low stamina will be unable to move as many squares because they are out of energy. " +
		"For example, a character with only 2 stamina remaining can only move 2 squares.";
		text13 = "At the end of your turn all the units fighting on your side will regain 1 stamina, and when your opponent completes its turn its units recover 1 stamina.";
		text14 = "Monitoring your own and opposing units' stamina is often the key to winning a fight."; // highlight them
		text15 = "Because Volund is not close enough to any opposing units, he cannot use any of his abilities so we have to end our turn and allow our opponent to have its turn. " +
			"To do this, click the 'End Turn' Button."; // highlight/circle it
		text16 = "Now that the opponent has made its move, let's focus on using abilities to attack opponents.";
		text17 = "To start select Sergius by moving the 'selection square' over him and left-clicking."; //make others non interactable
		text18 = "Now move him to the highlighted square."; //highlight a square
		text19 = "Now that we've moved Sergius we can use his abilities. Once a character is within attacking range of an opponent, his abilities, " +
			"listed on the left panel will turn white. Melee-type fighters usually have to be adjacent to an opposing unit to use their abilities." +
			"The number next to each ability shows how much stamina performing that ability will consume. " +
			"More powerful attacks require more stamina while the minimum amount of stamina needed to perform an attack-ability is 2"; //highlight
		text20 = "Let's use Sergius's BASIC ATTACK ability. To select an ability simply click on its name in the panel on the left-hand side of the screen."; //highlight & make text switch
		text21 = "Selecting an ability that allows a unit to attack an opponent causes the orange attack range graphic to appear. " +
		"Any opponent who is standing within the orange area may be targeted with the attack.";
		text22 = "To target an opposing unit, move the red 'selection square' over it. " +
		"This causes the % chance of the attack succeeding and the amount of damage it will do to appear on the left hand panel." + //circle it
		"Once an opponent is targeted, execute the attack by left-clicking. Go ahead and click on Vittius to have Sergius do a basic attack against him.";
		text23 = "Ordering a unit to perform an ability automatically ends your turn. If an attack suceeds, the amount of health damage will be displayed above the targeted opponent.";
		text24 = "Whenever a unit's health is reduced to 0 it is defeated and is removed from the battle. " +
			"Usually to win a battle you will need to defeat every opposing unit.";
		text25 = "You can check a specific opponent's health, stamina, move range, and abilities by moving the red 'selection square' over that unit. " +
		"This information will be displayed in the right-panel.";
		text26 = "Let's finish off Vittius by using Volund's lunge attack. First, use the selection cube to select Volund."; //auto switch
		text27 = "Now select 'Lunge Attack' from the abilities menu on the left panel. " +
		"This ability has a slightly better range than other melee-attacks."; //auto switch
		text28 = "Now, target Vittius by hovering over him and execute the attack by left-clicking.";
		texts.Add (text0);
		texts.Add (text1);
		texts.Add (text2);
		texts.Add (text3);
		texts.Add (text4);
		texts.Add (text5);
		texts.Add (text6);
		texts.Add (text7);
		texts.Add (text8);
		texts.Add (text9);
		texts.Add (text10);
		texts.Add (text11);
		texts.Add (text12);
		texts.Add (text13);
		texts.Add (text14);
		texts.Add (text15);
		texts.Add (text16);
		texts.Add (text17);
		texts.Add (text18);
		texts.Add (text19);
		texts.Add (text20);
		texts.Add (text21);
		texts.Add (text22);
		texts.Add (text23);
		texts.Add (text24);
		texts.Add (text25);
		texts.Add (text26);
		texts.Add (text27);
		texts.Add (text28);
		//texts.Insert (0, text0);
		//texts.Insert (1, text1);
		speechbox.text = text0;
		textArray = texts.ToArray ();
	}
	
	// Update is called once per frame

	public void nextText(){
		speechbox.text = textArray [x + 1];
		x += 1;
	}
			
	
	void ActivateSpeech(){
		speechbox.gameObject.SetActive (true);
		speechBubble.gameObject.SetActive (true);
	}
}
