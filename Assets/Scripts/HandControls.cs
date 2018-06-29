using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

    Rigidbody rb;
    public GameObject otherHand;
    public float power = 10;
    Vector3 p1LeftHand;
    Vector3 p1RightHand;
    Weapon script;

    public string player;
    public string LRHand;

    public GameObject weapon;
    GameObject equippedWeapon;

    float cd;
    bool canPress = false;
    bool weaponInHand = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        KeyPresses();

    }


    private void FixedUpdate()
    {
        p1LeftHand = new Vector3(Input.GetAxis(player + "LeftHandX"), 0.4f, Input.GetAxis(player + "LeftHandZ"));
        p1RightHand = new Vector3(Input.GetAxis(player + "RightHandX"), 0.4f, Input.GetAxis(player + "RightHandZ"));

        if (LRHand == "R")
        {
            if ((Mathf.Abs(p1RightHand.x) > 0.2 || Mathf.Abs(p1RightHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + p1RightHand * power * Time.deltaTime);
            }
        }
        else if (LRHand == "L")
        {
            if ((Mathf.Abs(p1LeftHand.x) > 0.2 || Mathf.Abs(p1LeftHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + p1LeftHand * power * Time.deltaTime);
            }
        }


        //Debug.Log(Input.GetAxis("P1LeftHandX") + " " + Input.GetAxis("P1LeftHandZ") + " Right " + Input.GetAxis("P1RightHandX") + " " + Input.GetAxis("P1RightHandZ"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" && weaponInHand == false)
        {
            weapon = other.gameObject;
            if (weapon.GetComponentInChildren<Weapon>().equipped == false)
            {
                canPress = true;
            }
            else
            {
                canPress = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(weaponInHand == false)
        {
            canPress = false;
            weapon = null;
        }
    }

    void KeyPresses()
    {
        if (Input.GetButton(player + "RightGrab") && LRHand == "R" && canPress == true && cd < Time.time)
        {
            script = weapon.GetComponentInChildren<Weapon>();
            Transform t = weapon.GetComponent<Transform>();
            ConfigurableJoint[] joints = weapon.GetComponents<ConfigurableJoint>();
            if (weaponInHand == false && joints.Length == 1)
            {
                weapon.transform.parent = this.transform;
                t.rotation = transform.rotation;
                t.Rotate(90, 0, 0);
                joints[0].connectedBody = GetComponent<Rigidbody>();
                joints[0].autoConfigureConnectedAnchor = false;
                joints[0].connectedAnchor = new Vector3(0.1f, 0.2f, 0);
                script.Equip();
                weaponInHand = true;
                equippedWeapon = weapon;
                cd = Time.time + 1;
            }
            else if (joints.Length == 2 && weaponInHand == false)
            {
                weapon.transform.parent = this.transform;
                transform.eulerAngles = new Vector3(0,0,0);
                otherHand.transform.eulerAngles = new Vector3(0,0,90);
                t.rotation = transform.rotation;
                t.eulerAngles = new Vector3(0, 90, 90);
                joints[0].connectedBody = GetComponent<Rigidbody>();
                joints[0].autoConfigureConnectedAnchor = false;
                joints[0].connectedAnchor = new Vector3(0.1f, 0.2f, 0);
                joints[1].connectedBody = otherHand.GetComponent<Rigidbody>();
                joints[1].autoConfigureConnectedAnchor = false;
                joints[1].connectedAnchor = new Vector3(-0.1f, 0.2f, 0);
                script.Equip();
                weaponInHand = true;
                equippedWeapon = weapon;
                cd = Time.time + 1;
            }
            else if (weaponInHand == true)
            {
                script.Dropped();
                weaponInHand = false;
                weapon = null;
                equippedWeapon = null;
                cd = Time.time + 1;
            }

        }
        else if (Input.GetButton(player + "LeftGrab") && LRHand == "L" && canPress == true && cd < Time.time)
        {
            script = weapon.GetComponentInChildren<Weapon>();
            Transform t = weapon.GetComponent<Transform>();
            ConfigurableJoint[] joints = weapon.GetComponents<ConfigurableJoint>();
            if (weaponInHand == false && joints.Length == 1)
            {
                weapon.transform.parent = this.transform;
                t.rotation = transform.rotation;
                t.Rotate(90, 0, 0);
                joints[0].connectedBody = GetComponent<Rigidbody>();
                joints[0].autoConfigureConnectedAnchor = false;
                joints[0].connectedAnchor = new Vector3(-0.1f, 0.2f, 0);
                script.Equip();
                weaponInHand = true;
                equippedWeapon = weapon;
                cd = Time.time + 1;
            }
            else if (weaponInHand == false && joints.Length == 2)
            {
                weapon.transform.parent = this.transform;
                transform.eulerAngles = new Vector3(0, 0, 0);
                otherHand.transform.eulerAngles = new Vector3(0, 0, 90);
                t.rotation = transform.rotation;
                t.eulerAngles = new Vector3(0, 90, 90);
                joints[0].connectedBody = GetComponent<Rigidbody>();
                joints[0].autoConfigureConnectedAnchor = false;
                joints[0].connectedAnchor = new Vector3(-0.1f, 0.2f, 0);
                joints[1].connectedBody = otherHand.GetComponent<Rigidbody>();
                joints[1].autoConfigureConnectedAnchor = false;
                joints[1].connectedAnchor = new Vector3(0.1f, 0.2f, 0);
                script.Equip();
                weaponInHand = true;
                equippedWeapon = weapon;
                cd = Time.time + 1;
            }
            else if (weaponInHand == true)
            {
                script.Dropped();
                weaponInHand = false;
                weapon = null;
                equippedWeapon = null;
                cd = Time.time + 1;
            }
        }
    }
}
