using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    SceneLoader sceneLoader;

    int winsNeeded = 3;
    int playersAlive;

    bool roundIsOver = true;
    bool randomMap = true;
    //bool mapSet = false;

    public GameObject roundWon;
    

    private void Start()
    {
        sceneLoader = gameObject.GetComponent<SceneLoader>();
        
    }
    private void Update()
    {
        if (playersAlive <= 1 & roundIsOver == false)
        {
            roundIsOver = true;
            RoundOver();
        }
    }

    public void newGame()
    {
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
            GameOver();
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
        playersAlive = 2;
        roundIsOver = false;
    }

    public void RoundStart()
    {
        //Run an animation showing the arena and how the obstacles function
        //Unfreeze everything
    }

    public void RoundOver()
    {
        //Freeze eveything

        if(StatHolder.Player1Wins >= winsNeeded || StatHolder.Player2Wins >= winsNeeded)
        {
            GameOver();
        }
        else
        {
            roundWon.SetActive(true);
            NewRound();
        }
    }

    public void GameOver()
    {
        roundWon.SetActive(true);
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        sceneLoader.MenuScene();
    }
}
