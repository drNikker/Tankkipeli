using UnityEngine;
using System.Collections;

public class CharacterUpright : MonoBehaviour
{

    new protected Rigidbody rigidbody;
    public bool keepUpright = false;
    public float uprightForce = 1000;
    public float addUprightForce = 100;
    public float uprightOffset = 1.45f;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 40; 
    }

    void FixedUpdate()
    {
        if (keepUpright) // when true, adds upward and downward force to player model to set it upright.
        {
            //upward
            rigidbody.AddForceAtPosition(new Vector3(0, (uprightForce + addUprightForce), 0), transform.position + transform.TransformPoint(new Vector3(0, uprightOffset, 0)), ForceMode.Force);

            //downward
            rigidbody.AddForceAtPosition(new Vector3(0, -uprightForce, 0), transform.position + transform.TransformPoint(new Vector3(0, -uprightOffset, 0)), ForceMode.Force);
        }
    }
}
