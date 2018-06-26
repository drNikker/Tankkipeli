using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBumper : MonoBehaviour
{
    public float force;

    public Rigidbody rb;
    public bool move;

    // Use this for initialization
    void Start()
    {
        //rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            
        }
    }
    /*
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Bumper hit");
            collider.attachedRigidbody.AddForce(0, 0, 10 * force);
            
        }
    }
    */

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Vector3 dir = collider.contacts[0].point - transform.position;
            dir = -dir.normalized;
            
            collider.gameObject.transform.GetComponent<Rigidbody>().AddForce(dir * force);

            Debug.Log("Bumper hit");
        }
    }
}
