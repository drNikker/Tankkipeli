using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XInputDotNetPure;

public class PlayerJoining : MonoBehaviour {

    MultiTargetCamera LevelCam;

    AudioScript audioScript;
    RoundManager roundManager;
    

    bool joined1;
    bool joined2;
    bool joined3;
    bool joined4;
    bool wait1;
    bool wait2;
    bool wait3;
    bool wait4;

    public GameObject TDMText;
    public GameObject DMText;

    bool gameStarting = false;
    bool startTimerOn = false;
    int totalPlayersInHoles = 0;
    float timer;

    //states for all 4 players' controllers because there is only one instance of this script.
    GamePadState P1state;
    GamePadState P1prevState;
    GamePadState P2state;
    GamePadState P2prevState;
    GamePadState P3state;
    GamePadState P3prevState;
    GamePadState P4state;
    GamePadState P4prevState;

    GamemodeHole[] holes;
    public List<int> votes;

    StatHolder.Modes chosenMode;

    // Use this for initialization
    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        LevelCam = GameObject.FindWithTag("MainCamera").GetComponent<MultiTargetCamera>();
        StatHolder.CurrentMode = StatHolder.Modes.DM;
        StatHolder.HowManyPlayers = 0;
        StatHolder.Player1Wins = 0;
        StatHolder.Player2Wins = 0;
        StatHolder.Player3Wins = 0;
        StatHolder.Player4Wins = 0;
        StatHolder.TeamRedWins = 0;
        StatHolder.TeamBlueWins = 0;
        StatHolder.WinsNeeded = 0;
        StatHolder.RoundNumber = 0;
        StatHolder.Player1Color = 100;
        StatHolder.Player2Color = 100;
        StatHolder.Player3Color = 100;
        StatHolder.Player4Color = 100;

        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        audioScript.PlaySceneMusic(0);

