using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hideAttackLog : MonoBehaviour
{
    public Image attackLog;
    public Button showAttackLogButton;
    // Start is called before the first frame update
   

    // Update is called once per frame
   

    public void HideAttackLog()
    {
        attackLog.gameObject.SetActive(false);
        showAttackLogButton.gameObject.SetActive(true);
    }

    public void ShowAttackLog()
    {
        attackLog.gameObject.SetActive(true);
        showAttackLogButton.gameObject.SetActive(false);
    }
}
