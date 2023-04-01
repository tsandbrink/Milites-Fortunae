using UnityEngine;
using System.Collections;

public class HealthBarFollowScript : MonoBehaviour {
	public Transform Parent;

	public bool checkThisForBoar = false;

	// Use this for initialization
	void Start () {
		if (checkThisForBoar == false) {
			Vector3 Offset = new Vector3 (Parent.position.x, Parent.position.y + .5f, Parent.position.z + .5f);
			gameObject.transform.position = Offset;
		} else {
			Vector3 Offset = new Vector3 (Parent.position.x, Parent.position.y + .1f, Parent.position.z + .5f);
			gameObject.transform.position = Offset;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (checkThisForBoar == false) {
			Vector3 Offset = new Vector3 (Parent.position.x, Parent.position.y + .5f, Parent.position.z + .5f);
			gameObject.transform.position = Offset;
		} else {
			Vector3 Offset = new Vector3 (Parent.position.x, Parent.position.y + .1f, Parent.position.z + .5f);
			gameObject.transform.position = Offset;
		}
	}
	
}
