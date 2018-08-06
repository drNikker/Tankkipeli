using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallCollider : MonoBehaviour
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

    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && cooldown <= Time.time)
        {
            playSound();
            cooldown = Time.time + 3;
        } else if (collision.gameObject.tag == "PlayArea")
        {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Environment")
        {
            GetComponent<SpikeDamage>().enabled = false;

        } else
        {
            GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().useGravity = true;
        }

        

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("CannonBall"))
        {
            Destroy(collider.gameObject);
        }
    }
    private void playSound()
    {
        currentAudioClip = audioScript.hazardAudioList[5];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
