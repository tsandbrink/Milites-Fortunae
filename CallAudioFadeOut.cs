using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAudioFadeOut : MonoBehaviour {

	AudioSource Sound1;
	public float fadetime;

	void Start(){
		Sound1 = KeepMusicPlaying.thisAudioSource;
		StartCoroutine(AudioFadeOut.FadeOut(Sound1, fadetime));
	}
}
