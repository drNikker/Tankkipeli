using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScrit : MonoBehaviour
{
    public GameObject boneToRot;
    
    public float rotationSpeed;

    public bool rotate;

    void Start()
    {
        rotate = true;
    }

    void FixedUpdate()
    {
        if (rotate == true)
        {
            boneToRot.transform.Rotate(0, rotationSpeed, 0 * Time.deltaTime);
        }
    }
}
