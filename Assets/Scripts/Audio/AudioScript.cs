using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public List<AudioClip> musicList = new List<AudioClip>();
    public List<AudioClip> weaponAudioList = new List<AudioClip>();
    public List<AudioClip> hazardAudioList = new List<AudioClip>();

    private AudioSource audioSource;
    private AudioSource audioSourceChild;
    private AudioSource audioSourceChild2;
    private AudioClip currentSceneMusic;
    [Space(10)]
    public AudioClip knockOut;
    public AudioClip roundOver;
    public AudioClip motorRumble;
    public AudioClip menuClick;
    public AudioClip menuConfirm;

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
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {

        audioSourceChild = gameObject.GetComponentInChildren<AudioSource>();
        audioSourceChild2 = gameObject.transform.Find("MotorRumbleAudioSource").GetComponent<AudioSource>();
        PlayRumbleSound();
    }

    void Update()
    {

    }

    public void PlayKnockOutSound()
    {
        audioSourceChild.PlayOneShot(knockOut);
    }

    public void PlayRoundOverSound()
    {
        audioSourceChild.PlayOneShot(roundOver);
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
    public void PlayRumbleSound()
    {
        audioSourceChild2.clip = motorRumble;
        audioSourceChild2.Play();
    }
    
}
