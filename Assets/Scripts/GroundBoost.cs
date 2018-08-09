using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoost : MonoBehaviour {
    public float BoostAmount;
    public bool XBoost;


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            if (XBoost)
            {
                other.transform.root.GetComponent<Rigidbody>().AddForce(new Vector3(-1,0,0) * BoostAmount);
            }
            else
            {
                other.transform.root.GetComponent<Rigidbody>().AddForce(transform.forward * BoostAmount);
            }
        }
    }
}
