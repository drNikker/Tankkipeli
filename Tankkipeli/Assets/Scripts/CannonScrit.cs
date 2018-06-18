using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScrit : MonoBehaviour
{
    public GameObject boneToRot;
    
    public float rotationSpeed;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        boneToRot.transform.Rotate(0, rotationSpeed, 0 * Time.deltaTime);
    }
}
