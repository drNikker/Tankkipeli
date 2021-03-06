﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTheScales : MonoBehaviour
{
    public Animator animator;
    public Animator tileanim;


    private bool waitTimerToGoSmall;
    [Space(10)]
    public float timeBeforeMakingSmall;
    private float originalTimeBeforeMakingSmall;

    private bool waitTimerToGoBig;
    [Space(10)]
    public float timeBeforeMakingBig;
    private float originalTimeBeforeMakingBig;

    void Start()
    {
        originalTimeBeforeMakingSmall = timeBeforeMakingSmall;
        originalTimeBeforeMakingBig = timeBeforeMakingBig;
      
        waitTimerToGoBig = true;
        timeBeforeMakingBig = 3f;
    }

    void Update()
    {
        if (waitTimerToGoSmall)
        {
            WaitTimerToGoSmall();
        }

        if (waitTimerToGoBig)
        {
            WaitTimerToGoBig();
        }
    }

    private void WaitTimerToGoSmall()
    {
        timeBeforeMakingSmall -= Time.deltaTime;
        if (timeBeforeMakingSmall <= 2 && timeBeforeMakingSmall >= 1.9)
        {
            tileanim.SetTrigger("Blink");
        }
        if (timeBeforeMakingSmall <= 0)
        {
            animator.SetTrigger("GoSmall");
            timeBeforeMakingSmall = originalTimeBeforeMakingSmall;
            waitTimerToGoBig = true;
            waitTimerToGoSmall = false;
        }
    }

    private void WaitTimerToGoBig()
    {
        timeBeforeMakingBig -= Time.deltaTime;
        if (timeBeforeMakingBig <= 2 && timeBeforeMakingBig >= 1.9)
        {
            tileanim.SetTrigger("Blink");
        }

        if (timeBeforeMakingBig <= 0)
        {
            animator.SetTrigger("GoBig");
            timeBeforeMakingBig = originalTimeBeforeMakingBig;
            waitTimerToGoSmall = true;
            waitTimerToGoBig = false;
        }
    }
}
