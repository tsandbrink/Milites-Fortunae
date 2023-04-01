using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LoadingAnimation : MonoBehaviour
{
    public Text loadingText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (playLoadingAnimation());
    }

    // Update is called once per frame
    IEnumerator playLoadingAnimation()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (i == 1)
            {
                loadingText.text = "Loading.";
            }
            else if (i == 2)
            {
                loadingText.text = "Loading..";
            }
            else if (i == 3)
            {
                loadingText.text = "Loading...";
                i = 0;
            }
            yield return new WaitForSeconds(.5f);
        }
    }
    
   void loading1dot()
    {
        loadingText.text = "Loading.";
    }

    void loading2dot()
    {
        loadingText.text = "Loading..";
    }

    void loading3dot(){
        loadingText.text = "Loading...";
    }
}
