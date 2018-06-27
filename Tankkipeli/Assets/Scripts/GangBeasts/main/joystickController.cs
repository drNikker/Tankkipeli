using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickController : MonoBehaviour {

	public Rigidbody torsoRB;
	public Transform torsoTM;

	public float speed;
	//Vector3 jumpSpeed;

	float dash;

	// Use this for initialization
	void Start () {
		torsoRB = GetComponent<Rigidbody>();
		dash = 10.0f;
		//dash = new Vector3 (0.0f, 0.0f, 100.0f);
		//jumpSpeed = new Vector3(0.0f, 485.0f, 0.0f);
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetAxis ("Vertical") > 0.3) {
			torsoRB.AddForce (transform.forward * dash, ForceMode.Impulse);
		}

		if (Input.GetAxis ("Vertical") < -0.3) {
			torsoRB.AddForce (-transform.forward * dash, ForceMode.Impulse);
		}
		/*if (Input.GetKey (KeyCode.W)) {
			torsoRB.AddForce (transform.forward * dash, ForceMode.Impulse);
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			torsoRB.AddForce (torsoTM.TransformDirection (Vector3.forward * speed * Time.fixedDeltaTime));
		}

		if (Input.GetKey (KeyCode.S)) {
			torsoRB.AddForce (torsoTM.TransformDirection (-Vector3.forward * speed * Time.fixedDeltaTime));
		}*/

		/*if (Input.GetKeyDown (KeyCode.Space)) {
			torsoRB.AddForce (jumpSpeed, ForceMode.Impulse);*/
		}
	}
