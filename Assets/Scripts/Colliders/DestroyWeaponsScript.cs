using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeaponsScript : MonoBehaviour
{
    public float destroyTime;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != null && other.gameObject.tag == "Weapon")
        {
            Destroy(other.gameObject, destroyTime);
        }
    }
}
