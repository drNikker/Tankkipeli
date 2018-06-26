using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFist : FistScript
{
    public float power;
    public float range;
    protected override void Start()
    {
        base.Start();

        anim.SetFloat("Power", power);
        anim.SetFloat("Range", range);

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
    }

    protected void PunchTimer()
    {

        startPunchTimerTime -= Time.deltaTime;

    

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
            punchTimer = true;
            holdOffTimer = false;
        }
    }
}
