using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBumper : MonoBehaviour
{
    public float force;
    public Animator anim;
    private Rigidbody rb;
    PhysicMovement1 physicMovement;

    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;

    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            physicMovement = collider.gameObject.GetComponent<PhysicMovement1>();
            physicMovement.StopAllCoroutines();
            physicMovement.edgeRecovery = false;
            physicMovement.RecoveryTimer(3);
            rb = collider.gameObject.GetComponent<Rigidbody>();
            anim.SetTrigger("Bump");
            Vector3 dir = collider.contacts[0].point - transform.position;
            dir = dir.normalized;

            rb.AddForce(dir * force);
            playSound();
        }
    }

    private void playSound()
    {
        currentAudioClip = audioScript.hazardAudioList[1];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
