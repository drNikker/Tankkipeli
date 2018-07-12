using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioRandomizer : MonoBehaviour
{
    public List<AudioClip> playerAudioList = new List<AudioClip>();
    public List<AudioClip> musicList = new List<AudioClip>();

    private AudioSource audioSource;
    private AudioClip currentAudioClip;
    private int randomIndex;

    public static PlayerAudioRandomizer Instance;

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

    public void PlaySceneMusic()
    {

    }
}
