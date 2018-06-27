using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBumper : MonoBehaviour
{
    public float force;

    private Rigidbody rb;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            rb = collider.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb);

            Vector3 dir = collider.contacts[0].point - transform.position;
            Debug.Log(collider.contacts[0].point);
            dir = dir.normalized;

            rb.AddForce(dir * force);
            Debug.Log(dir);
            Debug.Log(force);

            Debug.Log("Bumper hit");
        }
    }
}
