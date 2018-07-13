using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeHole : MonoBehaviour {

    public StatHolder.Modes mode;

    public int playersInHole;

    List<Collider> players;

    private void Start()
    {
        players = new List<Collider>();
    }

    void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i] == null)
            {
                players.Remove(players[i]);
                playersInHole -= 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !players.Contains(other))
        {
            players.Add(other);
            playersInHole += 1;
        }
    }



}
