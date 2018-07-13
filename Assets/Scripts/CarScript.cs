using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    float carAcceleration;
    float carSpeed;
    float swerweAmount;


    Vector3 destination;

	// Use this for initialization
	void Start ()
    {
        carAcceleration = Random.Range(0.1f,1);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * Time.deltaTime * carSpeed;
        carSpeed += carAcceleration;

    }
}
