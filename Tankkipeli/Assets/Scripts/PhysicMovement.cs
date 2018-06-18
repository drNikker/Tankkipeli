﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMovement : MonoBehaviour {

    Rigidbody rb;
    public GameObject left;
    //public GameObject left2;
    public GameObject right;
    //public GameObject right2;

    ////wheel colliders
    //public WheelCollider leftWC1;
    //public WheelCollider leftWC2;
    //public WheelCollider rightWC1;
    //public WheelCollider rightWC2;


    public float speed;
    public float turn;

    float rightThrust;
    float leftThrust;

    public string player;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        //leftWC1 = left.GetComponent<WheelCollider>();
        //leftWC2 = left2.GetComponent<WheelCollider>();
        //rightWC1 = right.GetComponent<WheelCollider>();
        //rightWC2 = right2.GetComponent<WheelCollider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        KeyPresses();

    }

    
    void KeyPresses()
    {
        bool RT = Input.GetAxis(player + "TankThreadRight") > 0.0;
        bool LT = Input.GetAxis(player + "TankThreadLeft") > 0.0;

        rightThrust = 0;
        leftThrust = 0;

        //rightWC1.motorTorque = 0;
        //rightWC2.motorTorque = 0;
        //leftWC1.motorTorque = 0;
        //leftWC2.motorTorque = 0;


        if (Input.GetKey(KeyCode.Keypad9) || Input.GetButton(player + "RB"))
        {
            rightThrust += 1;
            //rightWC1.motorTorque = 50;
            //rightWC2.motorTorque = 50;
        }
        if (Input.GetKey(KeyCode.Keypad7) || Input.GetButton(player + "LB"))
        {
            leftThrust += 1;
            //leftWC1.motorTorque = 50;
            //leftWC2.motorTorque = 50;
        }
        if (Input.GetKey(KeyCode.Keypad6) || RT)
        {
            rightThrust -= 1;
            //rightWC1.motorTorque = -50;
            //rightWC2.motorTorque = -50;
        }
        if (Input.GetKey(KeyCode.Keypad4) || LT)
        {
            leftThrust -= 1;
            //leftWC1.motorTorque = -50;
            //leftWC2.motorTorque = -50;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = rb.transform.position - transform.position;
        rb.AddForceAtPosition(left.transform.forward * speed * leftThrust, left.transform.position);
        rb.AddForceAtPosition(right.transform.forward * speed * rightThrust, right.transform.position);

        //rightWC1.motorTorque = rightThrust;
        //rightWC2.motorTorque = rightThrust;
        //leftWC1.motorTorque = leftThrust;
        //leftWC2.motorTorque = leftThrust;

    }

   /* void Turn()
    {
        rb.AddTorque(transform.up * turnForce * turn);
    }*/
    
}
