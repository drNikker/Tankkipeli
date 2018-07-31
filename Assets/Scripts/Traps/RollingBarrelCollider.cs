using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrelCollider : MonoBehaviour
{
    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;
    float cooldown;

    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && cooldown <= Time.time)
        {
            playSound();
            cooldown = Time.time + 3;
        }
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.CompareTag ("Untagged"))
    //    {
    //        Destroy(collider.gameObject);
    //    }
    //}

    private void playSound()
    {
        currentAudioClip = audioScript.hazardAudioList[0];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
