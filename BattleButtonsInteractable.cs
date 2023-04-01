using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleButtonsInteractable : MonoBehaviour
{
    public Button[] BattleButtons;
    public AudioSource buttonNoise;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Button b in BattleButtons)
        {
            int positionInArray = System.Array.IndexOf(BattleButtons, b);
            if (positionInArray <= BattleStateMachine.battleButtonLock)
            {
                b.interactable = true;
            }
            else
            {
                b.interactable = false;
            }
        }
    }

    public void playButtonSound()
    {
        buttonNoise.Play();
    }

    public void BattleButton1()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE1;
    }

    public void BattleButton2()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE2;
    }

    public void BattleButton3()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE3;
    }

    public void BattleButton4()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE4;
    }

    public void BattleButton5()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE5;
    }

    public void BattleButton6()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE6; 
    }

    public void BattleButton7()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE7;
    }

    public void BattleButton8()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE8;
    }

    public void BattleButton9()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE9;
    }

    public void BattleButton10()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE10;
    }

    public void BattleButton11()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE11;
    }

    public void BattleButton12()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE12;
    }

    public void BattleButton13()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE13;
    }

    public void BattleButton14()
    {
        BattleStateMachine.currentBattle = BattleStateMachine.BattleNumbers.BATTLE14;
    }
}
