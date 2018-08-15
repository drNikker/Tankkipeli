using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    WeaponDamage[] damageDealers;       //Damaging parts of the weapon
    ConfigurableJoint[] joints;         //Joints used to equip the weapon

    float baseCooldown = 4;     //The time it takes for weapon to be pickable after throw
    float cooldown = 2;         //Variable used to store the current cooldown
    float totalMass;            //Total mass of all rigidbodies in the weapon
    public float weaponThrowForce = 8000;       //The force applied during throw
    public float maxSpeed = 10;                 //Desired maximum throw speed

    bool timer = false;     //Is timer being used
    bool canEquip;          //CAN this weapon be equipped
    [HideInInspector] public bool equipped; //IS this weapon equipped
    [HideInInspector] public bool canTake = false;  
    [HideInInspector] public bool taken = false;    //Is this weapon taken. Used to double check in HandControls, not same as canEquip

    public WEAPON_STATE currentWeaponState;
    public Stance stance;

    Animator dropAnimation; //Dropped animation
    Light dropLight;        //Dropped lighting

	// Use this for initialization
	void Start ()
    {
        dropAnimation = GetComponent<Animator>();
        dropLight = GetComponent<Light>();
        damageDealers = GetComponentsInChildren<WeaponDamage>();

        currentWeaponState = WEAPON_STATE.DROPPED;
        SetWeaponState();
        joints = GetComponents<ConfigurableJoint>();
    }

    private void Update()
    {
        //If weapon is thrown, return to dropped after the cooldown time
        if (currentWeaponState == WEAPON_STATE.THROWN && cooldown <= Time.time)
        {
            Dropped();
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        //If thrown weapon has too much speed, it will be clamped to max speed
        else if (currentWeaponState == WEAPON_STATE.THROWN)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);               
            }

        }
        //Smoothens equipping of the weapons
        if (timer == true)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            for (int i = 0; i < joints.Length; i++)
            {
                SoftJointLimit limit = joints[i].linearLimit;
                limit.limit -= 0.5f * Time.deltaTime;
                joints[i].linearLimit = limit;
                if (limit.limit <= 0.01)
                {
                    timer = false;
                    limit.limit = 0;
                    joints[i].linearLimit = limit;
                }
            }
        }
        RayCastToGround();
    }

    //Equip
    public void Equip()
    {
        currentWeaponState = WEAPON_STATE.WIELDED;
        SetWeaponState();
    }

    //Drop
    public void Dropped()
    {
        currentWeaponState = WEAPON_STATE.DROPPED;
        SetWeaponState();
    }

    //Throw in given direction
    public void Thrown(Vector3 front)
    {
        GetMass();
        cooldown = Time.time + baseCooldown;
        currentWeaponState = WEAPON_STATE.THROWN;
        SetWeaponState();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 1;
        rb.AddForce((front + new Vector3(0,0.1f,0)) * weaponThrowForce * totalMass);
    }

    //Calculate the total mass of the rigidbodies
    void GetMass()
    {
        totalMass = 0.1f;
        foreach (WeaponDamage wd in damageDealers)
        {
            Rigidbody r = wd.GetComponent<Rigidbody>();
            totalMass += r.mass;
        }
    }

    //Different weapon stances used for weapon grip
    public enum Stance
    {
        NoStance,
        OneHanded,
        TwoHanded,
        TwoHandedTwinblade,
        FistWeapon,
    }

    //Different states the weapon can be in
    public enum WEAPON_STATE
    {
        DROPPED,
        WIELDED,
        THROWN,
    }

    //Enables weapons colliders after a small delay
    IEnumerator CollidersOn()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        yield return new WaitForSeconds(1f);
        for (int i = 1; i <= colliders.Length - 1; i++)
        {
            colliders[i].enabled = true;
        }
    }
    
    //Sets the weapon to its desired state
    public void SetWeaponState()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        joints = GetComponents<ConfigurableJoint>();
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        switch (currentWeaponState)
        {
            //Dropped, no collision, floats and can be picked up
            case WEAPON_STATE.DROPPED:
                canTake = true;
                for (int i = 1; i <= colliders.Length -1; i++)
                {
                    colliders[i].enabled = false;
                }
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].connectedBody = null;
                }
                GetComponent<BoxCollider>().enabled = true;
                transform.parent = null;
                foreach (Rigidbody body in bodies)
                {
                    body.isKinematic = true;
                    body.useGravity = false;
                }
                dropLight.enabled = true;
                dropAnimation.enabled = true;
                transform.eulerAngles = new Vector3(0, 0, 0);
                equipped = false;

                break;

                //Wielded, collision and rigidbodies enabled, animations and lights disabled
            case WEAPON_STATE.WIELDED:
                canTake = false;
                GetComponent<BoxCollider>().enabled = false;
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].xMotion = ConfigurableJointMotion.Limited;
                    joints[i].yMotion = ConfigurableJointMotion.Limited;
                    joints[i].zMotion = ConfigurableJointMotion.Limited;
                    joints[i].angularXMotion = ConfigurableJointMotion.Limited;
                    joints[i].angularYMotion = ConfigurableJointMotion.Limited;
                    joints[i].angularZMotion = ConfigurableJointMotion.Limited;
                    SoftJointLimit limit = joints[i].linearLimit;
                    limit.limit = 0;
                    joints[i].linearLimit = limit;

                }
                StartCoroutine("CollidersOn");
                foreach (Rigidbody body in bodies)
                {
                    body.isKinematic = false;
                    body.useGravity = true;
                }
                foreach (WeaponDamage wd in damageDealers)
                {
                    wd.ownHP = wd.FindOwnHP();
                }
                dropLight.enabled = false;
                dropAnimation.enabled = false;
                equipped = true;
                timer = true;
                break;

                //Thrown, colliders and rigidbodies enabled, joints free for all kind of movement
            case WEAPON_STATE.THROWN:
                canTake = false;

                for (int i = 1; i <= colliders.Length - 1; i++)
                {
                    colliders[i].enabled = true;
                }
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].connectedBody = null;
                    joints[i].xMotion = ConfigurableJointMotion.Free;
                    joints[i].yMotion = ConfigurableJointMotion.Free;
                    joints[i].zMotion = ConfigurableJointMotion.Free;
                    joints[i].angularXMotion = ConfigurableJointMotion.Free;
                    joints[i].angularYMotion = ConfigurableJointMotion.Free;
                    joints[i].angularZMotion = ConfigurableJointMotion.Free;
                    joints[i].autoConfigureConnectedAnchor = true;
                }
                GetComponent<BoxCollider>().enabled = true;
                transform.parent = null;
                foreach (Rigidbody body in bodies)
                {
                    body.isKinematic = false;
                    body.useGravity = true;
                }

                break;
        }
    }
    //Check if there is something to be on during dropped state. If not, destroy weapon
    public void RayCastToGround()
    {
        RaycastHit hit;
        
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 4) && currentWeaponState == WEAPON_STATE.DROPPED)
        {
            Destroy(gameObject);
        }
    }
}
