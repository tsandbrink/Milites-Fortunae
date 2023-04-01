using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackDescriptions : MonoBehaviour {

	Text thisTextObject;
	public Text descriptionText;
	public Text modifiersText;

	// Use this for initialization
	void Start () {
		thisTextObject = gameObject.GetComponent<Text> ();
		clearText ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void displayDescription(){
		if (thisTextObject.text == "BASIC ATTACK") {
			descriptionText.text = "Standard low cost attack";
			modifiersText.text = null;
		}

		//Heavy Infantry
		if (thisTextObject.text == "POWER ATTACK") {
			descriptionText.text = "Swing harder sacrificing accuracy for greater damage.";
			modifiersText.text = "+1 Dam, -10 Acc";
		}

		if (thisTextObject.text == "REST") {
			descriptionText.text = "Regain 2 stamina instead of 1 at the end of turn.";
			modifiersText.text = null;
		}

		//Light Infantry
		if (thisTextObject.text == "CHARGE") {
			descriptionText.text = "Charge in a straight line at far-away enemeies with chance to knock down";
			modifiersText.text = "5 Range, +4 Dam";
		}

		if (thisTextObject.text == "SLOW") {
			descriptionText.text = "Low damage attack that reduces opponent's defenses";
			modifiersText.text = "-1 Dam";
		}
			
		//Archer
		if (thisTextObject.text == "STANDARD SHOT") {
			descriptionText.text = "Standard low cost Attack";
			modifiersText.text = "6 Range";
		}

		if (thisTextObject.text == "POISON ARROW") {
			descriptionText.text = "Reduces both Stamina and Health of Target";
			modifiersText.text = "-5 Acc, 5 Range";
		}

		if (thisTextObject.text == "LOWER DEFENSE") {
			descriptionText.text = "Low-Power Attack that reduces target's ability to resist damage and defend attacks";
			modifiersText.text = "-5 Acc, 5 Range";
		}

		//Murmillo
		if (thisTextObject.text == "SHIELD ATTACK") {
			descriptionText.text = "Knock opponent backwards 1 sqaure";
			modifiersText.text = "-10 Acc";
		}

		if (thisTextObject.text == "COUNTER ATTACK") {
			descriptionText.text = "Perform a counter attack the next 2 times this unit gets attacked";
			modifiersText.text = null;
		}

		//Dual Swords
		if (thisTextObject.text == "TRIP") {
			descriptionText.text = "Low-power attack aimed at opponent's legs with chance to set knock-down status";
			modifiersText.text = "-1 Dam";
		}

		if (thisTextObject.text == "WHIRLWIND") {
			descriptionText.text = "Spinning attack that hits multiple opponents";
			modifiersText.text = null;
		}

		//Boar
		if (thisTextObject.text == "ROAR") {
			descriptionText.text = "Intimidate opponents causing them to retreat one square";
			modifiersText.text = null;
		}

		if (thisTextObject.text == "GORE") {
			descriptionText.text = "Knock opponent back one square with chance to knock down";
			modifiersText.text = null;
		}
			
		//Leader
		if (thisTextObject.text == "RALLY") {
			descriptionText.text = "Return another Ally of Choice to a square adjacent to Leader, " +
				"restoring 5 stamina, buffing its accuracy and defense";
			modifiersText.text = null;
		}

		if (thisTextObject.text == "REINFORCE") {
			descriptionText.text = "Calls in another Unit to join the battle.";
			modifiersText.text = null;
		}
	}

	public void clearText(){
		descriptionText.text = null;
		modifiersText.text = null;
	}
}
