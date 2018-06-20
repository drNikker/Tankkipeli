using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoader : MonoBehaviour
{
    RoundManager roundManager;
    //List<String> mapSet1;
    //List<Scene> mapSet2;
    //List<Scene> mapSet3;
    Scene[] mapSet1;
    Scene[] mapSet2;
    Scene[] mapSet3;

    private void Start()
    {
        roundManager = gameObject.GetComponent<RoundManager>();
        ////Map set 1
        mapSet1[1] = SceneManager.GetSceneByBuildIndex(1);
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(1));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(2));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(3));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(4));
        ////Map set 2
        mapSet2[1] = SceneManager.GetSceneByBuildIndex(5);
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(5));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(6));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(7));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(8));
        ////Map set 3
        mapSet3[1] = SceneManager.GetSceneByBuildIndex(9);
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(9));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(10));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(11));
        //mapSet1.Add(SceneManager.GetSceneByBuildIndex(12));

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
        StatHolder.HowManyPlayers = 0;
        StatHolder.Player1Wins = 0;
        StatHolder.Player2Wins = 0;
        StatHolder.WinsNeeded = 0;
        StatHolder.RoundNumber = 1;
        //Destroy(this.gameObject);

    }

    //Reloads the scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        roundManager.RoundStart();
    }

    //Assigns a random scene to load (excluding the menu scene)
    //public void NewRandomScene()
    //{
    //    SceneManager.LoadScene(Random.Range(1,12));
    //}

    //Loads the next scene in a given set of scenes (arenas)
    public void NextSetScene(int setNumber)
    {
        switch (setNumber)
        {
            case 1:
                if (StatHolder.RoundNumber > mapSet1.Length)
                {
                    StatHolder.RoundNumber = 1;
                    SceneManager.LoadScene(mapSet1[StatHolder.RoundNumber].name);
                }
                else
                {
                    SceneManager.LoadScene(mapSet1[StatHolder.RoundNumber].name);

                }
                break;
            case 2:
                if (StatHolder.RoundNumber > mapSet1.Length)
                {
                    StatHolder.RoundNumber = 1;
                    SceneManager.LoadScene(mapSet2[StatHolder.RoundNumber].name);
                }
                else
                {
                    SceneManager.LoadScene(mapSet2[StatHolder.RoundNumber].name);
                }
                break;
            case 3:
                if (StatHolder.RoundNumber > mapSet1.Length)
                {
                    StatHolder.RoundNumber = 1;
                    SceneManager.LoadScene(mapSet3[StatHolder.RoundNumber].name);
                }
                else
                {
                    SceneManager.LoadScene(mapSet3[StatHolder.RoundNumber].name);
                }
                break;
            default:

                break;
        }
    }
}