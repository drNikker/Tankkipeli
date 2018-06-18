using UnityEngine;
using System.Collections;

public class CharacterUpright : MonoBehaviour
{

    new protected Rigidbody rigidbody;
    public bool keepUpright = true;
    public float uprightForce = 10;
    public float uprightOffset = 1.45f;
    public float additionalUpwardForce = 10;
    public float dampenAngularForce = 0;
    //
    //
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 40; // **** CANNOT APPLY HIGH ANGULAR FORCES UNLESS THE MAXANGULAR VELOCITY IS INCREASED ****
    }
    //
    void FixedUpdate()
    {
        if (keepUpright)
        {
            // ***** USE TWO FORCES PULLING UP AND DOWN AT THE TOP AND BOTTOM OF THE OBJECT RESPECTIVELY TO PULL IT UPRIGHT ***
            //
            //  *** THIS TECHNIQUE CAN BE USED FOR PULLING AN OBJECT TO FACE ANY VECTOR ***
            //
            rigidbody.AddForceAtPosition(new Vector3(0, (uprightForce + additionalUpwardForce), 0),
                transform.position + transform.TransformPoint(new Vector3(0, uprightOffset, 0)), ForceMode.Force);
            //
            rigidbody.AddForceAtPosition(new Vector3(0, -uprightForce, 0),
                transform.position + transform.TransformPoint(new Vector3(0, -uprightOffset, 0)), ForceMode.Force);
            //  

        }
        if (dampenAngularForce > 0)
        {
            rigidbody.angularVelocity *= (1 - Time.deltaTime * dampenAngularForce);
        }
    }
}
