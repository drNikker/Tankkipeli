using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour {
	public float speed;
	public Transform hips;
	public Rigidbody controller;
	public Animator animtr;

	public HingeJoint Z_lowerArmL;
	public HingeJoint Z_lowerArmR;

	public HingeJoint Z_handL;
	public HingeJoint Z_handR;

	private JointLimits z_hl;
	private JointLimits z_hr;

	public ConfigurableJoint pivotJoint;
	public ConfigurableJoint capsuleController;

	public RotateBox rb_script;

	// Use this for initialization
	void Start () {
		z_hl = Z_handL.limits;
		z_hr = Z_handR.limits;

		Z_handL.limits = z_hl;
		Z_handR.limits = z_hr;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10f;
		//var z = Input.GetAxis("Vertical") * Time.deltaTime * 10f;

		//transform.Rotate(0, x, 0);
		//transform.Translate(x, 0, 0);
		//transform.Translate(0, 0, z);

		if (Input.GetKey (KeyCode.W)) 
		{	
			Z_lowerArmL.useSpring = false;
			Z_lowerArmR.useSpring = false;
			animtr.SetBool ("isWalking", true);
			//controller.AddForce (hips.TransformDirection (Vector3.forward * speed * Time.deltaTime));
		} 
		else {
				animtr.SetBool ("isWalking", false);
				Z_lowerArmL.useSpring = true;
				Z_lowerArmR.useSpring = true;
		   	 }
		if (Input.GetKey (KeyCode.S)) 
		{
			//controller.AddForce (hips.TransformDirection (-Vector3.forward * speed * Time.deltaTime));
		}

		if (Input.GetKey (KeyCode.F)) {

			Z_handL.limits = z_hl;
			Z_handR.limits = z_hr;

			z_hl.min = 0;
			z_hl.max = 0;
			z_hr.min = 0;
			z_hr.max = 0;
			animtr.SetBool ("isMitsThrown", true);
		} else {animtr.SetBool ("isMitsThrown", false);}

		if (Input.GetKey (KeyCode.L)) {
			animtr.SetBool ("punchLeft", true);
		} 
		else {animtr.SetBool ("punchLeft", false);}

		if (Input.GetKey (KeyCode.R)) {
			animtr.SetBool ("punchRight", true);
		} else {animtr.SetBool ("punchRight", false);}

		if (Input.GetKey (KeyCode.Z)) {
			animtr.SetBool ("kickLeft", true);
		} else {animtr.SetBool ("kickLeft", false);}

		if (Input.GetKey (KeyCode.X)) {
			animtr.SetBool ("kickRight", true);
		} else {animtr.SetBool ("kickRight", false);}

		if (Input.GetKey (KeyCode.Slash)) 
		{
			animtr.enabled = false;
			rb_script.enabled = false;
			Destroy (pivotJoint);
			Destroy (capsuleController);
		}

	}
}
