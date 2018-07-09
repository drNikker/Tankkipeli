﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoundManager : MonoBehaviour {
    SceneLoader sceneLoader;

    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    int playersAlive;
    int teamRedPlayersAlive;
    int teamBluePlayersAlive;

    //bool randomMap = true;
    //bool mapSet = false;

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


    private void Start()
    {

        playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
        sceneLoader = gameObject.GetComponent<SceneLoader>();
        
        if (SceneManager.GetActiveScene().name != "JoiningScene" )
        {
            if (StatHolder.HowManyPlayers == 0)
            {
                //This if statement exists for developing purposes. It ensures that the player spawns work even if you dont start at menu
                StatHolder.HowManyPlayers = 2;
            }
            if(SceneManager.GetActiveScene().name == "Menu")
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

    public void playerChecker()
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
                //if (alivePlayers.Except(Color.red).Any())
                //{
                //    RoundOver();
                //}
                break;
        }
    }

    public void newGame()
    {
        if (StatHolder.HowManyPlayers == 0)
        {
            StatHolder.HowManyPlayers = 2;
        }
        StatHolder.WinsNeeded = 6;
        StatHolder.WitchSet = Random.Range(1, 4);
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
        //Freeze eveything or do some other kind of ending stuff. Maybe a cool animation?

        switch (alivePlayers[0].name)
            {
            case "Player1(Clone)":
                StatHolder.Player1Wins += 1;
                break;
            case "Player2(Clone)":
                StatHolder.Player2Wins += 1;
                break;
            case "Player3(Clone)":
                StatHolder.Player3Wins += 1;
                break;
            case "Player4(Clone)":
                StatHolder.Player4Wins += 1;
                break;
            default:
                print("This should never happen");
                break;
        }

        roundWon.SetActive(true);

        if (StatHolder.Player1Wins >= StatHolder.WinsNeeded || StatHolder.Player2Wins >= StatHolder.WinsNeeded || StatHolder.Player3Wins >= StatHolder.WinsNeeded || StatHolder.Player4Wins >= StatHolder.WinsNeeded)
        {
            //Game is over
            switch (alivePlayers[0].name)
            {
                case "Player1(Clone)":
                    whoWonText.text = "Player 1 won the game";
                    break;
                case "Player2(Clone)":
                    whoWonText.text = "Player 2 won the game";
                    break;
                case "Player3(Clone)":
                    whoWonText.text = "Player 3 won the game";
                    break;
                case "Player4(Clone)":
                    whoWonText.text = "Player 4 won the game";
                    break;
                default:
                    print("This should never happen");
                    break;
            }
            StartCoroutine(BackToMenu());
        }
        else
        {
            switch (alivePlayers[0].name)
            {
                case "Player1(Clone)":
                    whoWonText.text = "Player 1 won the round";
                    break;
                case "Player2(Clone)":
                    whoWonText.text = "Player 2 won the round";
                    break;
                case "Player3(Clone)":
                    whoWonText.text = "Player 3 won the round";
                    break;
                case "Player4(Clone)":
                    whoWonText.text = "Player 4 won the round";
                    break;
                default:
                    print("This should never happen");
                    break;
            }
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
