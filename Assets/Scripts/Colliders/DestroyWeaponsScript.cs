using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeaponsScript : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Weapon")
        {
            Destroy(other.gameObject);
        }
    }
}
