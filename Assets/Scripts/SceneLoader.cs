using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoader : MonoBehaviour
{
    RoundManager roundManager;

    

    private void Start()
    {
        roundManager = gameObject.GetComponent<RoundManager>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            roundManager.newGame();
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
        StatHolder.Player1Wins = 0;
        StatHolder.Player2Wins = 0;
        Destroy(this.gameObject);

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
        SceneManager.LoadScene(Random.Range(1,3));
    }

    //Loads the next scene in a given set of scenes (arenas)
    //public void NextSetScene()
    //{
    //    roundManager.newRound();
    //}

}