using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeHole : MonoBehaviour {

    int playerCount;

    List<Collider> players;

    private void Start()
    {
        players = new List<Collider>();
    }

    private void Update()
    {
        print(playerCount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        playerCount = +1;
        print(playerCount);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        playerCount -= 1;
        print(playerCount);
    }


}
