using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    float carAcceleration;
    float carSpeed;
    float swerweAmount;

    public Vector3 destination;

    Rigidbody rb;


    private void Start()
    {
        carAcceleration = Random.Range(0,5);
    }

    private void Update()
    {
        rb.AddForce((destination - transform.position) * carAcceleration);
    }
}
