using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    float carAcceleration;
    float carSpeed;
    float swerweAmount;

    Vector3 destination;

    Rigidbody rb;

    //new
    public WheelCollider leftWheelCol1;
    public WheelCollider leftWheelCol2;
    public WheelCollider leftWheelCol3;
    public WheelCollider leftWheelCol4;
    public WheelCollider rightWheelCol1;
    public WheelCollider rightWheelCol2;
    public WheelCollider rightWheelCol3;
    public WheelCollider rightWheelCol4;

    public float topSpeed;
    public float accel;
    public float brakingForce;
    public float upRightCounter;

    float brakeTorqu;

    [SerializeField]
    float rightTread;
    [SerializeField]
    float leftTread;
    [SerializeField]
    float middleTread;
    public float wheelDamp;

    bool brakeRight = true;
    bool brakeLeft = true;
    bool brakeMiddle = true;

    public Vector3 com;

    // Use this for initialization
    void Start()
    {

        brakeTorqu = brakingForce;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;


        //Setting wheeldamping to each wheel because its broken otherwise
        leftWheelCol1.wheelDampingRate = wheelDamp;
        leftWheelCol2.wheelDampingRate = wheelDamp;
        leftWheelCol3.wheelDampingRate = wheelDamp;
        leftWheelCol4.wheelDampingRate = wheelDamp;
        rightWheelCol1.wheelDampingRate = wheelDamp;
        rightWheelCol2.wheelDampingRate = wheelDamp;
        rightWheelCol3.wheelDampingRate = wheelDamp;
        rightWheelCol4.wheelDampingRate = wheelDamp;

    }

    // Update is called once per frame
    void Update()
    {
        CarMove();
    }


    void CarMove()
    {
        // setup triggers as buttons
        //bool RT = Input.GetAxis(pelaaja + "TankThreadRight") > 0.0;
        //bool LT = Input.GetAxis(pelaaja + "TankThreadLeft") > 0.0;

        //Tread speed increase

            rightTread += accel * Time.deltaTime;
            brakeRight = false;

            leftTread += accel * Time.deltaTime;
            brakeLeft = false;

        //clamping tread speeds
        rightTread = Mathf.Clamp(rightTread, -topSpeed, topSpeed);
        leftTread = Mathf.Clamp(leftTread, -topSpeed, topSpeed);
        middleTread = Mathf.Clamp(middleTread, -topSpeed, topSpeed);



    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {

        //Apply speed
        rightWheelCol1.motorTorque = rightTread;
        rightWheelCol2.motorTorque = rightTread;
        leftWheelCol1.motorTorque = leftTread;
        leftWheelCol2.motorTorque = leftTread;
        rightWheelCol3.motorTorque = rightTread;
        rightWheelCol4.motorTorque = rightTread;
        leftWheelCol3.motorTorque = leftTread;
        leftWheelCol4.motorTorque = leftTread;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }
}
