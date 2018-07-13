using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XInputDotNetPure;

public class PlayerJoining : MonoBehaviour {

    MultiTargetCamera LevelCam;

    RoundManager roundManager;

    Color[] colorSet = { Color.red, Color.blue, Color.green, Color.yellow, Color.white};
    //bool teamDeathMatch = false;

    bool joined1;
    bool joined2;
    bool joined3;
    bool joined4;

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
    List<int> votes;

    // Use this for initialization
    void Start ()
    {
        LevelCam = GameObject.FindWithTag("MainCamera").GetComponent<MultiTargetCamera>();

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

        roundManager = GameObject.Find("GameManager").GetComponent<RoundManager>();
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
    }

    void keyPresses()
    {
        //Makes sure that players are always able to spawn
        if (roundManager.playerSpawns.Count == 0)
        {
            roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
        }

        //Player 1 join and color change
        if (joined1 == false && P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released || joined1 == false && Input.GetKeyDown("g"))
        {
            joined1 = true;
            StatHolder.HowManyPlayers++;
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:
                    StatHolder.Player1Color = Random.Range(0, 5);
                    while (StatHolder.Player1Color == StatHolder.Player2Color || StatHolder.Player1Color == StatHolder.Player3Color || StatHolder.Player1Color == StatHolder.Player4Color)
                    {
                        StatHolder.Player1Color = Random.Range(0, 5);
                    }
                    break;

                case StatHolder.Modes.TDM:
                    StatHolder.Player1Color = Random.Range(0, 2);
                    break;
            }
            roundManager.spawnPlayers.Add(roundManager.playerPrefab1);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }

