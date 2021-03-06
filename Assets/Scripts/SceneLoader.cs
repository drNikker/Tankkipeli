﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class SceneLoader : MonoBehaviour
{
    RoundManager roundManager;
    AudioScript audioScript;

    List<string> mapSet1 = new List<string>();
    List<string> mapSet2 = new List<string>();
    List<string> mapSet3 = new List<string>();
    List<string> mapSet4 = new List<string>();
    List<string> mapSet5 = new List<string>();

    public Animator anim;
    private bool menu;
    private bool open;
    public int setAmount = 5;


    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        roundManager = gameObject.GetComponent<RoundManager>();

        ////Map set 1
        mapSet1.Add("9999");
        mapSet1.Add("2310");
        mapSet1.Add("FlipperBox");
        mapSet1.Add("TheDefault");
        mapSet1.Add("Squiggly");
        mapSet1.Add("WeaponThrow");
        //Map set 2
        mapSet2.Add("QuadBridge");
        mapSet2.Add("BarrelRoll");
        mapSet2.Add("TheSlice");
        mapSet2.Add("CannonShuffle");
        mapSet2.Add("RotatingHoleInWall");
        mapSet2.Add("Normandy");
        //Map set 3
        mapSet3.Add("HyperHexagon");
        mapSet3.Add("Rotat-o-Maze");
        mapSet3.Add("Checkers");
        mapSet3.Add("TipTheScales");
        mapSet3.Add("CrushingWall");
        //Map set 4
        mapSet4.Add("FallingShit");
        mapSet4.Add("SpinningBeams");
        mapSet4.Add("Roof");
        mapSet4.Add("ConstructionInDisguise");
        mapSet4.Add("WreckingBall");
        //Map set 5
        mapSet5.Add("Nascar");
        mapSet5.Add("RalliJump");
        mapSet5.Add("DualRace");
        mapSet5.Add("Smash");
        //mapSet5.Add("Madness");
    }

    private void Update()
    {
        //if(Input.GetKeyDown("space"))
        //{
        //    StatHolder.CurrentMode = StatHolder.Modes.DM;
        //    SceneManager.LoadScene("JoiningScene");
        //    audioScript.PlaySceneMusic(0);
        //}
    }

    //Loads the menu scene and sets all win counters to zero
    public void MenuScene()
    {
        SceneManager.LoadScene("JoiningScene");
        StatHolder.HowManyPlayers = 0;
        StatHolder.Player1Wins = 0;
        StatHolder.Player2Wins = 0;
        StatHolder.Player3Wins = 0;
        StatHolder.Player4Wins = 0;
        StatHolder.TeamBlueWins = 0;
        StatHolder.TeamRedWins = 0;
        StatHolder.WinsNeeded = 0;
        StatHolder.RoundNumber = 0;
    }

    //Reloads the scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        roundManager.RoundStart();
    }

    //Assigns a Random scene to load (excluding the menu scene)
    public void NewRandomScene()
    {
        SceneManager.LoadScene(UnityEngine.Random.Range(1, 12));
    }

    //Loads the next scene in a given set of scenes (arenas)
    public void NextSetScene(int setNumber)
    {
        if (StatHolder.usedSets.Count >= setAmount)
        {
            StatHolder.usedSets.Clear();
        }
        switch (setNumber)
        {

            case 1:
                if (StatHolder.RoundNumber > mapSet1.Count -1)
                {
                    StatHolder.RoundNumber = 0;
                    int i = UnityEngine.Random.Range(1, setAmount + 1);
                    while (i == 1 || StatHolder.usedSets.Contains(i))
                    {
                        i = UnityEngine.Random.Range(1, setAmount + 1);
                    }
                    StatHolder.usedSets.Add(i);
                    NextSetScene(i);
                    StatHolder.WhichSet = i;
                    audioScript.StopPlayingSceneMusic();
                    audioScript.PlaySceneMusic(StatHolder.WhichSet);
                }
                else
                {
                    SceneManager.LoadScene(mapSet1[StatHolder.RoundNumber]);
                }
                break;
            case 2:
                if (StatHolder.RoundNumber > mapSet2.Count -1)
                {
                    StatHolder.RoundNumber = 0;
                    int i = UnityEngine.Random.Range(1, setAmount + 1);
                    while (i == 2 || StatHolder.usedSets.Contains(i))
                    {
                        i = UnityEngine.Random.Range(1, setAmount + 1);
                    }
                    StatHolder.usedSets.Add(i);
                    NextSetScene(i);
                    StatHolder.WhichSet = i;
                    audioScript.StopPlayingSceneMusic();
                    audioScript.PlaySceneMusic(StatHolder.WhichSet);
                }
                else
                {
                    SceneManager.LoadScene(mapSet2[StatHolder.RoundNumber]);
                }
                break;
            case 3:
                if (StatHolder.RoundNumber > mapSet3.Count -1)
                {
                    StatHolder.RoundNumber = 0;
                    int i = UnityEngine.Random.Range(1, setAmount + 1);
                    while (i == 3 || StatHolder.usedSets.Contains(i))
                    {
                        i = UnityEngine.Random.Range(1, setAmount + 1);
                    }
                    StatHolder.usedSets.Add(i);
                    NextSetScene(i);
                    StatHolder.WhichSet = i;
                    audioScript.StopPlayingSceneMusic();
                    audioScript.PlaySceneMusic(StatHolder.WhichSet);
                }
                else
                {
                    SceneManager.LoadScene(mapSet3[StatHolder.RoundNumber]);
                }
                break;
            case 4:
                if (StatHolder.RoundNumber > mapSet4.Count - 1)
                {
                    StatHolder.RoundNumber = 0;
                    int i = UnityEngine.Random.Range(1, setAmount + 1);
                    while (i == 4 || StatHolder.usedSets.Contains(i))
                    {
                        i = UnityEngine.Random.Range(1, setAmount + 1);
                    }
                    StatHolder.usedSets.Add(i);
                    NextSetScene(i);
                    StatHolder.WhichSet = i;
                    audioScript.StopPlayingSceneMusic();
                    audioScript.PlaySceneMusic(StatHolder.WhichSet);
                }
                else
                {
                    SceneManager.LoadScene(mapSet4[StatHolder.RoundNumber]);
                }
                break;
            case 5:
                if (StatHolder.RoundNumber > mapSet5.Count - 1)
                {
                    StatHolder.RoundNumber = 0;
                    int i = UnityEngine.Random.Range(1, setAmount + 1);
                    while (i == 5 || StatHolder.usedSets.Contains(i))
                    {
                        i = UnityEngine.Random.Range(1, setAmount + 1);
                    }
                    StatHolder.usedSets.Add(i);
                    NextSetScene(i);
                    StatHolder.WhichSet = i;
                    audioScript.StopPlayingSceneMusic();
                    audioScript.PlaySceneMusic(StatHolder.WhichSet);
                }
                else
                {
                    SceneManager.LoadScene(mapSet5[StatHolder.RoundNumber]);
                }
                break;
            default:
                Debug.LogWarning("SceneLoader NextSetScene() switch case default was chosen");
                break;
        }
    }
}