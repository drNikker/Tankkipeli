﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrelCollider : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag ("Untagged"))
        {
            Destroy(collider.gameObject);
        }
    }
}
