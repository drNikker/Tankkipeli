using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherAIMovement : MonoBehaviour {
    // XINPUT STUFF
    //public PlayerIndex playerIndex;

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
    void Start () {
        //menuSel = FindObjectOfType<MenuSelection>();
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
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (menuSel.menu == false)
        //{

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
        //Dizzy 
        //if (if the ai rotates fast for some time)
        //{
        //    timerUntilDizzy = true;
        //}
        //else
        //{
        //    timerUntilDizzyTime += Time.deltaTime;
        //    timerUntilDizzy = false;

        //    if (timerUntilDizzyTime >= originalTimerUntilDizzyTime)
        //    {
        //        timerUntilDizzyTime = originalTimerUntilDizzyTime;
        //    }
        //}
    }


    private void FixedUpdate()
    {
    TurnUpRight();
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
