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
            HoldOffTimer();
        }
    }

    protected void PunchTimerSeparate()
    {
        startPunchTimerTime -= Time.deltaTime;

        /*
        if (startPunchTimerTime <= stopRotation)
        {
            transform.root.gameObject.GetComponent<CannonScrit>().rotate = false;
        }
        */

        if (startPunchTimerTime <= 0)
        {
            switch (gameObject.tag)
            {
                case "Fist1":
                    anim.SetFloat("speedMultiplier1", punchSpeed);
                    anim.speed = punchSpeed;
                    anim.SetBool("1FB", true);
                    Debug.Log(anim.GetFloat("speedMultiplier"));
                    break;
                case "Fist2":
                    anim.SetFloat("speedMultiplier2", punchSpeed);
                    anim.speed = punchSpeed;
                    anim.SetBool("2FB", true);
                    Debug.Log(anim.GetFloat("speedMultiplier"));
                    break;
                case "Fist3":
                    anim.SetFloat("speedMultiplier3", punchSpeed);
                    anim.speed = punchSpeed;
                    anim.SetBool("3FB", true);
                    Debug.Log(anim.GetFloat("speedMultiplier"));
                    break;
                case "Fist4":
                    anim.SetFloat("speedMultiplier4", punchSpeed);
                    anim.speed = punchSpeed;
                    anim.SetBool("4FB", true);
                    Debug.Log(anim.GetFloat("speedMultiplier"));
                    break;
            }

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
            anim.speed = 1f;
            anim.SetBool("1FB", false);
            anim.SetBool("2FB", false);
            anim.SetBool("3FB", false);
            anim.SetBool("4FB", false);
            holdOffTimerTime = originalHoldOffTimerTime;
            punchTimer = true;
            holdOffTimer = false;
        }
    }
}
