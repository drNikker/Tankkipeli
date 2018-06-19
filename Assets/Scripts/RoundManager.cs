using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    SceneLoader sceneLoader;
    int winsNeeded;
    int playersAlive;

    bool randomMap = true;
    //bool mapSet = false;

    public GameObject roundWon;
    

    private void Start()
    {
        sceneLoader = gameObject.GetComponent<SceneLoader>();
        
    }

    public void playerChecker()
    {
        playersAlive -= 1;
        if (playersAlive <= 1)
        {
            RoundOver();
        }
    }

    public void newGame()
    {
        StatHolder.HowManyPlayers = 2;
        NewRound();
        winsNeeded = 3;
    }

    public void NewRound()
    {

        //Replace this with switch case function where cases are different mapsets or random map and default is reload?

        if (randomMap == true)
            {
                sceneLoader.NewRandomScene();
                RoundStart();
            }

        else
        {
            sceneLoader.ReloadScene();
        }

        //This may be used if we have a set of maps that the players have chosen
        //if (mapSet)
        //{

        //    RoundStart();
        //}
        playersAlive = StatHolder.HowManyPlayers;
    }

    public void RoundStart()
    {
        //Run an animation showing the arena and how the obstacles function
        //Unfreeze everything
    }

    public void RoundOver()
    {
        //Freeze eveything or do some other kind of ending stuff



        //Change this to accomodate the corresponding player
        StatHolder.Player1Wins += 1;

        if (StatHolder.Player1Wins >= winsNeeded || StatHolder.Player2Wins >= winsNeeded)
        {
            //Game is over
            roundWon.SetActive(true);
            StartCoroutine(BackToMenu());
        }
        else
        {
            roundWon.SetActive(true);
            StartCoroutine(NextRound());
        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(5);
        roundWon.SetActive(false);
        NewRound();
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        roundWon.SetActive(false);
        sceneLoader.MenuScene();
    }
}
