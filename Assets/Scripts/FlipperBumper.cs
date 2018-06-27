using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBumper : MonoBehaviour
{
    public float force;
    public Animator anim;
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
            anim.SetTrigger("Bump");
            Vector3 dir = collider.contacts[0].point - transform.position;
            dir = dir.normalized;

            rb.AddForce(dir * force);
        }
    }
}
