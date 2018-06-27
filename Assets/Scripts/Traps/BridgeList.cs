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

    void Start()
    {
        specialWaitTimer = true;
    }

    void Update()
    {
        if (specialWaitTimer == true)
        {
            SpecialTimer();
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
            specialWaitTimer = false;
        }
    }
}
