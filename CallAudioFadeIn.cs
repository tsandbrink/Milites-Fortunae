using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAudioFadeIn : MonoBehaviour {

	AudioSource Sound1;
	public float fadetime;

	void Start(){
		Sound1 = KeepMusicPlaying.thisAudioSource;
		StartCoroutine(AudioFadeOut.FadeIn(Sound1, fadetime));
	}
}
