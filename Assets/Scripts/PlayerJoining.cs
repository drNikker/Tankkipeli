using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoining : MonoBehaviour {

    RoundManager roundManager;

    Color[] colorSet = { Color.red, Color.blue, Color.green, Color.yellow, Color.white};
    //bool teamDeathMatch = false;

    bool joined1;
    bool joined2;
    bool joined3;
    bool joined4;


    // Use this for initialization
    void Start ()
    {
        StatHolder.HowManyPlayers = 0;
        StatHolder.Player1Wins = 0;
        StatHolder.Player2Wins = 0;
        StatHolder.Player3Wins = 0;
        StatHolder.Player4Wins = 0;
        StatHolder.WinsNeeded = 0;
        StatHolder.RoundNumber = 0;
        StatHolder.Player1Color = 0;
        StatHolder.Player2Color = 1;
        StatHolder.Player3Color = 2;
        StatHolder.Player4Color = 3;
        roundManager = GameObject.Find("GameManager").GetComponent<RoundManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        keyPresses();
    }

    void keyPresses()
    {
        //Player 1 join and color change
        if (joined1 == false && Input.GetButtonDown("P1Join") || joined1 == false && Input.GetKeyDown("g"))
        {
            joined1 = true;
            StatHolder.HowManyPlayers++;
            roundManager.spawnPlayers.Add(roundManager.playerPrefab1);
            if (roundManager.playerSpawns.Count == 0)
            {
                roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
            }
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
            //ChangeStatHolder.Player1Color();
        }
        else if (joined1 == true && Input.GetButtonDown("P1Join") || joined1 == true && Input.GetKeyDown("g"))
        {
            ChangePlayer1Color();
        }
        if (joined1 == true && Input.GetButtonDown("P1B"))
        {
            joined1 = false;
            StatHolder.HowManyPlayers--;
            Destroy(roundManager.alivePlayers[0]);
            roundManager.alivePlayers.Remove(roundManager.alivePlayers[0]);
        }


        //Player 2 join and color change
        if (joined2 == false && Input.GetButtonDown("P1Join") || joined2 == false && Input.GetKeyDown("g"))
        {
            joined2 = true;
            StatHolder.HowManyPlayers++;
            roundManager.spawnPlayers.Add(roundManager.playerPrefab2);
            if (roundManager.playerSpawns.Count == 0)
            {
                roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
            }
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
            //ChangeStatHolder.Player2Color();
        }
        else if (joined2 == true && Input.GetButtonDown("P2Join"))
        {
            ChangePlayer2Color();
        }
        if (joined2 == true && Input.GetButtonDown("P2B"))
        {
            joined2 = false;
            StatHolder.HowManyPlayers--;
            Destroy(roundManager.alivePlayers[1]);
            roundManager.alivePlayers.Remove(roundManager.alivePlayers[1]);
        }

        //Player 3 join and color change
        if (joined3 == false && Input.GetButtonDown("P3Join"))
        {
            joined3 = true;
            StatHolder.HowManyPlayers++;
            roundManager.spawnPlayers.Add(roundManager.playerPrefab3);
            if (roundManager.playerSpawns.Count == 0)
            {
                roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
            }
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
            //ChangeStatHolder.Player3Color();
        }
        else if (joined3 == true && Input.GetButtonDown("P3Join"))
        {
            ChangePlayer3Color();
        }
        if (joined3 == true && Input.GetButtonDown("P3B"))
        {
            joined3 = false;
            StatHolder.HowManyPlayers--;
            Destroy(roundManager.alivePlayers[2]);
            roundManager.alivePlayers.Remove(roundManager.alivePlayers[2]);
        }

        //Player 4 join and color change
        if (joined4 == false && Input.GetButtonDown("P4Join"))
        {
            joined4 = true;
            StatHolder.HowManyPlayers++;
            roundManager.spawnPlayers.Add(roundManager.playerPrefab4);
            if (roundManager.playerSpawns.Count == 0)
            {
                roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
            }
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
            //ChangeStatHolder.Player4Color();
        }
        else if (joined4 == true && Input.GetButtonDown("P4Join"))
        {
            ChangePlayer4Color();
        }
        if (joined4 == true && Input.GetButtonDown("P4B"))
        {
            joined4 = false;
            StatHolder.HowManyPlayers--;
            Destroy(roundManager.alivePlayers[3]);
            roundManager.alivePlayers.Remove(roundManager.alivePlayers[3]);
        }


        if (StatHolder.HowManyPlayers >= 2 && Input.GetButtonDown("P1Start") || StatHolder.HowManyPlayers >= 2 && Input.GetKeyDown("y"))
        {
            roundManager.newGame();
        }
    }

    void ChangePlayer1Color()
    {
        int save = Random.Range(0, 5);
        while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
        {

            save = Random.Range(0, 5);
        }
        StatHolder.Player1Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers[0];
        print(roundManager.alivePlayers[0]);
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        //rend[2].SetPropertyBlock(_propBlock); Player torso color
    }
    void ChangePlayer2Color()
    {
        int save = Random.Range(0, 5);
        while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
        {
            save = Random.Range(0, 5);
        }
        StatHolder.Player2Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers[1];
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", Random.ColorHSV());
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        //rend[2].SetPropertyBlock(_propBlock); Player torso color
    }
    void ChangePlayer3Color()
    {
        int save = Random.Range(0, 5);
        while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
        {
            save = Random.Range(0, 5);
        }
        StatHolder.Player3Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers[2];
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", Random.ColorHSV());
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        //rend[2].SetPropertyBlock(_propBlock); Player torso color
    }
    void ChangePlayer4Color()
    {
        int save = Random.Range(0, 5);
        while (save == StatHolder.Player1Color || save == StatHolder.Player2Color || save == StatHolder.Player3Color || save == StatHolder.Player4Color)
        {
            save = Random.Range(0, 5);
        }
        StatHolder.Player4Color = save;
        Color color = colorSet[save];
        GameObject player = roundManager.alivePlayers[3];
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", Random.ColorHSV());
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        //rend[2].SetPropertyBlock(_propBlock); Player torso color
    }
}
