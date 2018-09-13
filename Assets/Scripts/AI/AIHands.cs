using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class AIHands : MonoBehaviour
{

    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;


    Rigidbody rb;
    //Players other hand and the script in it
    public GameObject otherHand;
    AIHands otherHandScript;

    public float power = 10;        //Power used to move the hand
    float offset;                   //Offset for weapon placement

    //Vectors for directions the hands should be pointing, and front of the player
    Vector3 p1LeftHand;
    Vector3 p1RightHand;
    Vector3 front;

    //"Weapon" script in the equipped weapon
    Weapon script;

    //Player number and which hand this is attached to
    public string player;
    public string LRHand;

    //weapons transform and joints used to equip
    Transform t;
    ConfigurableJoint[] joints;

    //the weapon players can equip, and the one that is equipped
    public GameObject weapon;
    public GameObject equippedWeapon;
    //player, found with "player"-tag
    GameObject playerObj;
    //cooldown for equip/throw to avoid spam
    public float cd;
    //does player have a weapon
    public bool weaponInHand = false;
    //should the hand be moved infront of the player
    [HideInInspector] public bool guidingHand = false;

    public float LXminimum = -1.0F;
    public float LXmaximum = 0F;
    public float RXminimum = 0F;
    public float RXmaximum = 1.0F;
    public float Zminimum = -1.0F;
    public float Zmaximum = 1.0F;

    // starting value for the Lerp
    static float time = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerObj = FindPlayerObj();
        otherHandScript = otherHand.GetComponent<AIHands>();
    }

    void Update()
    {
    }


    private void FixedUpdate()
    {

        //Basic hand movement

        //p1LeftHand = new Vector3(state.ThumbSticks.Left.X, 0, state.ThumbSticks.Left.Y);
        //p1RightHand = new Vector3(state.ThumbSticks.Right.X, 0, state.ThumbSticks.Right.Y);
        // animate the position of the game object...
        p1LeftHand = new Vector3(Mathf.Lerp(LXmaximum, LXminimum, time), 0, Mathf.Lerp(Zmaximum, Zminimum, time));
        p1RightHand = new Vector3(Mathf.Lerp(RXminimum, RXmaximum, time), 0, Mathf.Lerp(Zminimum, Zmaximum, time));
        // .. and increase the t interpolater
        time += 0.3f * Time.fixedDeltaTime;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (time > 0.8f)
        {
            float temp = LXmaximum;
            LXmaximum = LXminimum;
            LXminimum = temp;
            float temp3 = RXmaximum;
            RXmaximum = RXminimum;
            RXminimum = temp3;
            float temp2 = Zmaximum;
            Zmaximum = Zminimum;
            Zminimum = temp2;
            time = 0.0f;
        }
        front = playerObj.transform.forward;

        if (LRHand == "R" && guidingHand == false)      //IF Right Hand
        {
            if ((Mathf.Abs(p1RightHand.x) > 0.2 || Mathf.Abs(p1RightHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + Quaternion.Euler(playerObj.transform.rotation.eulerAngles) * p1RightHand * power * Time.fixedDeltaTime);
            }
            Debug.DrawRay(transform.position, Quaternion.Euler(playerObj.transform.rotation.eulerAngles) * p1RightHand);
        }

        else if (LRHand == "R" && guidingHand == true)      //When Equipping
        {
            rb.MovePosition(transform.position + front * power * 2 * Time.fixedDeltaTime);
        }

        else if (LRHand == "L" && guidingHand == false)     //IF Left Hand
        {
            if ((Mathf.Abs(p1LeftHand.x) > 0.2 || Mathf.Abs(p1LeftHand.z) > 0.2))
            {
                rb.MovePosition(transform.position + Quaternion.Euler(playerObj.transform.rotation.eulerAngles) * p1LeftHand * power * Time.fixedDeltaTime);
            }
            Debug.DrawRay(transform.position, Quaternion.Euler(playerObj.transform.rotation.eulerAngles) * p1LeftHand, Color.red);
        }

        else if (LRHand == "L" && guidingHand == true)      //When Equipping
        {
            rb.MovePosition(transform.position + front * power * 2 * Time.fixedDeltaTime);
        }


        //Debug.Log(Input.GetAxis("P1LeftHandX") + " " + Input.GetAxis("P1LeftHandZ") + " Right " + Input.GetAxis("P1RightHandX") + " " + Input.GetAxis("P1RightHandZ"));
    }

    //Finds the player so front vector can be defined
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

    //Called from PickupDetection when new weapon can be equipped...
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

    //...and when it is out of pickup range
    public void WeaponOutOfReach()
    {
        if (weaponInHand == false)
        {
            weapon = null;
        }
    }

    //These are used to move hand(s) in fornt of the player during pickup
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

    //Sets weapons parent to hand, attaches the joints in the weapon and tells weapon to equip
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

    //Limits and frees the arms joint limits based on the weapon stance so the swinging would be as smooth as possible
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
        elbowLowLimit.limit = -50;
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

    //Adjusts the grip with the weapon based on the stance given for the weapon
    void SetStance(Weapon.Stance stance)
    {
        switch (stance)
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
                    t.rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
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

                    t.rotation = Quaternion.Euler(0, 0, 0);
                    transform.rotation = t.rotation * Quaternion.Euler(-90, 180, 0) * Quaternion.Euler(0, 0, playerObj.transform.eulerAngles.y);
                    otherHand.transform.rotation = t.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, 0, -playerObj.transform.eulerAngles.y);
                    t.rotation = Quaternion.Euler(0, 90 + playerObj.transform.eulerAngles.y, 0);
                    break;
                }
            case Weapon.Stance.FistWeapon:
                {
                    FistRestricting();
                    t.rotation = transform.rotation * Quaternion.Euler(0, -90, -30);
                    Vector3 newScale;
                    if (LRHand == "L")
                    { newScale = new Vector3(1, 1, -1); }
                    else
                    { newScale = new Vector3(1, 1, 1); }
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

    //Drops the weapon on the ground when taking 2H while having 1H equipped
    public void DropWeapon()
    {
        script.Dropped();
        SetStance(Weapon.Stance.NoStance);
        if (joints.Length == 2)
        {
            otherHandScript.weaponInHand = false;
            otherHandScript.SetStance(Weapon.Stance.NoStance);
        }
        script.taken = false;
    }

    //Unequips the weapon and tells weapon-script to throw the weapon
    public void ThrowWeapon()
    {
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
        script.taken = false;

        script.Thrown(front);
    }

    //Controlstick pressing detection
    public void KeyPresses()
    {
        //Right hand
        if (LRHand == "R" && weapon != null && cd < Time.time)   //If weapon available
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
            t = weapon.GetComponent<Transform>();
            joints = weapon.GetComponents<ConfigurableJoint>();

            if (weaponInHand == false && joints.Length == 1)    //1H weapon
            {
                script.canTake = false;
                weaponInHand = true;
                offset = 0.04f;
                StartCoroutine("MoveHand");
                Invoke("EquipOneHand", 0.2f);

            }
            else if (joints.Length == 2 && weaponInHand == false)       //2H weapon
            {
                script.canTake = false;
                if (otherHandScript.weaponInHand == true)
                { otherHandScript.DropWeapon(); }
                weaponInHand = true;
                otherHandScript.weaponInHand = true;
                otherHandScript.equippedWeapon = weapon;
                otherHandScript.weapon = weapon;
                offset = 0.04f;
                StartCoroutine("MoveBothHands");
                Invoke("EquipTwoHands", 0.2f);
            }
            else if (weaponInHand == true)      //throw
            {
                ThrowWeapon();
            }

        }

        //Left hand
        else if (LRHand == "L" && weapon != null && cd < Time.time)    //if weapon available
        {
            print(weapon);
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
            t = weapon.GetComponent<Transform>();
            joints = weapon.GetComponents<ConfigurableJoint>();

            if (weaponInHand == false && joints.Length == 1)        //1H weapon
            {
                weaponInHand = true;
                offset = -0.04f;
                StartCoroutine("MoveHand");
                Invoke("EquipOneHand", 0.2f);
            }

            else if (weaponInHand == false && joints.Length == 2)       //2H weapon
            {
                if (otherHandScript.weaponInHand == true)
                { otherHandScript.DropWeapon(); }
                weaponInHand = true;
                otherHandScript.weaponInHand = true;
                otherHandScript.equippedWeapon = weapon;
                otherHandScript.weapon = weapon;
                offset = -0.04f;
                StartCoroutine("MoveBothHands");
                Invoke("EquipTwoHands", 0.2f);
            }
            else if (weaponInHand == true)      //throw weapon
            {
                ThrowWeapon();
            }
        }
        print(weapon);
    }
}
