using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMovement : MonoBehaviour {

    //Rigidbody rb;
    public GameObject left;
    //public GameObject left2;
    public GameObject right;
    //public GameObject right2;

    //wheel colliders
    public WheelCollider leftWC1;
    public WheelCollider leftWC2;
    public WheelCollider leftWC3;
    public WheelCollider leftWC4;
    public WheelCollider rightWC1;
    public WheelCollider rightWC2;
    public WheelCollider rightWC3;
    public WheelCollider rightWC4;

    public float maxSpeed;
    public float acceleration;
    public float turn;
    public float brakeForce;
    
    float brakeTorq;
    
    [SerializeField]
    float rightThrust;
    float leftThrust;


    public string player;

	// Use this for initialization
	void Start () {
        //rb = GetComponent<Rigidbody>();
        
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

        
        brakeTorq = 0;
        //rightWC1.motorTorque = 0;
        //rightWC2.motorTorque = 0;
        //leftWC1.motorTorque = 0;
        //leftWC2.motorTorque = 0;


        if (Input.GetKey(KeyCode.Keypad9) || Input.GetButton(player + "RB"))
        {
            rightThrust += acceleration * Time.deltaTime;
            //rightWC1.motorTorque = 50;
            //rightWC2.motorTorque = 50;
        }
        if (Input.GetKey(KeyCode.Keypad7) || Input.GetButton(player + "LB"))
        {
            leftThrust += acceleration * Time.deltaTime;
            //leftWC1.motorTorque = 50;
            //leftWC2.motorTorque = 50;
        }
        if (Input.GetKey(KeyCode.Keypad6) || RT)
        {
            rightThrust -= acceleration * Time.deltaTime;
            //rightWC1.motorTorque = -50;
            //rightWC2.motorTorque = -50;
        }
        if (Input.GetKey(KeyCode.Keypad4) || LT)
        {
            leftThrust -= acceleration * Time.deltaTime;
            //leftWC1.motorTorque = -50;
            //leftWC2.motorTorque = -50;
        }

        if (!((Input.GetKey(KeyCode.Keypad9) || Input.GetButton(player + "RB")) || (Input.GetKey(KeyCode.Keypad6) || RT)))
        {
            rightThrust = 0;
        }
        if (!((Input.GetKey(KeyCode.Keypad7) || Input.GetButton(player + "LB")) || (Input.GetKey(KeyCode.Keypad4) || LT)))
        {
            leftThrust = 0;
        }

        rightThrust = Mathf.Clamp(rightThrust,-maxSpeed, maxSpeed);
        leftThrust = Mathf.Clamp(leftThrust, -maxSpeed, maxSpeed);

        //if ((Input.GetKey(KeyCode.Keypad9) || Input.GetButton(player + "RB")) || (Input.GetKey(KeyCode.Keypad7) || Input.GetButton(player + "LB")) || (Input.GetKey(KeyCode.Keypad6) || RT) || (Input.GetKey(KeyCode.Keypad4) || LT))
        //{
        //    brakeTorq = 1000;
        //}

        //@@@@@@@@@@@@@@@@@@@@@@@@@ BRAKE TORQ

        if (rightThrust == 0 || leftThrust == 0)
        {
            brakeTorq = brakeForce;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //Vector3 direction = rb.transform.position - transform.position;
        //rb.AddForceAtPosition(left.transform.forward * speed * leftThrust, left.transform.position);
        //rb.AddForceAtPosition(right.transform.forward * speed * rightThrust, right.transform.position);

        if (rightThrust == 0)
        {
            rightWC1.brakeTorque = brakeTorq;
            rightWC2.brakeTorque = brakeTorq;
            rightWC3.brakeTorque = brakeTorq;
            rightWC4.brakeTorque = brakeTorq;
        }
        else
        {
            rightWC1.brakeTorque = 0;
            rightWC2.brakeTorque = 0;
            rightWC3.brakeTorque = 0;
            rightWC4.brakeTorque = 0;
        }

        if (leftThrust == 0)
        {
            leftWC1.brakeTorque = brakeTorq;
            leftWC2.brakeTorque = brakeTorq;
            leftWC3.brakeTorque = brakeTorq;
            leftWC4.brakeTorque = brakeTorq;
        }
        else
        {
            leftWC1.brakeTorque = 0;
            leftWC2.brakeTorque = 0;
            leftWC3.brakeTorque = 0;
            leftWC4.brakeTorque = 0;
        }
        

        rightWC1.motorTorque = rightThrust;
        rightWC2.motorTorque = rightThrust;
        leftWC1.motorTorque = leftThrust;
        leftWC2.motorTorque = leftThrust;
        rightWC3.motorTorque = rightThrust;
        rightWC4.motorTorque = rightThrust;
        leftWC3.motorTorque = leftThrust;
        leftWC4.motorTorque = leftThrust;


        //rightWC1.brakeTorque = brakeTorq;
        //rightWC2.brakeTorque = brakeTorq;
        //leftWC1.brakeTorque = brakeTorq;
        //leftWC2.brakeTorque = brakeTorq;

    }
    
   /* void Turn()
    {
        rb.AddTorque(transform.up * turnForce * turn);
    }*/
    
}
