using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMovement1 : MonoBehaviour
{
    private GameObject player;
    private FullRagdollMode ragdollmode;
    private CharacterJoint characterJoint;

    private SoftJointLimit lowTwistLimitOriginal;
    private SoftJointLimit highTwistLimitOriginal;

    private Rigidbody rb;
    public Vector3 com;
    CharacterUpright charUpR;
    Quaternion upRight;
    RaycastHit downRightRay;
    PlayerHealth health;

    //wheel colliders
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
    public float wheelDamp;

    bool brakeRight = true;
    bool brakeLeft = true;

    public string pelaaja;

    private bool timerUntilDizzy;
    public float timerUntilDizzyTime = 3;
    private float originalTimerUntilDizzyTime;

    private bool backToNormalTimer;
    public float backToNormalTimerTime = 3;
    private float originalBackToNormalTimerTime;

    public bool canMove;

    // Use this for initialization
    void Start()
    {
        player = gameObject;
        ragdollmode = player.GetComponentInChildren<FullRagdollMode>();
        characterJoint = player.GetComponentInChildren<CharacterJoint>();
        health = GetComponent<PlayerHealth>();

        lowTwistLimitOriginal = characterJoint.lowTwistLimit;
        highTwistLimitOriginal = characterJoint.highTwistLimit;
        originalTimerUntilDizzyTime = timerUntilDizzyTime;
        originalBackToNormalTimerTime = backToNormalTimerTime;

        canMove = true;

        brakeTorqu = brakingForce;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
        upRight = Quaternion.Euler(0, 0, 0);
        charUpR = GetComponent<CharacterUpright>();

        charUpR.keepUpright = false;

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
        if (canMove)
        {
            KeyPress();
        }

        if (timerUntilDizzy)
        {
            DizzyTimer();
        }

        if (backToNormalTimer)
        {
            BackToNormalTimer();
        }
    }

    void KeyPress()
    {
        // setup triggers as buttons
        bool RT = Input.GetAxis(pelaaja + "TankThreadRight") > 0.0;
        bool LT = Input.GetAxis(pelaaja + "TankThreadLeft") > 0.0;
        // Tread Speed Increases

        if ((Input.GetKey(KeyCode.Keypad9) || Input.GetButton(pelaaja + "RB")) && !(Input.GetKey(KeyCode.Keypad6) || RT))
        {
            rightTread += accel * Time.deltaTime;
            brakeRight = false;
        }
        if ((Input.GetKey(KeyCode.Keypad7) || Input.GetButton(pelaaja + "LB")) && !(Input.GetKey(KeyCode.Keypad4) || LT))
        {
            leftTread += accel * Time.deltaTime;
            brakeLeft = false;
        }
        if ((Input.GetKey(KeyCode.Keypad6) || RT) && !(Input.GetKey(KeyCode.Keypad9) || Input.GetButton(pelaaja + "RB")))
        {
            rightTread -= accel * Time.deltaTime;
            brakeRight = false;
        }
        if ((Input.GetKey(KeyCode.Keypad4) || LT) && !(Input.GetKey(KeyCode.Keypad7) || Input.GetButton(pelaaja + "LB")))
        {
            leftTread -= accel * Time.deltaTime;
            brakeLeft = false;
        }

        rightTread = Mathf.Clamp(rightTread, -topSpeed, topSpeed);
        leftTread = Mathf.Clamp(leftTread, -topSpeed, topSpeed);


        if (!(Input.GetKey(KeyCode.Keypad9) || Input.GetButton(pelaaja + "RB")) && !(Input.GetKey(KeyCode.Keypad6) || RT))
        {
            rightTread = 0;
            brakeRight = true;
        }

        if (!(Input.GetKey(KeyCode.Keypad7) || Input.GetButton(pelaaja + "LB")) && !(Input.GetKey(KeyCode.Keypad4) || LT))
        {
            leftTread = 0;
            brakeLeft = true;
        }


        if (rightTread >= topSpeed && leftTread <= -topSpeed || leftTread >= topSpeed && rightTread <= -topSpeed || rightTread >= topSpeed && leftTread <= -topSpeed || leftTread >= topSpeed && rightTread <= -topSpeed)
        {
            timerUntilDizzy = true;
        }
        else
        {
            timerUntilDizzyTime += Time.deltaTime;
            timerUntilDizzy = false;

            if (timerUntilDizzyTime >= originalTimerUntilDizzyTime)
            {
                timerUntilDizzyTime = originalTimerUntilDizzyTime;
            }
        }
    }

    private void FixedUpdate()
    {
        Movement();
        TurnUpRight();
    }

    void Movement()
    {
        //Set braking

        if (brakeRight)
        {
            rightWheelCol1.brakeTorque = brakeTorqu;
            rightWheelCol2.brakeTorque = brakeTorqu;
            rightWheelCol3.brakeTorque = brakeTorqu;
            rightWheelCol4.brakeTorque = brakeTorqu;
        }
        else
        {
            rightWheelCol1.brakeTorque = 0;
            rightWheelCol2.brakeTorque = 0;
            rightWheelCol3.brakeTorque = 0;
            rightWheelCol4.brakeTorque = 0;
        }

        if (brakeLeft)
        {
            leftWheelCol1.brakeTorque = brakeTorqu;
            leftWheelCol2.brakeTorque = brakeTorqu;
            leftWheelCol3.brakeTorque = brakeTorqu;
            leftWheelCol4.brakeTorque = brakeTorqu;
        }
        else
        {
            leftWheelCol1.brakeTorque = 0;
            leftWheelCol2.brakeTorque = 0;
            leftWheelCol3.brakeTorque = 0;
            leftWheelCol4.brakeTorque = 0;
        }

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

    void TurnUpRight()
    {
        Physics.Raycast(transform.localPosition, Vector3.down, out downRightRay, 3);
        //Debug.DrawRay(transform.localPosition, Vector3.down, Color.red,3);
        if (rightWheelCol1.isGrounded == false && rightWheelCol2.isGrounded == false && leftWheelCol1.isGrounded == false && leftWheelCol2.isGrounded == false && downRightRay.collider != null)
        {
            upRightCounter += Time.deltaTime;
            if (upRightCounter >= 4)
            {
                charUpR.keepUpright = true;
                upRightCounter = 0;
            }
        }
        else
        {
            upRightCounter = 0;
            charUpR.keepUpright = false;
        }
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

    void onMovingPlatforms()
    {

    }

    private void DizzyTimer()
    {
        timerUntilDizzyTime -= Time.deltaTime;

        if (timerUntilDizzyTime <= 0)
        {
            ragdollmode.RagdollMode();
            health.currentState = PlayerHealth.PLAYER_STATE.STUNNED;
            health.SetPlayerState();
            canMove = false;
            rightTread = 0;
            leftTread = 0;

            timerUntilDizzyTime = originalTimerUntilDizzyTime;

            backToNormalTimer = true;
            timerUntilDizzy = false;
        }
    }

    private void BackToNormalTimer()
    {
        backToNormalTimerTime -= Time.deltaTime;

        if (backToNormalTimerTime <= 0)
        {
            characterJoint.lowTwistLimit = lowTwistLimitOriginal;
            characterJoint.highTwistLimit = highTwistLimitOriginal;
            backToNormalTimerTime = originalBackToNormalTimerTime;
            health.currentState = PlayerHealth.PLAYER_STATE.ALIVE;
            health.SetPlayerState();

            canMove = true;
            backToNormalTimer = false;
        }
    }
}
