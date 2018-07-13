using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;
    private int randomIndex;

    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void RandomizeWeaponAudio()
    {
        randomIndex = Random.Range(0, audioScript.weaponAudioList.Count);

        currentAudioClip = audioScript.weaponAudioList[randomIndex];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
