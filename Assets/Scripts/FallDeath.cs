using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bodypart")
        {
            other.transform.root.gameObject.GetComponent<PlayerHealth>().KillPlayer();
            //Debug.Log("lol");
        }
    }
}
