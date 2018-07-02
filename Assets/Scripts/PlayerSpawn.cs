using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    GameObject player;
    RoundManager roundManager;

    public void spawnPlayer()
    {
        roundManager = GameObject.Find("GameManager").GetComponent<RoundManager>();
        player = roundManager.spawnPlayers[Random.Range(0, roundManager.spawnPlayers.Count)];
        Instantiate(player, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        if (StatHolder.Player1Color != null)
        {
            Color color = new Vector4(StatHolder.Player1Color[0], StatHolder.Player1Color[1], StatHolder.Player1Color[2], StatHolder.Player1Color[3]);

            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            Renderer[] rend = player.GetComponentsInChildren<Renderer>();
            rend[0].GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", color);
            rend[0].SetPropertyBlock(_propBlock);
            rend[1].SetPropertyBlock(_propBlock);
            rend[2].SetPropertyBlock(_propBlock);
        }
        roundManager.spawnPlayers.Remove(player);
        roundManager.playerSpawns.Remove(this.gameObject);
    }
	

}
