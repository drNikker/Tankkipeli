using System.Collections;
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
    public GameObject equippedWeapon;
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
            if (temp.canTake == false)
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
        yield return new WaitForSeconds(1f);
        guidingHand = false;
        otherHandScript.guidingHand = false;
    }


    void EquipOneHand()
    {
        equippedWeapon.transform.position = playerObj.transform.position + new Vector3(0, 1, 0) + front * 1f;
        SetStance(script.stance);
        equippedWeapon.transform.parent = this.transform;
        joints[0].connectedBody = GetComponent<Rigidbody>();
        joints[0].autoConfigureConnectedAnchor = false;
        joints[0].connectedAnchor = new Vector3(offset, 0.14f, 0);
        script.Equip();
        otherHandScript.WeaponOutOfReach();
    }

    void EquipTwoHands()
    {

        equippedWeapon.transform.position = playerObj.transform.position + new Vector3(0, 1, 0) + front * 1f;
        SetStance(script.stance);
        equippedWeapon.transform.parent = this.transform;

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
        Transform elbow = transform.parent;

        CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();
        CharacterJoint elbowJoint = elbow.GetComponent<CharacterJoint>();
        CharacterJoint handJoint = GetComponent<CharacterJoint>();

        SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
        SoftJointLimit armHighLimit = armJoint.highTwistLimit;
        SoftJointLimit elbowLowLimit = elbowJoint.lowTwistLimit;
        SoftJointLimit elbowHighLimit = elbowJoint.highTwistLimit;
        SoftJointLimit handSwing2Limit = handJoint.swing2Limit;

        armLowLimit.limit = 0;
        armHighLimit.limit = 0;
        elbowLowLimit.limit = -20;
        elbowHighLimit.limit = 0;
        handSwing2Limit.limit = 0;

        armJoint.lowTwistLimit = armLowLimit;
        armJoint.highTwistLimit = armHighLimit;
        elbowJoint.lowTwistLimit = elbowLowLimit;
        elbowJoint.highTwistLimit = elbowHighLimit;
        handJoint.swing2Limit = handSwing2Limit;

    }

    void TwoHandingRestricting()
    {
        Transform arm = transform.parent.parent;

        CharacterJoint handJoint = GetComponent<CharacterJoint>();
        CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();

        SoftJointLimit handSwing2limit = handJoint.swing2Limit;
        SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
        SoftJointLimit armHighLimit = armJoint.highTwistLimit;
        SoftJointLimit armSwing1Limit = armJoint.swing1Limit;
        SoftJointLimit handLowLimit = handJoint.lowTwistLimit;
        SoftJointLimit handHighLimit = handJoint.highTwistLimit;

        handSwing2limit.limit = 0;
        armLowLimit.limit = 0;
        armHighLimit.limit = 0;
        armSwing1Limit.limit = 120;
        handLowLimit.limit = 0;
        handHighLimit.limit = 0;

        handJoint.swing2Limit = handSwing2limit;
        armJoint.lowTwistLimit = armLowLimit;
        armJoint.highTwistLimit = armHighLimit;
        armJoint.swing1Limit = armSwing1Limit;
        handJoint.lowTwistLimit = handLowLimit;
        handJoint.highTwistLimit = handHighLimit;

    }


    void FistRestricting()
    {
        Transform arm = transform.parent.parent;
        Transform elbow = transform.parent;

        CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();
        CharacterJoint elbowJoint = elbow.GetComponent<CharacterJoint>();
        CharacterJoint handJoint = GetComponent<CharacterJoint>();

        SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
        SoftJointLimit armHighLimit = armJoint.highTwistLimit;
        SoftJointLimit elbowLowLimit = elbowJoint.lowTwistLimit;
        SoftJointLimit elbowHighLimit = elbowJoint.highTwistLimit;
        SoftJointLimit handSwing2Limit = handJoint.swing2Limit;

        armLowLimit.limit = 0;
        armHighLimit.limit = 0;
        elbowLowLimit.limit = -90;
        elbowHighLimit.limit = 0;
        handSwing2Limit.limit = 0;

        armJoint.lowTwistLimit = armLowLimit;
        armJoint.highTwistLimit = armHighLimit;
        elbowJoint.lowTwistLimit = elbowLowLimit;
        elbowJoint.highTwistLimit = elbowHighLimit;
        handJoint.swing2Limit = handSwing2Limit;
    }

    void NoHandRestrictions()
    {
        Transform arm = transform.parent.parent;
        Transform elbow = transform.parent;
        CharacterJoint handJoint = GetComponent<CharacterJoint>();
        CharacterJoint armJoint = arm.GetComponent<CharacterJoint>();
        CharacterJoint elbowJoint = elbow.GetComponent<CharacterJoint>();

        SoftJointLimit handSwing2limit = handJoint.swing2Limit;
        SoftJointLimit handLowLimit = handJoint.lowTwistLimit;
        SoftJointLimit handHighLimit = handJoint.highTwistLimit;

        SoftJointLimit armLowLimit = armJoint.lowTwistLimit;
        SoftJointLimit armHighLimit = armJoint.highTwistLimit;
        SoftJointLimit armSwing1Limit = armJoint.swing1Limit;

        SoftJointLimit elbowLowLimit = elbowJoint.lowTwistLimit;
        SoftJointLimit elbowHighLimit = elbowJoint.highTwistLimit;

        handLowLimit.limit = 0;
        handHighLimit.limit = 0;
        handSwing2limit.limit = 0;
        armLowLimit.limit = -70;
        armHighLimit.limit = 50;
        armSwing1Limit.limit = 40;
        elbowLowLimit.limit = -90;
        elbowHighLimit.limit = 0;

        handJoint.swing2Limit = handSwing2limit;
        armJoint.lowTwistLimit = armLowLimit;
        armJoint.highTwistLimit = armHighLimit;
        armJoint.swing1Limit = armSwing1Limit;
        elbowJoint.lowTwistLimit = elbowLowLimit;
        elbowJoint.highTwistLimit = elbowHighLimit;
        handJoint.lowTwistLimit = handLowLimit;
        handJoint.highTwistLimit = handHighLimit;
    }

    void SetStance(Weapon.Stance stance)
    {
        switch(stance)
        {

            case Weapon.Stance.NoStance:
                {
                    transform.eulerAngles = new Vector3(-30, transform.eulerAngles.y, transform.eulerAngles.z);
                    NoHandRestrictions();
                    break;
                }
            case Weapon.Stance.OneHanded:
                {
                    OneHandingRestricting();
                    t.rotation = transform.rotation * Quaternion.Euler(90,0,0);
                    break;
                }

            case Weapon.Stance.TwoHanded:
                {
                    TwoHandingRestricting();
                    otherHandScript.TwoHandingRestricting();

                    t.rotation = Quaternion.Euler(0, -90, 0);
                    transform.rotation = t.rotation * Quaternion.Euler(-90, 0, 0) * Quaternion.Euler(0, 0, playerObj.transform.eulerAngles.y);
                    otherHand.transform.rotation = t.rotation * Quaternion.Euler(-90, 0, 0) * Quaternion.Euler(0, 0, -playerObj.transform.eulerAngles.y);

                    break;
                }
            case Weapon.Stance.TwoHandedTwinblade:
                {
                    TwoHandingRestricting();
                    otherHandScript.TwoHandingRestricting();

                    t.rotation = Quaternion.Euler(0,0,0);
                    transform.rotation = t.rotation * Quaternion.Euler(-90,180,0) * Quaternion.Euler(0,0, playerObj.transform.eulerAngles.y);
                    otherHand.transform.rotation = t.rotation * Quaternion.Euler(90,0,0) * Quaternion.Euler(0, 0, -playerObj.transform.eulerAngles.y);
                    t.rotation = Quaternion.Euler(0,90 + playerObj.transform.eulerAngles.y,0);
                    break;
                }
            case Weapon.Stance.FistWeapon:
                {
                    FistRestricting();
                    t.rotation = transform.rotation * Quaternion.Euler(0,-90,-30);
                    Vector3 newScale;
                    if(LRHand == "L")
                    { newScale = new Vector3(1,1,-1); }
                    else
                    { newScale = new Vector3(1,1,1); }
                    t.transform.localScale = newScale;
                    break;
                }
            default:
                {
                    transform.eulerAngles = new Vector3(-30, transform.eulerAngles.y, transform.eulerAngles.z);
                    NoHandRestrictions();
                    break;
                }
        }
    }

    public void DropWeapon()
    {
        script.Dropped();
        SetStance(Weapon.Stance.NoStance);
        if (joints.Length == 2)
        { otherHandScript.weaponInHand = false;
            otherHandScript.SetStance(Weapon.Stance.NoStance);
        }
        weaponInHand = false;
        weapon = null;
        equippedWeapon = null;
    }

    public void ThrowWeapon()
    {
        //weapon removal from hand
        SetStance(Weapon.Stance.NoStance);
        if (joints.Length == 2)
        {
            otherHandScript.weaponInHand = false;
            otherHandScript.equippedWeapon = null;
            otherHandScript.weapon = null;
            otherHandScript.SetStance(Weapon.Stance.NoStance);
        }
        weaponInHand = false;
        weapon = null;
        equippedWeapon = null;

        //target direction and speed (rotation?)
        script.Thrown(front);
    }

    void KeyPresses()
    {
        if (state.Buttons.RightStick == ButtonState.Pressed && prevState.Buttons.RightStick == ButtonState.Released && LRHand == "R" && weapon != null && cd < Time.time)
        {
            cd = Time.time + 1;
            otherHandScript.cd = Time.time + 1;
            script = weapon.GetComponentInChildren<Weapon>();
            if (equippedWeapon == null && script.taken == true)
            {
                return;
            }
            else if (equippedWeapon == null && script.taken == false)
            {
                script.taken = true;
            }
            equippedWeapon = weapon;
            otherHandScript.equippedWeapon = weapon;
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
            script = weapon.GetComponentInChildren<Weapon>();
            if (equippedWeapon == null && script.taken == true)
            {
                return;
            }
            else if (equippedWeapon == null && script.taken == false)
            {
                script.taken = true;
            }
            equippedWeapon = weapon;
            otherHandScript.equippedWeapon = weapon;
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
