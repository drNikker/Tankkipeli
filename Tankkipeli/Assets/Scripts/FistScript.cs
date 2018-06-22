using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public GameObject fist;
    public Transform target;

    public AnimationClip animationClip;

    public GameObject[] gameobjects;
    public string animationName;

    private Rigidbody punchRB;

    protected bool punchTimer;
    [Space(10)]
    public float startPunchTimerTime;
    public float defaultPunchTimerTime;

    protected bool fistDistanceTimer;
    public float fistDistanceTimerTime;
    protected float originalFistDistanceTimerTime;

    protected bool holdOffTimer;
    public float holdOffTimerTime;
    protected float originalHoldOffTimerTime;

    [Space(10)]
    public float punchSpeed;

    [Space(10)]
    public float x;
    public float y;
    public float z;

    public bool startPunching;
    public bool fistGoesBack;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private Vector3 localPos;

    public Animator anim;

    public float stopRotation;

    protected virtual void Start()
    {
        punchRB = gameObject.GetComponent<Rigidbody>();

        anim = transform.root.gameObject.GetComponent<Animator>();
        punchTimer = true;

        originalFistDistanceTimerTime = fistDistanceTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
    }

    void Update()
    {

    }

    protected void Punch(float x, float y, float z)
    {
        punchRB.velocity = fist.transform.localPosition = new Vector3(x, y, z * punchSpeed);
        fistDistanceTimer = true;

        /*
        if (transform.position == targetPos.position)
        {
            startPunching = false;
        }
        */
    }

    protected void FistDistanceTimer()
    {
        fistDistanceTimerTime -= Time.deltaTime;

        if (fistDistanceTimerTime <= 0)
        {
            punchRB.velocity = Vector3.zero;
            punchRB.angularVelocity = Vector3.zero;

            fistDistanceTimerTime = originalFistDistanceTimerTime;

            startPunching = false;
            holdOffTimer = true;
            fistDistanceTimer = false;
        }
    }

    protected void FistGoesBack(Transform targetPOS)
    {
        //punchRB.velocity = Vector3.MoveTowards(transform.localPosition, targetPOS.position, 1) * punchSpeed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(transform.position, targetPOS.position, ref velocity, smoothTime);

        //transform.position = Vector3.Lerp(transform.position, targetPOS.position, 1) * Time.deltaTime;

        if (transform.position == targetPOS.position)
        {
            fistGoesBack = false;
        }

        punchTimer = true;
    }
}
