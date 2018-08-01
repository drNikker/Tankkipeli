using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour
{
    public Animator anim;

    protected bool waitTimer;
    public float waitTimerTime;
    protected float originalWaitTimerTime;

    public float offsetTime;

    protected bool holdOffTimer;
    public float holdOffTimerTime;
    protected float originalHoldOffTimerTime;

    //private AudioScript audioScript;
    //private AudioClip currentAudioClip;
    //private AudioSource audioSource;

    void Start()
    {
        //audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        //audioSource = gameObject.GetComponent<AudioSource>();
        waitTimer = true;

        originalWaitTimerTime = waitTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
        waitTimerTime = offsetTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer == true)
        {
            WaitTimer();
        }

        if (holdOffTimer == true)
        {
            HoldOffTimer();
        }
    }

    private void WaitTimer()
    {
        waitTimerTime -= Time.deltaTime;

        if (waitTimerTime <= 0)
        {
            anim.SetBool("Open/Close", false);
            waitTimerTime = originalWaitTimerTime;
            holdOffTimer = true;
            waitTimer = false;
            //playSound();
        }
    }

    private void HoldOffTimer()
    {
        holdOffTimerTime -= Time.deltaTime;

        if (holdOffTimerTime <= 0)
        {
            anim.SetBool("Open/Close", true);
            holdOffTimerTime = originalHoldOffTimerTime;
            waitTimer = true;
            holdOffTimer = false;
            //StartCoroutine(waitAudio());
        }
    }
    //IEnumerator waitAudio()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    playSound();
    //}

    //private void playSound()
    //{
    //    currentAudioClip = audioScript.hazardAudioList[0];
    //    audioSource.clip = currentAudioClip;
    //    audioSource.Play();
    //}
}
