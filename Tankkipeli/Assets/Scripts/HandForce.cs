using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandForce : MonoBehaviour {

    public float thrust;
    public Rigidbody rb;

    public GameObject focus;

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
    }

}
