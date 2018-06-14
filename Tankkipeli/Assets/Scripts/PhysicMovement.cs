using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMovement : MonoBehaviour {

    Rigidbody rb;
    public GameObject left;
    public GameObject right;

    public float speed;
    public float turn;

    float rightThrust;
    float leftThrust;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        KeyPresses();

    }

    void KeyPresses()
    {
        rightThrust = 0;
        leftThrust = 0;

        if (Input.GetKey(KeyCode.Keypad9))
        {
            rightThrust += 1;
        }
        if (Input.GetKey(KeyCode.Keypad7))
        {
            leftThrust += 1;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            rightThrust -= 1;
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            leftThrust -= 1;
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
    }

   /* void Turn()
    {
        rb.AddTorque(transform.up * turnForce * turn);
    }*/
    
}
