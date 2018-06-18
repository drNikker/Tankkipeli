using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePosition_Debug : MonoBehaviour {
	public float MoveSpeed;
	private Rigidbody rb;
	public Vector3 MoveDirection;
	public Transform hips;

	// Use this for initialization

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}
	void Update () {

		//float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		//MoveDirection = (v * Vector3.forward + h * Vector3.right).normalized;
		MoveDirection = (v * hips.TransformDirection(Vector3.forward)).normalized;
	}

	void FixedUpdate()
	{
			rb.MovePosition (transform.position + MoveDirection * MoveSpeed * Time.deltaTime);
	}
}
