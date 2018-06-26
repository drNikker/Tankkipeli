using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificFistScript : FistScript
{
    public float power;
    protected override void Start()
    {
        base.Start();

        anim.SetFloat("Power", power);
    }

    void Update()
    {
        if (punchTimer == true)
        {
            PunchTimer();
        }

        if (holdOffTimer == true)
        {
            HoldOffTimer();
        }

        if (waitTimer)
        {
            waitTimerTime -= Time.deltaTime;

            if (waitTimerTime <= 0)
            {
                transform.root.gameObject.GetComponent<CannonScrit>().rotate = true;
                waitTimerTime = originalWaitTimerTime;
                punchTimer = true;
                waitTimer = false;
            }
        }
    }

    protected void PunchTimer()
    {
        startPunchTimerTime -= Time.deltaTime;

        if (startPunchTimerTime <= stopRotation)
        {
            transform.root.gameObject.GetComponent<CannonScrit>().rotate = false;
        }

        if (startPunchTimerTime <= 0)
        {
            anim.SetBool("FB",true);
            startPunchTimerTime = defaultPunchTimerTime;
            punchTimer = false;
            holdOffTimer = true;
        }
    }

    protected void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            anim.SetBool("FB", false);
            holdOffTimerTime = originalHoldOffTimerTime;
            waitTimer = true;
            holdOffTimer = false;
        }
    }
}
