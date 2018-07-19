﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class RoundManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    AudioScript audioScript;

    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    int playersAlive;

    //bool randomMap = true;
    //bool mapSet = false;
    static Color Red = new Color(0.3962264f, 0.03551085f, 0.08502093f, 1f);
    static Color Blue = new Color(0.115744f, 0.1928815f, 0.4811321f, 0);
    static Color Cyan = new Color(0.05793876f, 0.5849056f, 0.429675f, 1);
    static Color Yellow = new Color(0.9433962f, 0.9042832f, 0.2002492f, 1);
    static Color Green = new Color(0, 0.1886792f, 0.0004195716f, 1);
    static Color Purple = new Color(0.4823529f, 0.1176471f, 0.479214f, 1);
    static Color Orange = new Color(0.8867924f, 0.3786893f, 0.1547704f, 1);
    static Color Lime = new Color(0.4082314f, 0.945098f, 0.2f, 1);
    Color[] colorSet = { Red, Blue, Cyan, Yellow, Green, Purple, Orange, Lime };

    public Text whoWonText;

    public GameObject roundWon;

    public int weaponSpawnTimer;
    [HideInInspector]
    public List<GameObject> alivePlayers;
    [HideInInspector]
    public List<GameObject> spawnPlayers;
    public List<GameObject> weaponSpawns;
    [HideInInspector]
    public List<GameObject> playerSpawns;
    [HideInInspector]
    public List<GameObject> redPlayers;
    [HideInInspector]
    public List<GameObject> bluePlayers;

    private void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
        sceneLoader = gameObject.GetComponent<SceneLoader>();

        if (SceneManager.GetActiveScene().name != "JoiningScene")
        {
            if (StatHolder.HowManyPlayers == 0)
            {
                //This if statement exists for developing purposes. It ensures that the player spawns work even if you dont start at menu
                StatHolder.HowManyPlayers = 2;
            }
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                StatHolder.HowManyPlayers = 0;
            }
            playersForRound();
        }
        if (weaponSpawns.Count > 0)
        {
            StartCoroutine(SpawnWeapon());
        }
    }

    void playersForRound()
    {
        if (StatHolder.HowManyPlayers > 0)
        {
            if (playerSpawns.Count > 0)
            {
                switch (StatHolder.HowManyPlayers)
                {
                    case 2:
                        spawnPlayers.Add(playerPrefab1);
                        spawnPlayers.Add(playerPrefab2);
                        break;
                    case 3:
                        spawnPlayers.Add(playerPrefab1);
                        spawnPlayers.Add(playerPrefab2);
                        spawnPlayers.Add(playerPrefab3);
                        break;
                    case 4:
                        spawnPlayers.Add(playerPrefab1);
                        spawnPlayers.Add(playerPrefab2);
                        spawnPlayers.Add(playerPrefab3);
                        spawnPlayers.Add(playerPrefab4);
                        break;
                }


                for (int i = 0; i < StatHolder.HowManyPlayers; i++)
                {
                    if (playerSpawns.Count > 0 && spawnPlayers.Count > 0)
                    {
                        playerSpawns[Random.Range(0, playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
                    }
                }
                playersAlive = StatHolder.HowManyPlayers;
                RoundStart();
            }
        }
    }

    public void PlayerChecker()
    {
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:
                playersAlive -= 1;
                if (playersAlive == 1)
                {
                    RoundOver();
                }
                break;

            case StatHolder.Modes.TDM:
                if (redPlayers.Count == 0 || bluePlayers.Count == 0)
                {
                    RoundOver();
                }
                break;
        }

    }

    public void NewGame()
    {
        if (StatHolder.HowManyPlayers == 0)
        {
            StatHolder.HowManyPlayers = 2;
        }
        StatHolder.WinsNeeded = 6;
        StatHolder.WitchSet = Random.Range(1, 6);
        audioScript.PlaySceneMusic(StatHolder.WitchSet);
        NewRound();
    }

    public void NewRound()
    {

        //if (randomMap == true)
        //    {
        //        sceneLoader.NewRandomScene();
        //        RoundStart();
        //    }

        //else
        //{
        //    sceneLoader.ReloadScene();
        //}

        switch (StatHolder.WitchSet)
        {
            case 1:
                sceneLoader.NextSetScene(1);
                RoundStart();
                break;
            case 2:
                sceneLoader.NextSetScene(2);
                RoundStart();
                break;
            case 3:
                sceneLoader.NextSetScene(3);
                RoundStart();
                break;
            case 4:
                sceneLoader.NextSetScene(4);
                RoundStart();
                break;
            case 5:
                sceneLoader.NextSetScene(5);
                RoundStart();
                break;
            default:
                sceneLoader.NewRandomScene();
                RoundStart();
                break;
        }

    }

    public void RoundStart()
    {
        //Run an animation showing the arena and how the obstacles function
        //Unfreeze everything
    }

    public void RoundOver()
    {
        audioScript.PlayRoundOverSound();

        //Freeze eveything or do some other kind of ending stuff. Maybe a cool animation?

        //Add a win to the player/team who won the round and announce that winner
        int c = 5;
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:

                switch (alivePlayers[0].name)
                {
                    case "Player1(Clone)":
                        StatHolder.Player1Wins += 1;
                        c = StatHolder.Player1Color;
                        break;
                    case "Player2(Clone)":
                        StatHolder.Player2Wins += 1;
                        c = StatHolder.Player2Color;
                        break;
                    case "Player3(Clone)":
                        StatHolder.Player3Wins += 1;
                        c = StatHolder.Player3Color;
                        break;
                    case "Player4(Clone)":
                        StatHolder.Player4Wins += 1;
                        c = StatHolder.Player4Color;
                        break;
                    default:
                        print("This should never happen");
                        break;
                }
                break;

            case StatHolder.Modes.TDM:

                if (redPlayers.Count == 0)
                {
                    StatHolder.TeamBlueWins += 1;
                    whoWonText.text = "Team Blue won the round";
                }
                if (bluePlayers.Count == 0)
                {
                    StatHolder.TeamRedWins += 1;
                    whoWonText.text = "Team Red won the round";
                }
                break;
        }

        if (StatHolder.CurrentMode == StatHolder.Modes.DM)
        {
            switch(c)
            {
                case 0:
                    whoWonText.text = "Red player won the round";
                    break;
                case 1:
                    whoWonText.text = "Blue player won the round";
                    break;
                case 2:
                    whoWonText.text = "Cyan player won the round";
                    break;
                case 3:
                    whoWonText.text = "Yellow player won the round";
                    break;
                case 4:
                    whoWonText.text = "Green player won the round";
                    break;
                case 5:
                    whoWonText.text = "Purple player won the round";
                    break;
                case 6:
                    whoWonText.text = "Orange player won the round";
                    break;
                case 7:
                    whoWonText.text = "Lime player won the round";
                    break;

            }

        }

        roundWon.SetActive(true);

        //Check if anyone has enough points to win the game and announce the winner if they do
        if (StatHolder.Player1Wins >= StatHolder.WinsNeeded || StatHolder.Player2Wins >= StatHolder.WinsNeeded || StatHolder.Player3Wins >= StatHolder.WinsNeeded || StatHolder.Player4Wins >= StatHolder.WinsNeeded || StatHolder.TeamRedWins >= StatHolder.WinsNeeded || StatHolder.TeamBlueWins >= StatHolder.WinsNeeded)
        {
            //Game is over
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:

                    switch (c)
                    {
                        case 0:
                            whoWonText.text = "Red player won the game";
                            break;
                        case 1:
                            whoWonText.text = "Blue player won the game";
                            break;
                        case 2:
                            whoWonText.text = "Cyan player won the game";
                            break;
                        case 3:
                            whoWonText.text = "Yellow player won the game";
                            break;
                        case 4:
                            whoWonText.text = "Green player won the game";
                            break;
                        case 5:
                            whoWonText.text = "Purple player won the game";
                            break;
                        case 6:
                            whoWonText.text = "Orange player won the game";
                            break;
                        case 7:
                            whoWonText.text = "Lime player won the game";
                            break;
                    }
                    break;

                case StatHolder.Modes.TDM:
                    if (redPlayers.Count == 0)
                    {
                        whoWonText.text = "Team Blue won the game";
                    }
                    else if (bluePlayers.Count == 0)
                    {
                        whoWonText.text = "Team Red won the game";
                    }
                    break;
            }
            StartCoroutine(BackToMenu());
        }
        //If no one has enough wins, start a new round
        else
        {
            StartCoroutine(NextRound());
        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(5);
        roundWon.SetActive(false);
        StatHolder.RoundNumber += 1;
        NewRound();
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        roundWon.SetActive(false);
        audioScript.StopPlayingSceneMusic();
        sceneLoader.MenuScene();
    }

    IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(weaponSpawnTimer);

        int i = Random.Range(0, weaponSpawns.Count);
        weaponSpawns[i].GetComponent<WeaponSpawn>().CreateWeapon();
        StartCoroutine(SpawnWeapon());

    }
}
