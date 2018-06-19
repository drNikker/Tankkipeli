using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public GameObject fist;
    public Transform target;

    public bool punchTimer;
    public float punchTimerTime;
    private float originalPunchTimerTime;
    private bool hasPunched;

    public bool fistDistanceTimer;
    public float fistDistanceTimerTime;
    private float originalFistDistanceTimerTime;

    public bool holdOffTimer;
    public float holdOffTimerTime;
    private float originalHoldOffTimerTime;

    public float punchSpeed;

    private Rigidbody punchRB;

    void Start()
    {
        punchRB = GetComponent<Rigidbody>();

        punchTimer = true;
        originalPunchTimerTime = punchTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
    }

    void Update()
    {
        if (punchTimer)
        {
            PunchTimer();
        }

        if (fistDistanceTimer == true)
        {
            FistDistanceTimer();
        }

        if (holdOffTimer == true)
        {
            HoldOffTimer();
        }
    }

    private void PunchTimer()
    {
        punchTimerTime -= Time.deltaTime;

        if (punchTimerTime <= 0)
        {
            Punch();
            punchTimerTime = originalPunchTimerTime;
            punchTimer = false;
        }
    }

    private void Punch()
    {
        //float speed = punchSpeed * Time.deltaTime;
        
        punchRB.velocity = fist.transform.localPosition = new Vector3(0, 0.58f, 1 * punchSpeed);

        //punchRB.velocity = new Vector3(10, 0, 0) * punchSpeed;

        fistDistanceTimer = true;

    }

    private void FistDistanceTimer()
    {
        fistDistanceTimerTime -= Time.deltaTime;

        if (fistDistanceTimerTime <= 0)
        {
            punchRB.velocity = Vector3.zero;
            punchRB.angularVelocity = Vector3.zero;

            holdOffTimer = true;
            fistDistanceTimer = false;
        }
    }

    private void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            //punchRB.isKinematic = false;
            FistGoesBack();
            holdOffTimerTime = originalHoldOffTimerTime;
            holdOffTimer = false;
        }
    }

    private void FistGoesBack()
    {
        //punchRB.velocity = fist.transform.position = new Vector3(-1, -0.2f, 0);

        //punchRB.velocity = fist.transform.position = new Vector3(-1, 0.58f, -1 * punchSpeed);

        //punchRB.velocity = Vector3.MoveTowards(transform.localPosition, target.position, 1) * punchSpeed;

        transform.position = Vector3.Lerp(transform.position, target.position, 1);
        punchTimer = true;
    }
}
