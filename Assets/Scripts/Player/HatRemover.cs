﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRemover : MonoBehaviour {

    public float force = 10;
    public ParticleSystem VFX;
    CapsuleCollider col;

    public void RemoveHat()
    {

        FixedJoint joint = GetComponent<FixedJoint>();
        Rigidbody rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        Destroy(joint);
        transform.parent = null;
        VFX.Play(true);
        rb.AddForce(transform.up * force);
        StartCoroutine("ColliderOn");
    }

    IEnumerator ColliderOn()
    {
        yield return new WaitForSeconds(0.2f);
        col.isTrigger = false;
    }
}
