﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour
{
    public Animator anim;

    protected bool waitTimer;
    public float waitTimerTime;
    protected float originalWaitTimerTime;

    protected bool holdOffTimer;
    public float holdOffTimerTime;
    protected float originalHoldOffTimerTime;

    void Start()
    {
        //anim.SetBool("Open/Close", true);
        //bridgeList = GameObject.FindGameObjectWithTag("BridgeList").GetComponent<BridgeList>();
        waitTimer = true;

        originalWaitTimerTime = waitTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer == true)
        {
            WaitTimer();
        }

        if (holdOffTimer == true)
        {
            HoldOffTimer();
        }
    }

    private void WaitTimer()
    {
        waitTimerTime -= Time.deltaTime;

        if (waitTimerTime <= 0)
        {
            anim.SetBool("Open/Close", false);
            waitTimerTime = originalWaitTimerTime;
            holdOffTimer = true;
            waitTimer = false;
        }
    }

    private void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            anim.SetBool("Open/Close", true);
            holdOffTimerTime = originalHoldOffTimerTime;
            waitTimer = true;
            holdOffTimer = false;
        }
    }
}
