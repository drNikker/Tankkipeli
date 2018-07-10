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
    GameObject equippedWeapon;
    GameObject playerObj;

    public float cd;
    bool weaponInHand = false;
    public bool guidingHand = false;

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

        p1LeftHand = new Vector3(state.ThumbSticks.Left.X, 0f, state.ThumbSticks.Left.Y);
        p1RightHand = new Vector3(state.ThumbSticks.Right.X, 0f, state.ThumbSticks.Right.Y);

        front = playerObj.transform.forward;

        if (LRHand == "R" && guidingHand == false)
        {
            if ((Mathf.Abs(p1RightHand.x) > 0.2 || Mathf.Abs(p1RightHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + p1RightHand * power * Time.deltaTime);
            }
        }
        else if (LRHand == "R" && guidingHand == true)
        {
            rb.MovePosition(transform.position + front * power * Time.deltaTime);
        }
        else if (LRHand == "L" && guidingHand == false)
        {
            if ((Mathf.Abs(p1LeftHand.x) > 0.2 || Mathf.Abs(p1LeftHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + p1LeftHand * power * Time.deltaTime);
            }
        }
        else if (LRHand == "L" && guidingHand == true)
        {
            rb.MovePosition(transform.position + front * 20 * Time.deltaTime);
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
        if (weaponInHand == false)
        {
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
        yield return new WaitForSeconds(0.2f);
        guidingHand = false;
        otherHandScript.guidingHand = false;
    }

    void EquipOneHand()
    {
        weapon.transform.position = playerObj.transform.position + front * 2;
        equippedWeapon.transform.parent = this.transform;
        SetStance(script.stance);
        joints[0].connectedBody = GetComponent<Rigidbody>();
        joints[0].autoConfigureConnectedAnchor = false;
        joints[0].connectedAnchor = new Vector3(offset, 0.2f, 0);
        script.Equip();
        otherHandScript.WeaponOutOfReach();
    }

    void EquipTwoHands()
    {
        weapon.transform.position = playerObj.transform.position + front * 2;
        equippedWeapon.transform.parent = this.transform;
        SetStance(script.stance);
        joints[0].connectedBody = GetComponent<Rigidbody>();
        joints[0].autoConfigureConnectedAnchor = false;
        joints[0].connectedAnchor = new Vector3(offset, 0.2f, 0);
        joints[1].connectedBody = otherHand.GetComponent<Rigidbody>();
        joints[1].autoConfigureConnectedAnchor = false;
        joints[1].connectedAnchor = new Vector3(-offset, 0.2f, 0);
        script.Equip();
    }

    void SetStance(Weapon.Stance stance)
    {
        switch(stance)
        {

            case Weapon.Stance.NoStance:
                {
                    CharacterJoint handJoint = GetComponent<CharacterJoint>();
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                    limit.limit = 0;
                    handJoint.swing2Limit = limit;
                    otherHandJoint.swing2Limit = limit;
                    break;
                }
            case Weapon.Stance.OneHanded:
                {
                    transform.eulerAngles = new Vector3(0,0,0);
                    t.rotation = transform.rotation;
                    t.Rotate(90, 0, 0);
                    break;
                }

            case Weapon.Stance.TwoHanded:
                {
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                    otherLimit.limit = 90;
                    otherHandJoint.swing2Limit = otherLimit;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    otherHand.transform.eulerAngles = new Vector3(0, 0, 90);
                    t.rotation = transform.rotation;
                    t.eulerAngles = new Vector3(0, 90, 90);
                    break;
                }
            case Weapon.Stance.TwoHandedTwinblade:
                {
                    CharacterJoint handJoint = GetComponent<CharacterJoint>();
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                    limit.limit = 90;
                    handJoint.swing2Limit = limit;
                    otherHandJoint.swing2Limit = limit;
                    transform.eulerAngles = new Vector3(180, 0, 0);
                    otherHand.transform.eulerAngles = new Vector3(0, 0, 90);
                    t.rotation = transform.rotation;
                    t.eulerAngles = new Vector3(0, 90, 90);
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
                    CharacterJoint handJoint = otherHand.GetComponent<CharacterJoint>();
                    CharacterJoint otherHandJoint = otherHand.GetComponent<CharacterJoint>();
                    SoftJointLimit limit = handJoint.swing2Limit;
                    SoftJointLimit otherLimit = otherHandJoint.swing2Limit;
                    limit.limit = 0;
                    handJoint.swing2Limit = limit;
                    otherHandJoint.swing2Limit = limit;
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
        print("throwing");
        //weapon removal from hand
        SetStance(Weapon.Stance.NoStance);
        if (joints.Length == 2)
        { otherHandScript.weaponInHand = false; }
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
                weaponInHand = true;
                offset = 0.1f;
                StartCoroutine("MoveHand");
                Invoke("EquipOneHand", 0.2f);

            }
            else if (joints.Length == 2 && weaponInHand == false)
            {

                if (otherHandScript.weaponInHand == true)
                { otherHandScript.DropWeapon(); }
                weaponInHand = true;
                otherHandScript.weaponInHand = true;
                offset = 0.1f;
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
                offset = -0.1f;
                StartCoroutine("MoveHand");
                Invoke("EquipOneHand", 0.2f);
            }
            else if (weaponInHand == false && joints.Length == 2)
            {
                if (otherHandScript.weaponInHand == true)
                { otherHandScript.DropWeapon(); }
                weaponInHand = true;
                otherHandScript.weaponInHand = true;
                offset = -0.1f;
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
