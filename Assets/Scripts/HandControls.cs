using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

    Rigidbody rb;
    public float power = 10;
    Vector3 p1LeftHand;
    Vector3 p1RightHand;
    public string player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        p1RightHand.x = Input.GetAxis(player + "RightHandX");
        p1RightHand.z = Input.GetAxis(player + "RightHandZ");
        p1RightHand.y = 0;

        if ((p1RightHand.x != 0 || p1RightHand.y != 0))
        {
            rb.MovePosition(transform.position + p1RightHand * power * Time.deltaTime);
        }
        Debug.Log(Input.GetAxis("P1LeftHandX") + " " + Input.GetAxis("P1LeftHandZ") + " Right " + Input.GetAxis("P1RightHandX") + " " + Input.GetAxis("P1RightHandZ"));
    }
}
