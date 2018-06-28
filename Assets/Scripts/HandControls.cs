using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControls : MonoBehaviour {

    Rigidbody rb;
    public float power = 10;
    Vector3 p1LeftHand;
    Vector3 p1RightHand;

    public string player;
    public string LRHand;

    public ConfigurableJoint joint;
    public GameObject weapon;
    bool canEquip = false;

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
        if (other.tag == "Weapon")
        {
            canEquip = true;
            weapon = other.gameObject;
            print(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        weapon = null;
        canEquip = false;
        joint = null;
    }

    void KeyPresses()
    {
        if (Input.GetKey(KeyCode.Joystick1Button9) && canEquip == true)
        {
            print("r");
            joint = weapon.GetComponent<ConfigurableJoint>();
            joint.connectedBody = GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = new Vector3(0.1f, 0.15f, 0);
            Transform t = weapon.GetComponent<Transform>();
            weapon.transform.parent = this.transform;
            t.localEulerAngles = new Vector3(0, 0, 0);
            Weapon script = weapon.GetComponentInChildren<Weapon>();
            script.Equip();
            canEquip = false;

        }
    }
}
