using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBumper : MonoBehaviour
{
    public float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.attachedRigidbody.AddForce(Vector3.forward * speed);
            Debug.Log("Trigger Entered");
        }
    }
}
