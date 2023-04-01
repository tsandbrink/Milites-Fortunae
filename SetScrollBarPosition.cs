using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]

public class SetScrollBarPosition : MonoBehaviour {

	[Range(0f, 1f)]
	public float scrollStart;

	void Start () 
	{
		GetComponent<Scrollbar>().value = scrollStart;
	}


}
