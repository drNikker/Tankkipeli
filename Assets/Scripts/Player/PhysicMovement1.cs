﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PhysicMovement1 : MonoBehaviour
{
    // XINPUT STUFF
    public PlayerIndex playerIndex;
    GamePadState state;

    //OTHER
    private GameObject player;
    private FullRagdollMode ragdollmode;
    private CharacterJoint characterJoint;

    private SoftJointLimit lowTwistLimitOriginal;
    private SoftJointLimit highTwistLimitOriginal;

    private Rigidbody rb;
    public Vector3 com;
    CharacterUpright charUpR;
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

    float smooth = 50.0f;
    float tiltAngle = 10.0f;

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
        
        state = GamePad.GetState(playerIndex);

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
        StuckPreventer();
    }

    void KeyPress()
    {
        // setup triggers as buttons
        //bool RT = Input.GetAxis(pelaaja + "TankThreadRight") > 0.0;
        //bool LT = Input.GetAxis(pelaaja + "TankThreadLeft") > 0.0;
        
        //Tread speed increase

        if ((Input.GetKey(KeyCode.Keypad9) || state.Buttons.RightShoulder == ButtonState.Pressed) && !(Input.GetKey(KeyCode.Keypad6) || state.Triggers.Right > 0.0))
        {
            rightTread += accel * Time.deltaTime;
            brakeRight = false;
        }
        if ((Input.GetKey(KeyCode.Keypad7) || state.Buttons.LeftShoulder == ButtonState.Pressed) && !(Input.GetKey(KeyCode.Keypad4) || state.Triggers.Left > 0.0))
        {
            leftTread += accel * Time.deltaTime;
            brakeLeft = false;
        }
        
        if ((Input.GetKey(KeyCode.Keypad6) || state.Triggers.Right > 0.0) && !(Input.GetKey(KeyCode.Keypad9) || state.Buttons.RightShoulder == ButtonState.Pressed))
        {
            rightTread -= accel * Time.deltaTime;
            brakeRight = false;
        }
        if ((Input.GetKey(KeyCode.Keypad4) || state.Triggers.Left > 0.0) && !(Input.GetKey(KeyCode.Keypad7) || state.Buttons.LeftShoulder == ButtonState.Pressed))
        {
            leftTread -= accel * Time.deltaTime;
            brakeLeft = false;
        }

        //clamping tread speeds
        rightTread = Mathf.Clamp(rightTread, -topSpeed, topSpeed);
        leftTread = Mathf.Clamp(leftTread, -topSpeed, topSpeed);

        //set motortorque to 0 wwhen no input is given
        if (!(Input.GetKey(KeyCode.Keypad9) || state.Buttons.RightShoulder == ButtonState.Pressed) && !(Input.GetKey(KeyCode.Keypad6) || state.Triggers.Right > 0.0))
        {
            rightTread = 0;
            brakeRight = true;
        }

        if (!(Input.GetKey(KeyCode.Keypad7) || state.Buttons.LeftShoulder == ButtonState.Pressed) && !(Input.GetKey(KeyCode.Keypad4) || state.Triggers.Left > 0.0))
        {
            leftTread = 0;
            brakeLeft = true;
        }

        //Dizzy 
        if (rightTread >= topSpeed && leftTread <= -topSpeed || leftTread >= topSpeed && rightTread <= -topSpeed)
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

    void StuckPreventer()
    {
        if (player.transform.rotation.z > 1 )
        {
            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
            float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

            Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        }

    }
}
