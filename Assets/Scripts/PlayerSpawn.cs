﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    GameObject player;
    RoundManager roundManager;

    public void spawnPlayer()
    {
        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        player = roundManager.spawnPlayers[Random.Range(0, roundManager.spawnPlayers.Count)];
        Instantiate(player, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        roundManager.spawnPlayers.Remove(player);
        roundManager.playerSpawns.Remove(this.gameObject);
    }
	

}
