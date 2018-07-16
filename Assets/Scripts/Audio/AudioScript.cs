using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public List<AudioClip> musicList = new List<AudioClip>();
    public List<AudioClip> weaponAudioList = new List<AudioClip>();

    private AudioSource audioSource;
    private AudioClip currentSceneMusic;
    [Space(10)]
    public AudioClip knockOut;
    public AudioClip roundOver;

    public static AudioScript Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    public void PlayKnockOutSound()
    {
        audioSource.PlayOneShot(knockOut);
    }

    public void PlayRoundOverSound()
    {
        audioSource.PlayOneShot(roundOver);
    }

    public void PlaySceneMusic(int index)
    {
        currentSceneMusic = musicList[index];
        audioSource.clip = currentSceneMusic;
        audioSource.Play(); 
    }

    public void StopPlayingSceneMusic()
    {
        audioSource.Stop();
    }
}
