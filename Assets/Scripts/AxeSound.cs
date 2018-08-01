using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSound : MonoBehaviour {
    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;
    WeaponDamage weaponDamage;
    float cooldown;

    // Use this for initialization
    void Start()
    {
        weaponDamage = gameObject.GetComponentInChildren<WeaponDamage>();
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }


    public void playSound()
    {
        if (weaponDamage.finalDamage < 20)
        {
            currentAudioClip = audioScript.weaponAudioList[3];
        }
        else
        {
            currentAudioClip = audioScript.weaponAudioList[4];
        }
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }

}
