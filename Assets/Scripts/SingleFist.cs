using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFist : FistScript
{
    public float power;
    public float range;
    protected override void Start()
    {
        base.Start();

        anim.SetFloat("Power", power);
        anim.SetFloat("Range", range);

        //punchTimer = true;
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
            punchTimer = true;
            holdOffTimer = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bodypart")
        {
            punchTimer = true;
        }
    }
}
