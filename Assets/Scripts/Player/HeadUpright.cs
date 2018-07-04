using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUpright : MonoBehaviour {

    new protected Rigidbody rigidbody;
    public float uprightForce = 10;
    public float uprightOffset = 1.45f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 40; // **** CANNOT APPLY HIGH ANGULAR FORCES UNLESS THE MAXANGULAR VELOCITY IS INCREASED ****
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        rigidbody.AddForceAtPosition(new Vector3(0, uprightForce, 0),
    transform.position , ForceMode.Force);
    }
}
