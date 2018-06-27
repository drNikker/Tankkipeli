using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    GameObject player;
    RoundManager roundManager;

    private void Start()
    {
        roundManager = GameObject.Find("GameManager").GetComponent<RoundManager>();
    }

    public void spawnPlayer()
    {
        player = roundManager.spawnPlayers[Random.Range(0, roundManager.spawnPlayers.Count)];
        Instantiate(player, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        roundManager.spawnPlayers.Remove(player);
    }
	

}
