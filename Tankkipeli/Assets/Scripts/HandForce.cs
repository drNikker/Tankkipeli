using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandForce : MonoBehaviour {

    public float thrust;
    public Rigidbody rb;

    public GameObject focus;

    Vector3 p1LeftHand;
    Vector3 p1RightHand;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(focus.transform);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) && this.name == "handControllerL")
        {
            rb.AddForce(transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.E) && this.name == "handControllerR")
        {
            rb.AddForce(transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.A) && this.name == "handControllerL")
        {
            rb.AddForce(transform.forward * thrust * -1);
        }
        if (Input.GetKey(KeyCode.D) && this.name == "handControllerR")
        {
            rb.AddForce(transform.forward * thrust * -1);
        }

        p1LeftHand = Vector3.zero;
        p1RightHand = Vector3.zero;

        p1LeftHand.x = Input.GetAxis("P1LeftHandX");
        p1LeftHand.y = Input.GetAxis("P1LeftHandY");

        
        p1RightHand.x = Input.GetAxis("P1RightHandX");
        p1RightHand.y = Input.GetAxis("P1RightHandY");


        if ((p1LeftHand.x != 0 || p1LeftHand.y != 0) && this.name == "handControllerL")
        {
            rb.AddForce(p1LeftHand * thrust);
        }

        if ((p1RightHand.x != 0 || p1RightHand.y != 0) && this.name == "handControllerR")
        {
            rb.AddForce(p1RightHand * thrust);
        }
        Debug.Log(Input.GetAxis("P1LeftHandX") + " " + Input.GetAxis("P1LeftHandY"));
        //rb.AddForce(p1RightHand * thrust);
        

    }

}
