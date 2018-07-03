using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityFist : FistScript
{
    private ProximityFistCollider proximityFistCollider;
    [Space(10)]
    public float power;
    public float range;
    [HideInInspector]
    public bool canDoDamage;

    protected override void Start()
    {
        base.Start();

        proximityFistCollider = gameObject.GetComponentInChildren<ProximityFistCollider>();

        anim.SetFloat("Power", power);
        anim.SetFloat("Range", range);

        waitTimer = false;
        canDoDamage = false;
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
    }

    protected void PunchTimer()
    {
        anim.SetBool("Warning", true);
        punchTimerTime -= Time.deltaTime;

        if (punchTimerTime <= 0)
        {
            canDoDamage = true;
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
            canDoDamage = false;
            holdOffTimer = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canDoDamage == true)
        {
            if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
            {
                collision.transform.root.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

                cooldown = Time.time + cooldownTime;
            }
        }
    }
}
