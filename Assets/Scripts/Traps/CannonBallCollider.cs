using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallCollider : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("CannonBall"))
        {
            Destroy(collider.gameObject);
        }
    }
}
