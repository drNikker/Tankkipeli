using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Bodypart")
        {
            other.GetComponent<PlayerHealth>().KillPlayer();
        }
    }

}
