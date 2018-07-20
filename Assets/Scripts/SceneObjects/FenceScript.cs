using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceScript : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Rigidbody playerRigidBody;

    public float playerImpactMagnitude;
    public float weaponImpactMagnitude;
    [Space(10)]
    public float playerPushingForce;
    public float fencePushingForce;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
    }

    //Checks if player or weapon is colliding and if it returns true it pushes the collided player away
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.relativeVelocity.magnitude >= playerImpactMagnitude)
        {
            playerRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            rigidBody.isKinematic = false;

            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection = pushDirection.normalized;

            playerRigidBody.AddForce(pushDirection * playerPushingForce);
        }

        if (collision.gameObject.tag == "Weapon" && collision.relativeVelocity.magnitude >= weaponImpactMagnitude)
        {
            Debug.Log("jou");
            rigidBody.isKinematic = false;
            
            Vector3 fencePushDirection = collision.contacts[0].point - transform.position;
            fencePushDirection.Normalize();

            rigidBody.AddForce(fencePushDirection * fencePushingForce);
        }
    }
}
