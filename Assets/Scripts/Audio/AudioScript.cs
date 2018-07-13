using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public List<AudioClip> playerAudioList = new List<AudioClip>();
    public List<AudioClip> musicList = new List<AudioClip>();

    private AudioSource audioSource;
    private AudioClip currentAudioClip;
    private AudioClip currentSceneMusic;
    private int randomIndex;

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

    public void RandomizePlayerAudio()
    {
        randomIndex = Random.Range(0, playerAudioList.Count);

        currentAudioClip = playerAudioList[randomIndex];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }

    public void PlaySceneMusic(int index)
    {
        currentSceneMusic = musicList[index];
        audioSource.clip = currentSceneMusic;
        audioSource.Play();
    }
}
