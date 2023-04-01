using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FirstLoadScreenScript : MonoBehaviour {

	public int levelToLoad;


	// Use this for initialization
	void Start () {
		StartCoroutine (LoadLevel());
	}
	
	// Update is called once per frame


	IEnumerator LoadLevel(){
      //  yield return new WaitForSeconds(2); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (levelToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null; 
        }
	}
}
