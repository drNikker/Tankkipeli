﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoader : MonoBehaviour
{
    RoundManager roundManager;
    List<string> mapSet1 = new List<string>();
    List<string> mapSet2 = new List<string>();
    List<string> mapSet3 = new List<string>();



    private void Start()
    {

        roundManager = gameObject.GetComponent<RoundManager>();
        ////Map set 1
        mapSet1.Add("2310");
        mapSet1.Add("9999");
        mapSet1.Add("FlipperBox");
        mapSet1.Add("TheDefault");
        //Map set 2
        mapSet2.Add("BarrelRoll");
        mapSet2.Add("CannonShuffle");
        mapSet2.Add("QuadBridge");
        mapSet2.Add("RotatingHoleInWall");
        mapSet2.Add("TheSlice");
        //Map set 3
        mapSet3.Add("Checkers");
        mapSet3.Add("CrushingWall");
        mapSet3.Add("HyperHexagon");
        mapSet3.Add("Rotat-o-Maze");
        mapSet3.Add("TipTheScales");

    }

    private void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            StatHolder.CurrentMode = StatHolder.Modes.DM;
            SceneManager.LoadScene("JoiningScene");
        }
        if (Input.GetKeyDown("t"))
        {
            StatHolder.CurrentMode = StatHolder.Modes.TDM;
            SceneManager.LoadScene("JoiningScene");
        }
        if (Input.GetKeyDown("h"))
        {
            MenuScene();

        }
        if (Input.GetKeyDown("r"))
        {
            ReloadScene();

        }

    }
    

    //Loads the menu scene and sets all win counters to zero
    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
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

    //Assigns a random scene to load (excluding the menu scene)
    public void NewRandomScene()
    {
        SceneManager.LoadScene(Random.Range(1, 12));
    }

    //Loads the next scene in a given set of scenes (arenas)
    public void NextSetScene(int setNumber)
    {
        switch (setNumber)
        {
            case 1:
                if (StatHolder.RoundNumber > mapSet1.Count -1)
                {
                    StatHolder.RoundNumber = 0;
                    //SceneManager.LoadScene(mapSet1[StatHolder.RoundNumber]); Replace the below with this if you want the set to start over once its finished. The below code randomizes a new set(can be the same set)
                    int i = Random.Range(1, 4);
                    NextSetScene(i);
                    StatHolder.WitchSet = i;
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
                    //SceneManager.LoadScene(mapSet2[StatHolder.RoundNumber]); Replace the below with this if you want the set to start over once its finished. The below code randomizes a new set(can be the same set)
                    int i = Random.Range(1, 4);
                    NextSetScene(i);
                    StatHolder.WitchSet = i;
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
                    //SceneManager.LoadScene(mapSet3[StatHolder.RoundNumber]); Replace the below with this if you want the set to start over once its finished. The below code randomizes a new set(can be the same set)
                    int i = Random.Range(1, 4);
                    NextSetScene(i);
                    StatHolder.WitchSet = i;
                }
                else
                {
                    SceneManager.LoadScene(mapSet3[StatHolder.RoundNumber]);
                }
                break;
            default:

                break;
        }
    }
}