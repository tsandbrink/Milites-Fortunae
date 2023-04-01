using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionsPanel : MonoBehaviour {

	public RawImage descriptionPanel;
	public Text TypeText;
	public Text enemyTypeText;
	public Text inPanelTypeText;
	public Text descriptionText;
	public RawImage inPanelImage;
	public Texture[] characterPortraits;


	// Use this for initialization
	void Start () {
		descriptionPanel.gameObject.SetActive (false);
	}
	
	// Update is called once per frame


	public void closePanel(){
		descriptionPanel.gameObject.SetActive (false);
	}

	public void openPanelPlayer(){
		descriptionPanel.gameObject.SetActive (true);
		if (TypeText.text == "MURMILLO") {
			inPanelTypeText.text = "MURMILLO";
			descriptionText.text = "Well-armored fighters capable of taking many hits. Accurate though not particularly strong attackers. " +
				"Best utilized as tanks. Shield attack can be used to force tired opponents out of range.";
			inPanelImage.texture = characterPortraits [0];
		}

		if (TypeText.text == "WARRIOR") {
			inPanelTypeText.text = "WARRIOR";
			inPanelImage.texture = characterPortraits [1];
			descriptionText.text = "Heavy damage, low accuracy brawlers. Use other units to weaken opponent defenses " +
			"and then strike with power attack.";
		}

		if (TypeText.text == "DUAL-SWORDS") {
			inPanelTypeText.text = "DUAL-SWORDS";
			inPanelImage.texture = characterPortraits [2];
			descriptionText.text = "Aggressive attackers, but their lack of shield leaves them extremely vulnerable. Trip is effective for " +
			"disabling strong defenders.";
		}

		if (TypeText.text == "ARCHER") {
			inPanelTypeText.text = "ARCHER";
			inPanelImage.texture = characterPortraits [3];
			descriptionText.text = "Ranged units with high accuracy but lack of armor means they cannot take many hits. " +
			"Must have a clear line of sight on target to perform attacks."; 
		}

		if (TypeText.text == "LIGHT INFANTRY") {
			inPanelTypeText.text = "LIGHT INFANTRY";
			inPanelImage.texture = characterPortraits[4];
			descriptionText.text = "Shock troops capable of striking quickly and heavily, " +
				"but low constitution means they are easily defeated if they remain engaged too long. " +
				"Charge attack is most effective as part of a hit and run strategy.";
		}

		if (TypeText.text == "SARACENE") {
			inPanelTypeText.text = "SARACENE";
			inPanelImage.texture = characterPortraits [5];
			descriptionText.text = "Support units but not terribly sturdy in close combat. " +
				"Look to use Mine opportunistically. Effective when paired with a MURMILLO.";
		}

		if (TypeText.text == "BOAR") {
			inPanelTypeText.text = "BOAR";
			inPanelImage.texture = characterPortraits [6];
			descriptionText.text = "Wild boars. High stamina and thick skin means they can survive many rounds." +
				" Don't try to take down alone.";
		}

		if (TypeText.text == "HEAVY INFANTRY") {
			inPanelTypeText.text = "HEAVY INFANTRY";
			inPanelImage.texture = characterPortraits [7];
			descriptionText.text = "Balanced fighters capable of striking heavily " +
				"or can use rest ability to outlast low-stamina opponents.";
		}

		if (TypeText.text == "AMAZON") {
			inPanelTypeText.text = "AMAZON";
			inPanelImage.texture = characterPortraits [8];
			descriptionText.text = "Powerful archers who use volley to shoot over obstacles.";
		}

		if (TypeText.text == "PEASANT") {
			inPanelTypeText.text = "PEASANT";
			inPanelImage.texture = characterPortraits [9];
			descriptionText.text = "Poorly trained militia, capable of only basic attacks.";
		}

		if (TypeText.text == "FIELD MEDIC") {
			inPanelTypeText.text = "FIELD MEDIC";
			inPanelImage.texture = characterPortraits [10];
			descriptionText.text = "Support units who attend to injured or tired units. " +
				"Use rally to get a unit with depleted health or stamina out of danger.";
		}

		if (TypeText.text == "LEADER") {
			inPanelTypeText.text = "LEADER";
			inPanelImage.texture = characterPortraits [11];
			descriptionText.text = "Commander of an enemy force. Will often use 'rally' to keep self well protected. ";
		}
	}

	public void openPanelEnemy(){
		descriptionPanel.gameObject.SetActive (true);
		if (enemyTypeText.text == "MURMILLO") {
			inPanelTypeText.text = "MURMILLO";
			descriptionText.text = "Well-armored fighters capable of taking many hits. Accurate though not particularly strong attackers. " +
				"Best utilized as tanks. Shield attack can be used to force tired opponents out of range.";
			inPanelImage.texture = characterPortraits [0];
		}

		if (enemyTypeText.text == "WARRIOR") {
			inPanelTypeText.text = "WARRIOR";
			inPanelImage.texture = characterPortraits [1];
			descriptionText.text = "Heavy damage, low accuracy brawlers. Use other units to weaken opponent defenses " +
				"and then strike with power attack.";
		}

		if (enemyTypeText.text == "DUAL-SWORDS") {
			inPanelTypeText.text = "DUAL-SWORDS";
			inPanelImage.texture = characterPortraits [2];
			descriptionText.text = "Aggressive attackers, but their lack of shield leaves them extremely vulnerable. Trip is effective at " +
				"disabling strong defenders.";
		}

		if (enemyTypeText.text == "ARCHER") {
			inPanelTypeText.text = "ARCHER";
			inPanelImage.texture = characterPortraits [3];
			descriptionText.text = "Ranged units with high accuracy but lack of armor means they cannot take many hits. " +
				"Must have a clear line of sight on target to perform attacks."; 
		}

		if (enemyTypeText.text == "LIGHT INFANTRY") {
			inPanelTypeText.text = "LIGHT INFANTRY";
			inPanelImage.texture = characterPortraits[4];
			descriptionText.text = "Shock troops capable of striking quickly and heavily, " +
			"but low constitution means they are ineffective if they remain engaged. " +
			"Charge attack is effective as part of a hit and run strategy.";
		}

		if (enemyTypeText.text == "SARACENE") {
			inPanelTypeText.text = "SARACENE";
			inPanelImage.texture = characterPortraits [5];
			descriptionText.text = "Support units but not terribly sturdy in close combat. " +
				"Look to use Mine opportunistically. Effective when paired with a MURMILLO.";
		}

		if (enemyTypeText.text == "BOAR") {
			inPanelTypeText.text = "BOAR";
			inPanelImage.texture = characterPortraits [6];
			descriptionText.text = "Wild boars. High stamina and thick skin means they can survive many rounds." +
				" Don't try to take down alone.";
		}

		if (enemyTypeText.text == "HEAVY INFANTRY") {
			inPanelTypeText.text = "HEAVY INFANTRY";
			inPanelImage.texture = characterPortraits [7];
			descriptionText.text = "Balanced fighters capable of striking heavily " +
				"or can use rest ability to outlast low-stamina opponents.";
		}

		if (enemyTypeText.text == "AMAZON") {
			inPanelTypeText.text = "AMAZON";
			inPanelImage.texture = characterPortraits [8];
			descriptionText.text = "Powerful archers who use volley to shoot over obstacles.";
		}

		if (enemyTypeText.text == "PEASANT") {
			inPanelTypeText.text = "PEASANT";
			inPanelImage.texture = characterPortraits [9];
			descriptionText.text = "Poorly trained militia, capable of only basic attacks.";
		}

		if (enemyTypeText.text == "FIELD MEDIC") {
			inPanelTypeText.text = "FIELD MEDIC";
			inPanelImage.texture = characterPortraits [10];
			descriptionText.text = "Support units who attend to injured or tired units. " +
				"Use rally to get a unit with depleted health or stamina out of danger.";
		}

		if (enemyTypeText.text == "LEADER") {
			inPanelTypeText.text = "LEADER";
			inPanelImage.texture = characterPortraits [11];
			descriptionText.text = "Commander of an enemy force. Will often use 'rally' to keep self well protected. ";
		}
	}
}
