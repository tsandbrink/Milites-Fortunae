using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    public Text saveText;

    public void Save()
    {
        PlayerPrefs.SetInt("battleButtonLock", BattleStateMachine.battleButtonLock);
        PlayerPrefs.SetInt("Gold", GoldScript.Gold);
        PlayerPrefs.SetInt("healthBoost", BattleStateMachine.healthBoost);
        PlayerPrefs.SetInt("staminaBoost", BattleStateMachine.staminaBoost);
        PlayerPrefs.SetInt("moveRangeBoost", BattleStateMachine.moveRangeBoost);
        PlayerPrefs.Save();
        saveText.text = "Game Saved!";
        StartCoroutine(fadeText(2.0f, saveText));
    }

    public void Load()
    {
        BattleStateMachine.battleButtonLock = PlayerPrefs.GetInt("battleButtonLock");
        GoldScript.Gold = PlayerPrefs.GetInt("Gold");
        BattleStateMachine.healthBoost = PlayerPrefs.GetInt("healthBoost");
        BattleStateMachine.staminaBoost = PlayerPrefs.GetInt("staminaBoost");
        BattleStateMachine.moveRangeBoost = PlayerPrefs.GetInt("moveRangeBoost");
        Application.LoadLevel(1);
    }

    public void NewGame()
    {
        BattleStateMachine.battleButtonLock = 0;
        GoldScript.Gold = 0;
        BattleStateMachine.healthBoost = 0;
        BattleStateMachine.staminaBoost = 0;
        BattleStateMachine.moveRangeBoost = 0;
    }

    public IEnumerator fadeText(float f, Text textToFade)
    {
        textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 1);
        while (textToFade.color.a > 0.0f)
        {
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, textToFade.color.a - Time.deltaTime / f);
            if (textToFade.color.a == 0.0f)
            {
                textToFade.text = " ";
                Color c = new Color(255, 0, 0, 255);
                textToFade.color = c;
            }
            yield return null;
        }
    }
}
