using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoining : MonoBehaviour {

    RoundManager roundManager;

    //bool teamDeathMatch = false;

    bool joined1;
    bool joined2;
    bool joined3;
    bool joined4;

    // Use this for initialization
    void Start ()
    {
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
        if (joined1 == false && Input.GetButtonDown("P1Join"))
        {
            joined1 = true;
            StatHolder.HowManyPlayers ++;
            roundManager.spawnPlayers.Add(roundManager.playerPrefab1);
            if (roundManager.playerSpawns.Count == 0)
            {
                roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
            }
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }
        else if (joined1 == true && Input.GetButtonDown("P1Join"))
        {
            Color color = Random.ColorHSV();
            GameObject player = roundManager.alivePlayers[0];
            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            Renderer[] rend = player.GetComponentsInChildren<Renderer>();
            rend[0].GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", color);
            rend[0].SetPropertyBlock(_propBlock);
            rend[1].SetPropertyBlock(_propBlock);
            rend[2].SetPropertyBlock(_propBlock);
            Vector4 savedColor = color;
            StatHolder.Player1Color[0] = savedColor[0];
            StatHolder.Player1Color[1] = savedColor[1];
            StatHolder.Player1Color[2] = savedColor[2];
            StatHolder.Player1Color[3] = savedColor[3];



        }
        if (joined1 == true && Input.GetButtonDown("P1B"))
        {
            joined1 = false;
            StatHolder.HowManyPlayers--;
            Destroy(roundManager.alivePlayers[0]);
            roundManager.alivePlayers.Remove(roundManager.alivePlayers[0]);
        }


        //Player 2 join and color change
        if (joined2 == false && Input.GetButtonDown("P1Join"))
        {
            joined2 = true;
            StatHolder.HowManyPlayers++;
            roundManager.spawnPlayers.Add(roundManager.playerPrefab2);
            if (roundManager.playerSpawns.Count == 0)
            {
                roundManager.playerSpawns.AddRange(GameObject.FindGameObjectsWithTag("playerSpawn"));
            }
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }
        else if (joined2 == true && Input.GetButtonDown("P2Join"))
        {
            GameObject player = roundManager.alivePlayers[1];
            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            Renderer[] rend = player.GetComponentsInChildren<Renderer>();
            rend[0].GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Random.ColorHSV());
            rend[0].SetPropertyBlock(_propBlock);
            rend[1].SetPropertyBlock(_propBlock);
            rend[2].SetPropertyBlock(_propBlock);
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
        }
        else if (joined3 == true && Input.GetButtonDown("P3Join"))
        {
            GameObject player = roundManager.alivePlayers[2];
            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            Renderer[] rend = player.GetComponentsInChildren<Renderer>();
            rend[0].GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Random.ColorHSV());
            rend[0].SetPropertyBlock(_propBlock);
            rend[1].SetPropertyBlock(_propBlock);
            rend[2].SetPropertyBlock(_propBlock);
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
            roundManager.playerSpawns[Random.Range(0, roundManager.playerSpawns.Count)].GetComponent<PlayerSpawn>().spawnPlayer();
        }
        else if (joined4 == true && Input.GetButtonDown("P4Join"))
        {
            GameObject player = roundManager.alivePlayers[3];
            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            Renderer[] rend = player.GetComponentsInChildren<Renderer>();
            rend[0].GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Random.ColorHSV());
            rend[0].SetPropertyBlock(_propBlock);
            rend[1].SetPropertyBlock(_propBlock);
            rend[2].SetPropertyBlock(_propBlock);
        }
        if (joined4 == true && Input.GetButtonDown("P4B"))
        {
            joined4 = false;
            StatHolder.HowManyPlayers--;
            Destroy(roundManager.alivePlayers[3]);
            roundManager.alivePlayers.Remove(roundManager.alivePlayers[3]);
        }


        if (StatHolder.HowManyPlayers >= 2 && Input.GetButtonDown("P1Start"))
        {
            roundManager.newGame();
        }
    }
}
