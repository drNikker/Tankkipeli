﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class HandControls : MonoBehaviour {

    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;


    Rigidbody rb;
    public GameObject otherHand;
    HandControls otherHandScript;

    public float power = 10;
    float offset;

    Vector3 p1LeftHand;
    Vector3 p1RightHand;
    Vector3 front;
    Weapon script;

    public string player;
    public string LRHand;

    Transform t;
    ConfigurableJoint[] joints;

    public GameObject weapon;
    GameObject equippedWeapon;
    GameObject playerObj;

    public float cd;
    bool weaponInHand = false;
    [HideInInspector]public bool guidingHand = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerObj = FindPlayerObj();
        otherHandScript = otherHand.GetComponent<HandControls>();
    }

    void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);
        KeyPresses();
    }


    private void FixedUpdate()
    {
        //p1LeftHand = new Vector3(Input.GetAxis(player + "LeftHandX"), 0f, Input.GetAxis(player + "LeftHandZ"));
        //p1RightHand = new Vector3(Input.GetAxis(player + "RightHandX"), 0f, Input.GetAxis(player + "RightHandZ"));

        //Quaternion.Euler(playerObj.transform.rotation.eulerAngles) *

        p1LeftHand =  new Vector3(state.ThumbSticks.Left.X, 0, state.ThumbSticks.Left.Y);
        p1RightHand =  new Vector3(state.ThumbSticks.Right.X, 0, state.ThumbSticks.Right.Y);

        front = playerObj.transform.forward;

        if (LRHand == "R" && guidingHand == false)
        {
            if ((Mathf.Abs(p1RightHand.x) > 0.2 || Mathf.Abs(p1RightHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + Quaternion.Euler(playerObj.transform.rotation.eulerAngles) * p1RightHand * power * Time.deltaTime);
            }
            Debug.DrawRay(transform.position, Quaternion.Euler(playerObj.transform.rotation.eulerAngles)* p1RightHand);
        }
        else if (LRHand == "R" && guidingHand == true)
        {
            rb.MovePosition(transform.position + front * power*2 * Time.deltaTime);
        }
        else if (LRHand == "L" && guidingHand == false)
        {
            if ((Mathf.Abs(p1LeftHand.x) > 0.2 || Mathf.Abs(p1LeftHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + Quaternion.Euler(playerObj.transform.rotation.eulerAngles) * p1LeftHand * power * Time.deltaTime);
            }
            Debug.DrawRay(transform.position, Quaternion.Euler(playerObj.transform.rotation.eulerAngles)*p1LeftHand, Color.red);
        }
        else if (LRHand == "L" && guidingHand == true)
        {
            rb.MovePosition(transform.position + front * power*2 * Time.deltaTime);
        }


        //Debug.Log(Input.GetAxis("P1LeftHandX") + " " + Input.GetAxis("P1LeftHandZ") + " Right " + Input.GetAxis("P1RightHandX") + " " + Input.GetAxis("P1RightHandZ"));
    }

    GameObject FindPlayerObj()
    {
        Transform t = transform;
        while (t.parent != null)
        {
            if (t.parent.tag == "Player")
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        Debug.LogWarning("Player was not found! Make sure player has Player tag in it");
        return null;
    }



    public void WeaponInReach(GameObject wpn)
    {
        Weapon temp = wpn.GetComponent<Weapon>();
        if (weaponInHand == false)
        {
            if (/*otherHandScript.weaponInHand == true && otherHandScript.weapon == wpn*/ temp.canTake == false)
            {
                weapon = null;
            }
            else
                weapon = wpn;
        }
    }

    public void WeaponOutOfReach()
    {
        if (weaponInHand == false)
        {
            weapon = null;
        }
    }


    IEnumerator MoveHand()
    {
        guidingHand = true;
        yield return new WaitForSeconds(0.2f);
        guidingHand = false;
    }

    IEnumerator MoveBothHands()
    {
        guidingHand = true;
        otherHandScript.guidingHand = true;
        yield return new WaitForSeconds(2f);
        guidingHand = false;
        otherHandScript.guidingHand = false;
    }


    void EquipOneHand()
    {
        OneHandingRestricting();
        equippedWeapon.transform.position = playerObj.transform.position + front * 0.5f;
        equippedWeapon.transform.parent = this.transform;
        SetStance(script.stance);
        joints[0].connectedBody = GetComponent<Rigidbody>();
        joints[0].autoConfigureConnectedAnchor = false;
        joints[0].connectedAnchor = new Vector3(offset, 0.14f, 0);
        script.Equip();
        otherHandScript.WeaponOutOfReach();
    }

    void EquipTwoHands()
    {
        /*equippedWeapon.transform.position = playerObj.transform.position + front * 0.5f;
        equippedWeapon.transform.parent = this.transform;
        SetStance(script.stance);

        t.rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
        joints[0].connectedBody = GetComponent<Rigidbody>();
        joints[0].autoConfigureConnectedAnchor = false;
        joints[0].connectedAnchor = new Vector3(offset, 0.1f, 0);

        t.rotation = otherHand.transform.rotation * Quaternion.Euler(90, 0, 0);
        joints[1].connectedBody = otherHand.GetComponent<Rigidbody>();
        joints[1].autoConfigureConnectedAnchor = false;
        joints[1].connectedAnchor = new Vector3(-offset, 0.1f, 0);
        script.Equip();*/
        equippedWeapon.transform.position = playerObj.transform.position + front * 1f;
        equippedWeapon.transform.parent = this.transform;
        SetStance(script.stance);


        joints[0].connectedBody = GetComponent<Rigidbody>();
        joints[0].autoConfigureConnectedAnchor = false;
        joints[0].connectedAnchor = new Vector3(offset, 0.1f, 0);


        joints[1].connectedBody = otherHand.GetComponent<Rigidbody>();
        joints[1].autoConfigureConnectedAnchor = false;
        joints[1].connectedAnchor = new Vector3(-offset, 0.1f, 0);
        script.Equip();
    }

    void OneHandingRestricting()
    {
        Transform arm = transform.parent.parent;
        CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();
        SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
        SoftJointLimit armHighLimit = armJoint.highTwistLimit;
        armLowLimit.limit = -20;
        armHighLimit.limit = 0;
        armJoint.lowTwistLimit = armLowLimit;
        armJoint.highTwistLimit = armHighLimit;
    }

    void SetStance(Weapon.Stance stance)
    {
        switch(stance)
        {

            case Weapon.Stance.NoStance:
                {
                    Transform arm = transform.parent.parent;
                    CharacterJoint handJoint = GetComponent<CharacterJoint>();
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
                    SoftJointLimit armHighLimit = armJoint.highTwistLimit;
                    limit.limit = 0;
                    armLowLimit.limit = -70;
                    armHighLimit.limit = 50;
                    handJoint.swing2Limit = limit;
                    otherHandJoint.swing2Limit = limit;
                    armJoint.lowTwistLimit = armLowLimit;
                    armJoint.highTwistLimit = armHighLimit;
                    break;
                }
            case Weapon.Stance.OneHanded:
                {
                    CharacterJoint handJoint = GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    limit.limit = 0;
                    handJoint.swing2Limit = limit;
                    //transform.eulerAngles = new Vector3(0,0,0);
                    t.rotation = transform.rotation;
                    t.Rotate(90, 0, 0);
                    break;
                }

            case Weapon.Stance.TwoHanded:
                {
                    CharacterJoint handJoint = GetComponent<CharacterJoint>();
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                    otherLimit.limit = 90;
                    handJoint.swing2Limit = otherLimit;
                    otherHandJoint.swing2Limit = otherLimit;

                    t.rotation = Quaternion.Euler(0, 0, 0);
               //     transform.rotation = t.rotation * Quaternion.Euler(0,0,0) ;
                    otherHand.transform.rotation = t.rotation * Quaternion.Euler(-90, 0, 0);
                    /*
                    transform.Rotate(-90,0,0);
                    otherHand.transform.Rotate(0,0,0);
                    t.rotation = transform.rotation;
                    t.eulerAngles = new Vector3(0, 90, 90);
                    otherHand.transform.rotation = transform.rotation;*/

                    break;
                }
            case Weapon.Stance.TwoHandedTwinblade:
                {
                    /* CharacterJoint handJoint = GetComponent<CharacterJoint>();
                     CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                     SoftJointLimit limit = handJoint.swing2Limit;
                     SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                     limit.limit = 90;
                     handJoint.swing2Limit = limit;
                     otherHandJoint.swing2Limit = limit;
                     transform.eulerAngles = new Vector3(180, 0, 0);
                     otherHand.transform.eulerAngles = new Vector3(0, 0, 90);
                     t.rotation = transform.rotation;
                     t.eulerAngles = new Vector3(0, 90, 90);*/

                     CharacterJoint handJoint = GetComponent<CharacterJoint>();
                     CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                     SoftJointLimit limit = handJoint.swing2Limit;
                     SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                     limit.limit = 90;
                     handJoint.swing2Limit = limit;
                     otherHandJoint.swing2Limit = limit;


                     t.rotation = Quaternion.Euler(0,0,0);
                     //transform.rotation = t.rotation * Quaternion.Euler(0,0,0);
                     otherHand.transform.rotation = t.rotation * Quaternion.Euler(90,0,0);
                    break;
                }
            case Weapon.Stance.FistWeapon:
                {
                    t.Rotate(90,0,0);
                    t.Rotate(0,180,0);
                    break;
                }
            default:
                {
                    Transform arm = transform.parent.parent;
                    CharacterJoint handJoint = GetComponent<CharacterJoint>();
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
                    SoftJointLimit armHighLimit = armJoint.highTwistLimit;
                    limit.limit = 0;
                    armLowLimit.limit = -70;
                    armHighLimit.limit = 50;
                    handJoint.swing2Limit = limit;
                    otherHandJoint.swing2Limit = limit;
                    armJoint.lowTwistLimit = armLowLimit;
                    armJoint.highTwistLimit = armHighLimit;
                    break;
                }
        }
    }

    public void DropWeapon()
    {
        script.Dropped();
        SetStance(Weapon.Stance.NoStance);
        if (joints.Length == 2)
        { otherHandScript.weaponInHand = false; }
        weaponInHand = false;
        weapon = null;
        equippedWeapon = null;
    }

    public void ThrowWeapon()
    {
        //weapon removal from hand
        SetStance(Weapon.Stance.NoStance);
        if (joints.Length == 2)
        { otherHandScript.weaponInHand = false;;
        }
        weaponInHand = false;
        weapon = null;
        equippedWeapon = null;

        //target direction and speed (rotation?)
        script.Thrown(front);
        //when slow enough, drop state
    }

    void KeyPresses()
    {
        if (state.Buttons.RightStick == ButtonState.Pressed && prevState.Buttons.RightStick == ButtonState.Released && LRHand == "R" && weapon != null && cd < Time.time)
        {
            cd = Time.time + 1;
            otherHandScript.cd = Time.time + 1;
            equippedWeapon = weapon;
            script = weapon.GetComponentInChildren<Weapon>();
            t = weapon.GetComponent<Transform>();
            joints = weapon.GetComponents<ConfigurableJoint>();
            if (weaponInHand == false && joints.Length == 1)
            {
                script.canTake = false;
                weaponInHand = true;
                offset = 0.04f;
                StartCoroutine("MoveHand");
                Invoke("EquipOneHand", 0.2f);

            }
            else if (joints.Length == 2 && weaponInHand == false)
            {
                script.canTake = false;
                if (otherHandScript.weaponInHand == true)
                { otherHandScript.DropWeapon(); }
                weaponInHand = true;
                otherHandScript.weaponInHand = true;
                offset = 0.04f;
                StartCoroutine("MoveBothHands");
                Invoke("EquipTwoHands", 0.2f);
            }
            else if (weaponInHand == true)
            {
                ThrowWeapon();
            }

        }
        else if (state.Buttons.LeftStick == ButtonState.Pressed && prevState.Buttons.LeftStick == ButtonState.Released && LRHand == "L" && weapon != null && cd < Time.time)
        {
            cd = Time.time + 1;
            otherHandScript.cd = Time.time + 1;
            equippedWeapon = weapon;
            script = weapon.GetComponentInChildren<Weapon>();
            t = weapon.GetComponent<Transform>();
            joints = weapon.GetComponents<ConfigurableJoint>();
            if (weaponInHand == false && joints.Length == 1)
            {
                weaponInHand = true;
                offset = -0.04f;
                StartCoroutine("MoveHand");
                Invoke("EquipOneHand", 0.2f);
            }
            else if (weaponInHand == false && joints.Length == 2)
            {
                if (otherHandScript.weaponInHand == true)
                { otherHandScript.DropWeapon(); }
                weaponInHand = true;
                otherHandScript.weaponInHand = true;
                offset = -0.04f;
                StartCoroutine("MoveBothHands");
                Invoke("EquipTwoHands", 0.2f);
            }
            else if (weaponInHand == true)
            {
                ThrowWeapon();
            }
        }
    }
}
