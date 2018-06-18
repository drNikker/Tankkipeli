using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScript : MonoBehaviour {

	Animator animtr;
	bool TriggerWlking;
	bool TriggerLeftKick;
	bool TriggerRightKick;

	public GameObject arm_R;
	public GameObject arm_R_z;


	// Use this for initialization
	void Start () {
		animtr = gameObject.GetComponent<Animator> ();
		TriggerWlking = false;
		TriggerLeftKick = false;
		TriggerRightKick = false;

	}

	void LateUpdate () {

		// Walk Cycle.
		if (Input.GetKey (KeyCode.W)) {
			animtr.SetFloat ("speed", 1f);
		} else {animtr.SetFloat ("speed", 0f);}


		if (Input.GetAxis("Vertical") > 0.3) 
			TriggerWlking = true;

		else TriggerWlking = false;

		if(TriggerWlking == false)
				animtr.SetBool("isRunning", false);

		if(TriggerWlking == true)
				animtr.SetBool("isRunning", true);

		// Kick Left.

		if (Input.GetButton ("P1_X")) 
			TriggerLeftKick = true;

		else TriggerLeftKick = false;

		if(TriggerLeftKick == false)
			animtr.SetBool("leftKick", false);

		if(TriggerLeftKick == true)
			animtr.SetBool("leftKick", true);


		// Kick Right.

		if (Input.GetButton ("P1_B")) 
			TriggerRightKick = true;

		else TriggerRightKick = false;

		if(TriggerRightKick == false)
			animtr.SetBool("rightKick", false);

		if(TriggerRightKick == true)
			animtr.SetBool("rightKick", true);	
	
	}
}