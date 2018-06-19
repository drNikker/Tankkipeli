using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRemover : MonoBehaviour {


    public void RemoveHat()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        Rigidbody rb = GetComponent<Rigidbody>();
        Destroy(joint);
        transform.parent = null;
        rb.AddForce(transform.up * 3);
    }

}
