using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRemover : MonoBehaviour {


    public void RemoveHat()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        //Rigidbody rb = GetComponent<Rigidbody>();
        //joint.connectedBody = null;
        //rb.WakeUp();
        Destroy(joint);
    }

}
