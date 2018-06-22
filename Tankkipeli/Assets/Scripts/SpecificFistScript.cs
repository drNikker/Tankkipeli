using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificFistScript : FistScript
{

    protected override void Start()
    {
        base.Start();
    }

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

        if (fistGoesBack == true)
        {
            FistGoesBack(target);
        }

        if (startPunching == true)
        {
            Punch(x, y, z);
        }
    }

    protected void PunchTimer()
    {
        transform.root.gameObject.GetComponent<CannonScrit>().rotate = true;
        startPunchTimerTime -= Time.deltaTime;

        if (startPunchTimerTime <= stopRotation)
        {
            transform.root.gameObject.GetComponent<CannonScrit>().rotate = false;
        }

        if (startPunchTimerTime <= 0)
        {
            for (int i = 0; i < gameobjects.Length; i++)
            {
                animationName = gameobjects[i];
            }

            anim.SetTrigger("animationClip");
            //startPunching = true;
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
            //fistGoesBack = true;
            holdOffTimerTime = originalHoldOffTimerTime;
            punchTimer = true;
            holdOffTimer = false;
        }
    }
}
