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
    }

    protected void PunchTimer()
    {
        punchTimerTime -= Time.deltaTime;

        if (punchTimerTime <= 0)
        {
            Punch(x, y, z);
            punchTimerTime = originalPunchTimerTime;
            punchTimer = false;
        }
    }

    protected void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            FistGoesBack(target);
            holdOffTimerTime = originalHoldOffTimerTime;
            holdOffTimer = false;
        }
    }
}
