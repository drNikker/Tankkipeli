using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using XInputDotNetPure;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    // XINPUT STUFF
    //public PlayerIndex playerIndex;


    //OTHER
    NavMeshAgent agent;
    private GameObject player;
    private FullRagdollMode ragdollmode;
    private CharacterJoint characterJoint;
    private MenuSelection menuSel;

    private SoftJointLimit lowTwistLimitOriginal;
    private SoftJointLimit highTwistLimitOriginal;

    private Rigidbody rb;
    public Vector3 com; // center of mass
    private CharacterUpright charUpR;
    private PlayerHealth health;

    public PlayerStateEffect playerStateEffect;
    public PlayerIconScript playerIcon;
    public TankTextureSpeed tankTextureSpeed;


    public WhirlEffect whirlEffect;

    //wheel colliders
    public WheelCollider leftWheelCol1;
    public WheelCollider leftWheelCol2;
    public WheelCollider leftWheelCol3;
    public WheelCollider leftWheelCol4;
    public WheelCollider rightWheelCol1;
    public WheelCollider rightWheelCol2;
    public WheelCollider rightWheelCol3;
    public WheelCollider rightWheelCol4;
    public WheelCollider middleWheelCol1;
    public WheelCollider middleWheelCol2;
    public WheelCollider middleWheelCol3;

    public float topSpeed;
    public float accel;
    public float brakingForce;
    public float upRightCounter = 0;

    //"BOOLS" FOR PLAYER CONTROLS switchING
    //Controls which direction buttons rotate treads
    private int invertSpeed = 1;
    //used to set invert speed
    public int invertSpeedBool = 0;

    //switches left and right side buttons (triggers and bumpers)
    private int invertControls = 1;

    private float brakeTorqu;
    private float rightTread;
    private float leftTread;
    private float middleTread;
    public float wheelDamp;

    bool brakeRight = true;
    bool brakeLeft = true;
    bool brakeMiddle = true;
    [HideInInspector]
    public bool edgeRecovery = false;

    //Dizzy variables
    private bool timerUntilDizzy;
    public float timerUntilDizzyTime = 3;
    private float originalTimerUntilDizzyTime;

    private bool backToNormalTimer;
    public float backToNormalTimerTime = 3;
    private float originalBackToNormalTimerTime;

    public bool canMove;
    private bool firstFrame = true;

    Vector3 prevScale;
    float recentRotation;
    // Use this for initialization
    void Start()
    {
        //menuSel = FindObjectOfType<MenuSelection>();
        agent = GetComponentInChildren<NavMeshAgent>();
        player = gameObject;
        ragdollmode = player.GetComponentInChildren<FullRagdollMode>();
        characterJoint = player.GetComponentInChildren<CharacterJoint>();
        health = GetComponent<PlayerHealth>();
        playerStateEffect = gameObject.GetComponentInChildren<PlayerStateEffect>();
        playerIcon = gameObject.GetComponentInChildren<PlayerIconScript>();
        tankTextureSpeed = gameObject.GetComponentInChildren<TankTextureSpeed>();

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

        //Setting wheeldamping to each wheel because it doest work through unity editor otherwise
        leftWheelCol1.wheelDampingRate = wheelDamp;
        leftWheelCol2.wheelDampingRate = wheelDamp;
        leftWheelCol3.wheelDampingRate = wheelDamp;
        leftWheelCol4.wheelDampingRate = wheelDamp;
        rightWheelCol1.wheelDampingRate = wheelDamp;
        rightWheelCol2.wheelDampingRate = wheelDamp;
        rightWheelCol3.wheelDampingRate = wheelDamp;
        rightWheelCol4.wheelDampingRate = wheelDamp;
        middleWheelCol1.wheelDampingRate = wheelDamp;
        middleWheelCol2.wheelDampingRate = wheelDamp;
        middleWheelCol3.wheelDampingRate = wheelDamp;
    }

    // Update is called once per frame
    void Update()
    {

        //if (menuSel.menu == false)
        //{
            if (canMove)
            {
                    KeyPress();
            }
        //}

        if (timerUntilDizzy)
        {
            DizzyTimer();
        }

        if (backToNormalTimer)
        {
            BackToNormalTimer();
        }
        SetWhirlLevel();



        firstFrame = false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down * 0.5f, out hit) && hit.collider.gameObject.name == "TankMesh" || hit.collider.gameObject.name == "FallDeathtrigger")
        {
            tankTextureSpeed.speedR = 0;
            tankTextureSpeed.speedL = 0;
            tankTextureSpeed.wheelParticlesFR.Clear();
            tankTextureSpeed.wheelParticlesFL.Clear();
            tankTextureSpeed.wheelParticlesBR.Clear();
            tankTextureSpeed.wheelParticlesBL.Clear();
        }


    }





    //normal controls
    void KeyPress()
    {
        //Tread speed increase
        print("x " + agent.velocity.x);
        print("z " + agent.velocity.z);
        if (transform.rotation.y < agent.transform.rotation.y)
        {
            rightTread -= accel * invertSpeed * Time.deltaTime;
            brakeRight = false;

            if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedR = -0.99f * invertSpeed;
            }
            leftTread += accel * invertSpeed * Time.deltaTime;
            brakeLeft = false;

            if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedL = 0.99f * invertSpeed;
            }
            middleTread = 0;
            brakeMiddle = true;
        }
        else if (transform.rotation.y > agent.transform.rotation.y || agent.velocity.x < -0.41)
        {
            leftTread -= accel * invertSpeed * Time.deltaTime;
            brakeLeft = false;

            if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedL = -0.99f * invertSpeed;
            }
            rightTread += accel * invertSpeed * Time.deltaTime;
            brakeRight = false;

            if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedR = 0.99f * invertSpeed;
            }
            middleTread = 0;
            brakeMiddle = true;
        }
        //RB
        //if (agent.gameObject.transform.rotation.y > recentRotation)            //RB
        //{
        //    rightTread -= accel * invertSpeed * Time.deltaTime;
        //    brakeRight = false;

        //    if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
        //    {
        //        tankTextureSpeed.speedR = -0.99f * invertSpeed;
        //    }

        //}

        //////LB
        //if (agent.gameObject.transform.rotation.y < recentRotation)              //LB
        //{
        //    leftTread -= accel * invertSpeed * Time.deltaTime;
        //    brakeLeft = false;

        //    if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
        //    {
        //        tankTextureSpeed.speedL = -0.99f * invertSpeed;
        //    }
        //}

        //MIDDLE WHEELS
        //if ()
        //{
        //    middleTread -= accel * invertSpeed * Time.deltaTime;
        //    brakeMiddle = false;
        //}

        //if ()
        //{
        //    middleTread += accel * invertSpeed * Time.deltaTime;
        //    brakeMiddle = false;
        //}

        ////RT
        //if (agent.gameObject.transform.rotation.y < recentRotation)        //RT
        //{
        //    rightTread += accel * invertSpeed * Time.deltaTime;
        //    brakeRight = false;

        //    if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
        //    {
        //        tankTextureSpeed.speedR = 0.99f * invertSpeed;
        //    }
        //}

        ////LT
        //if (agent.gameObject.transform.rotation.y > recentRotation)          //LT
        //{
        //    leftTread += accel * invertSpeed * Time.deltaTime;
        //    brakeLeft = false;

        //    if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
        //    {
        //        tankTextureSpeed.speedL = 0.99f * invertSpeed;
        //    }
        //}

        if (agent.velocity.z > 0.1 /*|| agent.velocity.z > agent. velocity.x - 3*/)
        {
            print("jaa");
            rightTread -= accel * invertSpeed * Time.deltaTime;
            brakeRight = false;

            if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedR = -0.99f * invertSpeed;
            }

            leftTread -= accel * invertSpeed * Time.deltaTime;
            brakeLeft = false;

            if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedL = -0.99f * invertSpeed;
            }
            middleTread -= accel * invertSpeed * Time.deltaTime;
            brakeMiddle = false;
        }
        if (agent.velocity.z < -0.1f /*&& agent.velocity.z < agent.velocity.x - 0.1*/)
        {
            print("kyllä");
            rightTread += accel * invertSpeed * Time.deltaTime;
            brakeRight = false;

            if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedR = 0.99f * invertSpeed;
            }
            leftTread += accel * invertSpeed * Time.deltaTime;
            brakeLeft = false;

            if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedL = 0.99f * invertSpeed;
            }
            middleTread += accel * invertSpeed * Time.deltaTime;
            brakeMiddle = false;
        }

        //Clamping tread speeds
        rightTread = Mathf.Clamp(rightTread, -topSpeed, topSpeed);
        leftTread = Mathf.Clamp(leftTread, -topSpeed, topSpeed);
        middleTread = Mathf.Clamp(middleTread, -topSpeed, topSpeed);

        ////set motortorque to 0 when no input is given
        if (agent.velocity.z < 0.1 && agent.velocity.z > -0.1)
        {
            rightTread = 0;
            brakeRight = true;
            middleTread = 0;
            brakeMiddle = true;

            if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedR = 0;
            }
            leftTread = 0;
            brakeLeft = true;

            if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedL = 0;
            }
        }

        if (agent.velocity.z > -0.1 && agent.velocity.z < 0.1)
        {
            rightTread = 0;
            brakeRight = true;
            middleTread = 0;
            brakeMiddle = true;

            if (rightWheelCol1.isGrounded == true || rightWheelCol2.isGrounded == true || rightWheelCol3.isGrounded == true || rightWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedR = 0;
            }
            leftTread = 0;
            brakeLeft = true;
            middleTread = 0;
            brakeMiddle = true;

            if (leftWheelCol1.isGrounded == true || leftWheelCol2.isGrounded == true || leftWheelCol3.isGrounded == true || leftWheelCol4.isGrounded == true)
            {
                tankTextureSpeed.speedL = 0;
            }
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
        //DebugMovementControls();
        recentRotation = agent.gameObject.transform.rotation.y;
    }

    //Inverted controls

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
        if (brakeMiddle)
        {
            middleWheelCol1.brakeTorque = brakeTorqu;
            middleWheelCol2.brakeTorque = brakeTorqu;
            middleWheelCol3.brakeTorque = brakeTorqu;
        }
        else
        {
            middleWheelCol1.brakeTorque = 0;
            middleWheelCol2.brakeTorque = 0;
            middleWheelCol3.brakeTorque = 0;
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
        middleWheelCol1.motorTorque = middleTread;
        middleWheelCol2.motorTorque = middleTread;
        middleWheelCol3.motorTorque = middleTread;
    }
   

    void TurnUpRight()
    {

        // 60 < X < 300
        if ((60 < transform.rotation.eulerAngles.x && transform.rotation.eulerAngles.x < 300) || (60 < transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z < 300))
        {
            upRightCounter += Time.fixedDeltaTime;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = other.transform;
            //print("i am working");
        }
        if (other.gameObject.tag == "TipTheScalesPlat")
        {
            if (prevScale != other.transform.parent.localScale)
            {
                prevScale = other.transform.parent.localScale;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TipTheScalesPlat")
        {
            //get unit vector between player and platform center
            Vector3 POunit = Vector3.Normalize(other.transform.position - this.transform.position);
            // difference between the scale of the platfom on previous frame and this frame. if same scalediff = 0
            float scaleDiff = prevScale.x - other.transform.parent.localScale.x;
            if (scaleDiff != 0)
            {

                Vector3 POmove = POunit * scaleDiff * 0.5f;
                //set Y to zero because we dont want to move on that axis
                POmove.y = 0;
                transform.position += POmove;

                prevScale = other.transform.parent.localScale;
            }
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

        SetWhirlLevel();

        if (timerUntilDizzyTime <= 0)
        {
            whirlEffect.SetLevel(0);

            ragdollmode.RagdollMode();
            health.currentState = PlayerHealth.PLAYER_STATE.STUNNED;
            health.SetPlayerState();
            canMove = false;
            rightTread = 0;
            leftTread = 0;
            playerStateEffect.dizzyStart = true;
            tankTextureSpeed.speedR = 0;
            tankTextureSpeed.speedL = 0;

            timerUntilDizzyTime = originalTimerUntilDizzyTime;

            backToNormalTimer = true;
            timerUntilDizzy = false;
        }
    }

    private void SetWhirlLevel()
    {
        //Test and send whirl effect level

        if (timerUntilDizzyTime >= originalTimerUntilDizzyTime - 0.05f)
        {
            whirlEffect.SetLevel(0);
        }

        if (originalTimerUntilDizzyTime >= timerUntilDizzyTime && timerUntilDizzyTime >= originalTimerUntilDizzyTime / 4 * 3)
        {
            whirlEffect.SetLevel(1);
        }

        if (originalTimerUntilDizzyTime / 4 * 3 >= timerUntilDizzyTime && timerUntilDizzyTime >= originalTimerUntilDizzyTime / 4 * 2)
        {
            whirlEffect.SetLevel(2);
        }

        if (originalTimerUntilDizzyTime / 4 * 2 >= timerUntilDizzyTime && timerUntilDizzyTime >= originalTimerUntilDizzyTime / 4)
        {
            whirlEffect.SetLevel(3);
        }

        if (originalTimerUntilDizzyTime / 4 >= timerUntilDizzyTime && timerUntilDizzyTime >= 0.05f)
        {
            whirlEffect.SetLevel(4);
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
            playerStateEffect.dizzyUpdate = false;

            canMove = true;
            backToNormalTimer = false;
        }
    }


   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Drawbridge")
        {
            StopAllCoroutines();
            edgeRecovery = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Drawbridge")
        {
            RecoveryTimer(3);
        }
    }
    public IEnumerator RecoveryTimer(float time)
    {
        yield return new WaitForSeconds(time);
        edgeRecovery = true;
    }
}
