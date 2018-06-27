﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

    Rigidbody rb;
    public float power = 10;
    Vector3 p1LeftHand;
    Vector3 p1RightHand;

    public string player;
    public string LRHand;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        p1LeftHand = new Vector3(Input.GetAxis(player + "LeftHandX"), 0.2f, Input.GetAxis(player + "LeftHandZ"));
        p1RightHand = new Vector3(Input.GetAxis(player + "RightHandX"), 0.2f, Input.GetAxis(player + "RightHandZ"));

        if (LRHand == "R")
        {
            if ((p1RightHand.x != 0 || p1RightHand.y != 0))
            {
                rb.MovePosition(transform.position + p1RightHand * power * Time.deltaTime);
            }
        }
        else if (LRHand == "L")
        {
            if ((p1LeftHand.x != 0 || p1LeftHand.y != 0))
            {
                rb.MovePosition(transform.position + p1LeftHand * power * Time.deltaTime);
            }
        }


        Debug.Log(Input.GetAxis("P1LeftHandX") + " " + Input.GetAxis("P1LeftHandZ") + " Right " + Input.GetAxis("P1RightHandX") + " " + Input.GetAxis("P1RightHandZ"));
    }
}