        else if (joined1 == true && P1state.Buttons.A == ButtonState.Pressed && P1prevState.Buttons.A == ButtonState.Released || joined1 == true && Input.GetKeyDown("g"))
        {
            ChangePlayer1Color();
        }
        if (joined1 == true && P1state.Buttons.B == ButtonState.Pressed && P1prevState.Buttons.B == ButtonState.Released)
        {
            joined1 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player1(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player1(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player1(Clone)").SingleOrDefault());
        }


        //Player 2 join and color change
        if (joined2 == false && P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released || joined2 == false && Input.GetKeyDown("f"))
        {
            joined2 = true;
            StatHolder.HowManyPlayers++;
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:
                    StatHolder.Player2Color = Random.Range(0, 5);
                    while (StatHolder.Player2Color == StatHolder.Player1Color || StatHolder.Player2Color == StatHolder.Player3Color || StatHolder.Player2Color == StatHolder.Player4Color)
                    {
                        StatHolder.Player2Color = Random.Range(0, 5);
                    }
                    break;

                case StatHolder.Modes.TDM:
                    StatHolder.Player2Color = Random.Range(0, 2);
                    break;
            }
            roundManager.spawnPlayers.Add(roundManager.playerPrefab2);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }
        else if (joined2 == true && P2state.Buttons.A == ButtonState.Pressed && P2prevState.Buttons.A == ButtonState.Released)
        {
            ChangePlayer2Color();
        }
        if (joined2 == true && P2state.Buttons.B == ButtonState.Pressed && P2prevState.Buttons.B == ButtonState.Released)
        {
            joined2 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player2(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player2(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player2(Clone)").SingleOrDefault());
        }

        //Player 3 join and color change
        if (joined3 == false && P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released)
        {
            joined3 = true;
            StatHolder.HowManyPlayers++;
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:
                    StatHolder.Player3Color = Random.Range(0, 5);
                    while (StatHolder.Player3Color == StatHolder.Player2Color || StatHolder.Player3Color == StatHolder.Player1Color || StatHolder.Player3Color == StatHolder.Player4Color)
                    {
                        StatHolder.Player3Color = Random.Range(0, 5);
                    }
                    break;

                case StatHolder.Modes.TDM:
                    StatHolder.Player3Color = Random.Range(0, 2);
                    break;
            }
            roundManager.spawnPlayers.Add(roundManager.playerPrefab3);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }
        else if (joined3 == true && P3state.Buttons.A == ButtonState.Pressed && P3prevState.Buttons.A == ButtonState.Released)
        {
            ChangePlayer3Color();
        }
        if (joined3 == true && P3state.Buttons.B == ButtonState.Pressed && P3prevState.Buttons.B == ButtonState.Released)
        {
            joined3 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player3(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player3(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player3(Clone)").SingleOrDefault());
        }

        //Player 4 join and color change
        if (joined4 == false && P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released)
        {
            joined4 = true;
            StatHolder.HowManyPlayers++;
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:
                    StatHolder.Player4Color = Random.Range(0, 5);
                    while (StatHolder.Player4Color == StatHolder.Player2Color || StatHolder.Player4Color == StatHolder.Player3Color || StatHolder.Player4Color == StatHolder.Player1Color)
                    {
                        StatHolder.Player4Color = Random.Range(0, 5);
                    }
                    break;

                case StatHolder.Modes.TDM:
                    StatHolder.Player3Color = Random.Range(0, 2);
                    break;
            }
            roundManager.spawnPlayers.Add(roundManager.playerPrefab4);
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }
        else if (joined4 == true && P4state.Buttons.A == ButtonState.Pressed && P4prevState.Buttons.A == ButtonState.Released)
        {
            ChangePlayer4Color();
        }
        if (joined4 == true && P4state.Buttons.B == ButtonState.Pressed && P4prevState.Buttons.B == ButtonState.Released)
        {
            joined4 = false;
            StatHolder.HowManyPlayers--;
            LevelCam.RemoveTarget("Player4(Clone)");
            Destroy(roundManager.alivePlayers.Where(obj => obj.name == "Player4(Clone)").SingleOrDefault());
            roundManager.alivePlayers.Remove(roundManager.alivePlayers.Where(obj => obj.name == "Player4(Clone)").SingleOrDefault());
        }


        if (StatHolder.HowManyPlayers >= 2 && P1state.Buttons.Start == ButtonState.Pressed && P1prevState.Buttons.Start == ButtonState.Released || StatHolder.HowManyPlayers >= 2 && Input.GetKeyDown("y"))
        {

            votes = new List<int>();
            holes = GetComponentsInChildren<GamemodeHole>();
            int totalPlayersInHoles = 0;
            
            for(int i =0; i < holes.Length; i++)
            {
                totalPlayersInHoles += holes[i].playersInHole;
                votes.Add(holes[i].playersInHole);
            }
            if(StatHolder.HowManyPlayers == totalPlayersInHoles)
            {
                int mode = Random.Range(1,totalPlayersInHoles+1);
                int chosen;
                int i = 0;

                for (chosen = 0; mode > i; chosen++)
                {
                    i += votes[chosen];
                }
                StatHolder.CurrentMode = holes[chosen-1].mode;
                roundManager.NewGame();
            }
        }
    }

    void ChangePlayer1Color()
    {
        int save = Random.Range(0, 5);
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:

                while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
                {
                    save = Random.Range(0, 5);
                }
                break;

            case StatHolder.Modes.TDM:
                save = Random.Range(0, 2);
                while (save == StatHolder.Player1Color)
                {
                    save = Random.Range(0, 2);
                }
                break;
        }
        StatHolder.Player1Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers.Where(obj => obj.name == "Player1(Clone)").SingleOrDefault();
        print(player);
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        rend[2].SetPropertyBlock(_propBlock);
    }
    void ChangePlayer2Color()
    {
        int save = Random.Range(0, 5);
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:
                while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
                {
                    save = Random.Range(0, 5);
                }
                break;

            case StatHolder.Modes.TDM:
                save = Random.Range(0, 2);
                while (save == StatHolder.Player2Color)
                {
                    save = Random.Range(0, 2);
                }
                break;
        }
        StatHolder.Player2Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers.Where(obj => obj.name == "Player2(Clone)").SingleOrDefault();
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        rend[2].SetPropertyBlock(_propBlock); 
    }
    void ChangePlayer3Color()
    {
        int save = Random.Range(0, 5);
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:
                while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
                {
                    save = Random.Range(0, 5);
                }
                break;

            case StatHolder.Modes.TDM:
                save = Random.Range(0, 2);
                while (save == StatHolder.Player3Color)
                {
                    save = Random.Range(0, 2);
                }
                break;
        }
        StatHolder.Player3Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers.Where(obj => obj.name == "Player3(Clone)").SingleOrDefault();
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        rend[2].SetPropertyBlock(_propBlock);
    }
    void ChangePlayer4Color()
    {
        int save = Random.Range(0, 5);
        switch (StatHolder.CurrentMode)
        {
            case StatHolder.Modes.DM:
                while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
                {
                    save = Random.Range(0, 5);
                }
                break;

            case StatHolder.Modes.TDM:
                save = Random.Range(0, 2);
                while (save == StatHolder.Player4Color)
                {
                    save = Random.Range(0, 2);
                }
                break;
        }
        StatHolder.Player4Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers.Where(obj => obj.name == "Player4(Clone)").SingleOrDefault();
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        rend[2].SetPropertyBlock(_propBlock); 
    }
}
