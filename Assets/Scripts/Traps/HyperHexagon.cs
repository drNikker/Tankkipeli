using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperHexagon : MonoBehaviour
{
    public Animator animator;

    private bool timerUntilDrop;
    [Space(10)]
    public float timerUntilDropTime;


    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;
    void Start()
    {
        timerUntilDrop = true;
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timerUntilDrop)
        {
            TimerUntilDrop();
        }
    }

    private void TimerUntilDrop()
    {
        timerUntilDropTime -= Time.deltaTime;

        if (timerUntilDropTime <= 0)
        {
            PlaySound();
                StartCoroutine(DelayBeeb());

            switch (gameObject.tag)
            {
                case "Fist1":
                    animator.SetTrigger("1");
                    break;
                case "Fist2":
                    animator.SetTrigger("2");
                    break;
                case "Fist3":
                    animator.SetTrigger("3");
                    break;
                case "Fist4":
                    animator.SetTrigger("4");
                    break;
                case "Untagged":
                    animator.SetTrigger("5");
                    break;
                case "Car":
                    animator.SetTrigger("6");
                    break;
            }

            timerUntilDropTime = 0;
            timerUntilDrop = false;
        }
    }
    IEnumerator DelayBeeb()
    {
        yield return new WaitForSeconds(0.65f);
        PlaySound();
        yield return new WaitForSeconds(0.65f);
        PlaySound();
        yield return new WaitForSeconds(0.65f);
        PlaySound();
        yield return new WaitForSeconds(0.65f);
        PlaySound();
    }

    private void PlaySound()
    {
        currentAudioClip = audioScript.hazardAudioList[4];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }

}
