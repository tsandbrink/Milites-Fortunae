using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeepMusicPlaying : MonoBehaviour {

	public static AudioSource thisAudioSource;

	void Awake() {
		thisAudioSource = gameObject.GetComponent<AudioSource> ();
		DontDestroyOnLoad(transform.gameObject);
	}
		


	
}
