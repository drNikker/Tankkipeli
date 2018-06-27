using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateFists : FistScript
{

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (punchTimer == true)
        {
            PunchTimerSeparate();
        }

        if (holdOffTimer == true)
        {
           
        }
    }

    protected void PunchTimerSeparate()
    {
        punchTimerTime -= Time.deltaTime;

        /*
        if (startPunchTimerTime <= stopRotation)
        {
            transform.root.gameObject.GetComponent<CannonScrit>().rotate = false;
        }
        */

        if (punchTimerTime <= 0)
        {
            punchTimerTime = defaultPunchTimerTime;
            punchTimer = false;
            holdOffTimer = true;
        }
    }
}
