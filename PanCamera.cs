using UnityEngine;
using System.Collections;

public class PanCamera : MonoBehaviour {

	public Transform cameraAnchor;
	public Transform forwardTarget;
	public Transform backwardTarget;
	public Transform rightTarget;
	public Transform leftTarget;
	public Transform zoomOutTarget;
	public Transform zoomInTarget;
	public float speed;
	public int forwardposition = 5;
	public int backposition = 2;
	public float rightposition = 5;
	public float leftposition = 5;
	public int zoomOutPosition =5;
	public int zoomInPosition = 2;

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("w")
		    && transform.position.z > cameraAnchor.position.z -forwardposition) {
			transform.position = Vector3.MoveTowards(transform.position, forwardTarget.position, speed*Time.deltaTime);
		
		}   
		if (Input.GetKey ("s")
			&& transform.position.z < cameraAnchor.position.z +backposition) {
			transform.position = Vector3.MoveTowards(transform.position, backwardTarget.position, speed*Time.deltaTime);
		}

		if (Input.GetKey ("d")
		    && transform.position.x > cameraAnchor.position.x -rightposition) {
			transform.position = Vector3.MoveTowards(transform.position, rightTarget.position, speed*Time.deltaTime);
			
		}   
		if (Input.GetKey ("a")
			&& transform.position.x < cameraAnchor.position.x + leftposition) {
			transform.position = Vector3.MoveTowards(transform.position, leftTarget.position, speed*Time.deltaTime);
		}

		if (Input.GetAxis ("Mouse ScrollWheel")< 0
			&& transform.position.y < cameraAnchor.position.y +zoomOutPosition){
			transform.position = Vector3.MoveTowards(transform.position, zoomOutTarget.position, 5*speed*Time.deltaTime);
		}

		if (Input.GetAxis ("Mouse ScrollWheel")> 0
			&& transform.position.y > cameraAnchor.position.y -zoomInPosition){
			transform.position = Vector3.MoveTowards(transform.position, zoomInTarget.position, 5*speed*Time.deltaTime);
		}
			
	}
}