        votes = new List<int>();
        votes.Add(0);
        votes.Add(0);
    }

    // Update is called once per frame
    void Update ()
    {
        //setting previous state and new state
        P1prevState = P1state;
        P2prevState = P2state;
        P3prevState = P3state;
        P4prevState = P4state;
        P1state = GamePad.GetState(PlayerIndex.One);
        P2state = GamePad.GetState(PlayerIndex.Two);
        P3state = GamePad.GetState(PlayerIndex.Three);
        P4state = GamePad.GetState(PlayerIndex.Four);

        keyPresses();
        GameStarter();
        StartTimer();
    }

    void keyPresses()
    {
        //Makes sure that players are always able to spawn
        if (roundManager.playerSpawns.Count == 0)
        {
            roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
        }

        //Player 1 join
        if (joined1 == false && wait1 == false && P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released || joined1 == false && Input.GetKeyDown("g"))
        {
            wait1 = true;
            StartCoroutine(Player1Joined());
            StatHolder.HowManyPlayers++;


            roundManager.spawnPlayers.Add(roundManager.playerPrefab1);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }


        if (joined1 == true && P1state.Buttons.B == ButtonState.Pressed && P1prevState.Buttons.B == ButtonState.Released)
        {
            joined1 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player1(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player1(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player1(Clone)").SingleOrDefault());
        }


        //Player 2 join
        if (joined2 == false && wait2 == false && P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released || joined2 == false && Input.GetKeyDown("f"))
        {
            wait2 = true;
            StartCoroutine(Player2Joined());
            StatHolder.HowManyPlayers++;


            roundManager.spawnPlayers.Add(roundManager.playerPrefab2);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }



        if (joined2 == true && P2state.Buttons.B == ButtonState.Pressed && P2prevState.Buttons.B == ButtonState.Released)
        {
            joined2 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player2(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player2(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player2(Clone)").SingleOrDefault());
        }

        //Player 3 join
        if (joined3 == false && wait3 == false && P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released || joined3 == false && Input.GetKeyDown("d"))
        {
            wait3 = true;
            StartCoroutine(Player3Joined());
            StatHolder.HowManyPlayers++;


            roundManager.spawnPlayers.Add(roundManager.playerPrefab3);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }


        if (joined3 == true && P3state.Buttons.B == ButtonState.Pressed && P3prevState.Buttons.B == ButtonState.Released)
        {
            joined3 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player3(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player3(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player3(Clone)").SingleOrDefault());
        }

        //Player 4 join
        if (joined4 == false && wait4 == false && P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released || joined4 == false && Input.GetKeyDown("s"))
        {
            wait4 = true;
            StatHolder.HowManyPlayers++;
            StartCoroutine(Player4Joined());

            roundManager.spawnPlayers.Add(roundManager.playerPrefab4);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }



        if (joined4 == true && P4state.Buttons.B == ButtonState.Pressed && P4prevState.Buttons.B == ButtonState.Released)
        {
            joined4 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player4(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player4(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player4(Clone)").SingleOrDefault());
        }
       
    }

    void GameStarter()
    {
        CheckPlayersInHoles();
        if (StatHolder.HowManyPlayers >= 2 && gameStarting == false)
        {
            if (StatHolder.HowManyPlayers == totalPlayersInHoles)
            {
                int mode = Random.Range(1, totalPlayersInHoles + 1);
                int chosen;
                int i = 0;

                for (chosen = 0; mode > i; chosen++)
                {
                    i += votes[chosen];
                }
                chosenMode = holes[chosen - 1].mode;
                timer = 3;
                gameStarting = true;
                startTimerOn = true;
            }
        }
    }

    IEnumerator Player1Joined()
    {
        yield return new WaitForSeconds(1);
        wait1 = false;
        joined1 = true;
    }
    IEnumerator Player2Joined()
    {
        yield return new WaitForSeconds(1);
        wait2 = false;
        joined2 = true;
    }
    IEnumerator Player3Joined()
    {
        yield return new WaitForSeconds(1);
        wait3 = false;
        joined3 = true;
    }
    IEnumerator Player4Joined()
    {
        yield return new WaitForSeconds(1);
        wait4 = false;
        joined4 = true;
    }

    void StartTimer()
    {
        if (gameStarting == true && startTimerOn == true)
        {
            CheckPlayersInHoles();
            if (totalPlayersInHoles != StatHolder.HowManyPlayers)
            {
                gameStarting = false;
                startTimerOn = false;
                return;
            }
            print(timer);
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                StatHolder.CurrentMode = chosenMode;
                TDMTeamAssigner();
                startTimerOn = false;
                if (chosenMode == StatHolder.Modes.DM)
                {
                    Instantiate(DMText, new Vector3(0, 40, 0), Quaternion.identity);
                }
                else if(chosenMode == StatHolder.Modes.TDM)
                {
                    Instantiate(TDMText, new Vector3(0, 40, 0), Quaternion.identity);
                }
                Invoke("StartGame", 4);
            }
        }
    }

    void StartGame()
    {
        roundManager.NewGame();
    }

    void CheckPlayersInHoles()
    {
        votes = new List<int>();
        holes = GetComponentsInChildren<GamemodeHole>();
        totalPlayersInHoles = 0;

        for (int i = 0; i < holes.Length; i++)
        {
            totalPlayersInHoles += holes[i].playersInHole;
            votes.Add(holes[i].playersInHole);
        }
    }

    void TDMTeamAssigner()
    {
        if (StatHolder.CurrentMode == StatHolder.Modes.TDM)
        {
            if (StatHolder.HowManyPlayers == 2)
            {
                StatHolder.Player1Color = 0;
                StatHolder.Player1SkinColor = 0;
                StatHolder.Player2Color = 1;
                StatHolder.Player2SkinColor = 0;
            }
            else
            {
                StatHolder.Player1Color = Random.Range(0, 2);
                StatHolder.Player2Color = Random.Range(0, 2);
            }
            if (StatHolder.Player1Color == 0 && StatHolder.Player2Color == 0)
            {
                StatHolder.Player1SkinColor = 0;
                StatHolder.Player2SkinColor = 1;
                StatHolder.Player3Color = 1;
                StatHolder.Player3SkinColor = 0;
                StatHolder.Player4Color = 1;
                StatHolder.Player4SkinColor = 1;
            }
            else if (StatHolder.Player1Color == 1 && StatHolder.Player2Color == 1)
            {
                StatHolder.Player1SkinColor = 0;
                StatHolder.Player2SkinColor = 1;
                StatHolder.Player3Color = 0;
                StatHolder.Player3SkinColor = 0;
                StatHolder.Player4Color = 0;
                StatHolder.Player4SkinColor = 1;
            }
            else
            {
                StatHolder.Player1SkinColor = 0;
                StatHolder.Player2SkinColor = 0;
                StatHolder.Player3SkinColor = 1;
                StatHolder.Player4SkinColor = 1;
                StatHolder.Player3Color = Random.Range(0, 2);
                StatHolder.Player4Color = Random.Range(0, 2);
                while (StatHolder.Player3Color == StatHolder.Player4Color)
                {
                    StatHolder.Player4Color = Random.Range(0, 2);
                }
            }

        }
    }


}
