using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {
    SceneLoader sceneLoader;
    
    
    int playersAlive;

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
    public List<GameObject> playerSpawns;

    private void Start()
    {
        sceneLoader = gameObject.GetComponent<SceneLoader>();
        playersAlive = 2;
    }

    public void playerChecker()
    {
        playersAlive -= 1;
        print(playersAlive);
        if (playersAlive <= 1)
        {
            RoundOver();
        }
    }

    public void newGame()
    {
        StatHolder.HowManyPlayers = 2;
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

                playersAlive = StatHolder.HowManyPlayers;
        if (weaponSpawns.Count > 0)
        {
            StartCoroutine(SpawnWeapon());
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


        switch(alivePlayers[0].name)
            {
            case "Player1":
                StatHolder.Player1Wins += 1;
                break;
            case "Player2":
                StatHolder.Player2Wins += 1;
                break;
            default:
                print("This should never happen");
                break;
        }

        print("wins " + StatHolder.Player2Wins);
        print("needed " + StatHolder.WinsNeeded);
        roundWon.SetActive(true);

        if (StatHolder.Player1Wins >= StatHolder.WinsNeeded || StatHolder.Player2Wins >= StatHolder.WinsNeeded)
        {
            //Game is over
            switch (alivePlayers[0].name)
            {
                case "Player1":
                    whoWonText.text = "Player1 won the game";
                    break;
                case "Player2":
                    whoWonText.text = "Player 2 won the game";
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
                case "Player1":
                    whoWonText.text = "Player 1 won the round";
                    break;
                case "Player2":
                    whoWonText.text = "Player 2 won the round";
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

        int i = Random.Range(0, weaponSpawns.Count + 1);
        weaponSpawns[i].GetComponent<WeaponSpawn>().CreateWeapon();
        
    }
}
