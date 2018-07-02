using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrelCollider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag ("Untagged"))
        {
            Destroy(collider.gameObject);
        }
    }
}
