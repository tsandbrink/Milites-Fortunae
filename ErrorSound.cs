using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorSound : MonoBehaviour
{

    public static AudioSource errorSource;
    // Start is called before the first frame update
    void Start()
    {
        errorSource = gameObject.GetComponent<AudioSource>();
        errorSource.volume = .8f;
    }

}
