using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityFistCollider : MonoBehaviour
{
    private ProximityFist proximityFist;
    [HideInInspector]
    public bool allowPunching;

    void Start()
    {
        proximityFist = transform.GetComponentInParent<ProximityFist>();

        allowPunching = true;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Bodypart"))
        {
            //Debug.Log("Koskettaa");

            if (proximityFist.holdOffTimer == false && proximityFist.waitTimer == false)
            {
                proximityFist.punchTimer = true;
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Bodypart"))
        {
            //Debug.Log("Koskettaa koko ajan");
            if (proximityFist.holdOffTimer == false && proximityFist.waitTimer == false)
            {
                proximityFist.punchTimer = true;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Bodypart"))
        {
            //Debug.Log("heipapa");

            allowPunching = false;
        }
    }
}
