using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TrainingScript : MonoBehaviour
{
    public GameObject trainingMenu;
    public int healthBoostCost;
    public int staminaBoostCost;
    public int moveBoostCost;

    public Button healthButton;
    public Button staminaButton;
    public Button moveButton;

    public GoldScript goldScript;

    public Button openTrainingMenuButton;

    public Text boostText;
    public Text trainingMenuButtonText;

    // Start is called before the first frame update
    void Start()
    {
        trainingMenu.SetActive(false);
        if (GoldScript.Gold < staminaBoostCost)
        {
            openTrainingMenuButton.interactable = false;
        }
        if (GoldScript.Gold < staminaBoostCost)
        {
            trainingMenuButtonText.text = "You Need More Gold!";
        }
        else
        {
            trainingMenuButtonText.text = " ";
        }
    }

    void Update()
    {
        if (GoldScript.Gold < healthBoostCost)
        {
            healthButton.interactable = false;
        }
        if (GoldScript.Gold < staminaBoostCost)
        {
            staminaButton.interactable = false;
        }
        if (GoldScript.Gold < moveBoostCost)
        {
            moveButton.interactable = false;
        }
    }
   
    public void OpenTrainingMenu()
    {
        trainingMenu.SetActive(true);
        if (GoldScript.Gold < healthBoostCost)
        {
            healthButton.interactable = false;
        }
        if (GoldScript.Gold < staminaBoostCost)
        {
            staminaButton.interactable = false;
        }
        if (GoldScript.Gold < moveBoostCost)
        {
            moveButton.interactable = false;
        }
        goldScript.goldText2.text = "Gold: " + GoldScript.Gold.ToString();
    }

    public void CloseTrainingMenu()
    {
        trainingMenu.SetActive(false);
    }

    public void HealthBoostFunction()
    {
        BattleStateMachine.healthBoost += 1;
        GoldScript.Gold -= healthBoostCost;
        goldScript.goldText.text = "Gold: " + GoldScript.Gold.ToString();
        goldScript.goldText2.text = "Gold: " + GoldScript.Gold.ToString();
        boostText.text = "+1 Health";
        fadeboostTextFunction();
    }

    public void StaminaBoostFunction()
    {
        BattleStateMachine.staminaBoost += 1;
        GoldScript.Gold -= staminaBoostCost;
        goldScript.goldText.text = "Gold: " + GoldScript.Gold.ToString();
        goldScript.goldText2.text = "Gold: " + GoldScript.Gold.ToString();
        boostText.text = "+1 Stamina";
        fadeboostTextFunction();
    }

    public void MoveBoostFunction()
    {
        BattleStateMachine.moveRangeBoost += 2;
        GoldScript.Gold -= moveBoostCost;
        goldScript.goldText.text = "Gold: " + GoldScript.Gold.ToString();
        goldScript.goldText2.text = "Gold: " + GoldScript.Gold.ToString();
        boostText.text = "+1 Move Range";
        fadeboostTextFunction();
    }

    public IEnumerator fadeBoostText(float fadeTime, Text boostText)
    {
        boostText.color = new Color(boostText.color.r, boostText.color.g, boostText.color.b, 1);
        while (boostText.color.a > 0.0f)
        {
            boostText.color = new Color(boostText.color.r, boostText.color.g, boostText.color.b, boostText.color.a - Time.deltaTime / fadeTime);
            if (boostText.color.a == 0.0f)
            {
                boostText.text = " ";
                Color c = new Color(255, 0, 0, 255);
                boostText.color = c;
            }
            yield return null;
        }
    }

    public void fadeboostTextFunction()
    {
        StartCoroutine(fadeBoostText(3.5f, boostText));
    }
}
