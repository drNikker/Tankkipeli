using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityFist : FistScript
{
    public float power;
    public float range;

    private ProximityFistCollider proximityFistCollider;

    protected override void Start()
    {
        base.Start();

        proximityFistCollider = gameObject.GetComponentInChildren<ProximityFistCollider>();

        anim.SetFloat("Power", power);
        anim.SetFloat("Range", range);

        waitTimer = false;
    }

    void Update()
    {
        if (punchTimer == true)
        {
            PunchTimer();
        }

        if (holdOffTimer == true)
        {
            HoldOffTimer();
        }

        if (waitTimer)
        {
            waitTimerTime -= Time.deltaTime;

            if (waitTimerTime <= 0)
            {
                waitTimerTime = originalWaitTimerTime;
                
                if (proximityFistCollider.allowPunching == true)
                {
                    punchTimer = true;
                }
                
                waitTimer = false;
            }
        }

        //CheckPlayer();
    }
    /*
    private void CheckPlayer()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, transform.right, out hit, 3);

        if (hit.collider.gameObject.tag != null)
        {
            if (hit.collider.gameObject.tag == "Bodypart")
            {
                punchTimer = true;
                Debug.Log(hit.collider.gameObject.tag);
                //gameObject.GetComponent<yourScript>().yourFunction()
            }
        }
        else
        {
            Debug.Log("There's no player.");
        }   
    }
    */
    protected void PunchTimer()
    {
        anim.SetBool("Warning", true);
        punchTimerTime -= Time.deltaTime;

        if (punchTimerTime <= 0)
        {
            anim.SetBool("Warning", false);
            anim.SetBool("FB", true);
            punchTimerTime = defaultPunchTimerTime;
            punchTimer = false;
            holdOffTimer = true;
        }
    }

    protected void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            anim.SetBool("FB", false);
            holdOffTimerTime = originalHoldOffTimerTime;

            waitTimer = true;
            holdOffTimer = false;
        }
    }
}
