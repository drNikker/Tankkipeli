using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFist : FistScript
{
    [Space(10)]
    public float power;
    public float range;
    public ParticleSystem VFX;
    int finalDamageVFX;
    [HideInInspector]
    public bool canDoDamage;

    protected override void Start()
    {
        VFX = GetComponent<ParticleSystem>();
        base.Start();

        anim.SetFloat("Power", power);
        anim.SetFloat("Range", range);

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
                punchTimer = true;
                waitTimer = false;
            }
        }
    }

    protected void PunchTimer()
    {
        punchTimerTime -= Time.deltaTime;

        if (punchTimerTime <= stopRotation)
        {
            anim.SetBool("Warning", true);
        }

        if (punchTimerTime <= 0)
        {
            canDoDamage = true;
            anim.SetBool("Warning", false);
            anim.SetBool("FB", true);
            punchTimerTime = defaultPunchTimerTime;
            holdOffTimer = true;
            punchTimer = false;
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
                finalDamageVFX = Mathf.RoundToInt(damage);
                VFX.Emit(2 * finalDamageVFX);

                cooldown = Time.time + cooldownTime;
            }
        }
    }
}
