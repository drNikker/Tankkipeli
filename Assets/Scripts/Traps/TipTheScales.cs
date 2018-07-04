using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTheScales : MonoBehaviour
{
    public Animator animator;

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

        waitTimerToGoSmall = true;
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

        if (timeBeforeMakingBig <= 0)
        {
            animator.SetTrigger("GoBig");
            timeBeforeMakingBig = originalTimeBeforeMakingBig;
            waitTimerToGoSmall = true;
            waitTimerToGoBig = false;
        }
    }
}
