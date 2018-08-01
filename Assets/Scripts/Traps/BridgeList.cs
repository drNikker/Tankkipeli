using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeList : MonoBehaviour
{
    public GameObject[] bridgeList;

    protected bool specialWaitTimer;
    [Space(10)]
    public float specialWaitTimerTime;
    protected float originalSpecialWaitTimerTime;

    protected bool holdOffTimer;
    public float holdOffTimerTime;
    protected float originalHoldOffTimerTime;

    void Start()
    {
        specialWaitTimer = true;

        for (int i = 0; i < bridgeList.Length; i++)
        {
            bridgeList[i].GetComponent<DrawBridge>().enabled = false;
        }
    }

    void Update()
    {
        if (specialWaitTimer == true)
        {
            SpecialTimer();
        }

        if (holdOffTimer == true)
        {
            HoldOffTimer();
        }
    }

    private void SpecialTimer()
    {
        specialWaitTimerTime -= Time.deltaTime;

        if (specialWaitTimerTime <= 0)
        {
            for (int i = 0; i < bridgeList.Length; i++)
            {
                bridgeList[i].GetComponent<Animator>().SetBool("Open/Close", false);
            }

            specialWaitTimerTime = 0;
            holdOffTimer = true;
            specialWaitTimer = false;
        }
    }

    private void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            for (int i = 0; i < bridgeList.Length; i++)
            {
                bridgeList[i].GetComponent<Animator>().SetBool("Open/Close", true);
            }

            holdOffTimerTime = originalHoldOffTimerTime;
            holdOffTimer = false;
        }
    }
}
