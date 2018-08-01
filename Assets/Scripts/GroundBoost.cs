using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoost : MonoBehaviour {
    public float BoostAmount;


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            other.transform.root.GetComponent<Rigidbody>().AddForce(transform.forward * BoostAmount);
        }
    }
}
