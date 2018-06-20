using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificFistScript : FistScript
{

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (punchTimer == true)
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

        if (plöö == true)
        {
            FistGoesBack(target);
        }
    }

    protected void PunchTimer()
    {
        startPunchTimerTime -= Time.deltaTime;

        if (startPunchTimerTime <= 0)
        {
            Punch(x, y, z);
            startPunchTimerTime = defaultPunchTimerTime;
            punchTimer = false;
        }
    }

    protected void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            plöö = true;
            //FistGoesBack(target);
            holdOffTimerTime = originalHoldOffTimerTime;
            holdOffTimer = false;
        }
    }
}
