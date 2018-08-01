using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificFistScript : FistScript
{
    [Space(10)]
    public float power;
    public float range;
    public ParticleSystem VFX;
    public ParticleSystem VFXCharge;
    public ParticleSystem VFXLaunch;
    int finalDamageVFX;
    [HideInInspector]
    public bool canDoDamage;
    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
        transform.root.gameObject.GetComponent<CannonScrit>().rotate = true;
        VFX = GetComponent<ParticleSystem>();

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
                transform.root.gameObject.GetComponent<CannonScrit>().rotate = true;
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
            VFXCharge.Play(true);
            transform.root.GetComponent<CannonScrit>().rotate = false;
        }

        if (punchTimerTime <= 0)
        {
            canDoDamage = true;
            anim.SetBool("Warning", false);
            VFXCharge.Stop(true);
            VFXLaunch.Play(true);
            anim.SetBool("FB",true);
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
            VFXLaunch.Stop(true);
            holdOffTimer = false;
            canDoDamage = false;
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
                VFX.Emit(3 * finalDamageVFX);

                cooldown = Time.time + cooldownTime;
                playSound();
            }
        }
    }
    private void playSound()
    {
        currentAudioClip = audioScript.hazardAudioList[3];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
