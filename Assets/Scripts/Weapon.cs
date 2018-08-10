using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    WeaponDamage[] damageDealers;
    ConfigurableJoint[] joints;

    float baseCooldown = 4;
    float cooldown = 2;
    float totalMass;
    public float weaponThrowForce = 8000;
    public float maxSpeed = 10;

    bool timer = false;
    bool canEquip;
    [HideInInspector] public bool equipped;
    [HideInInspector] public bool canTake = false;
    [HideInInspector] public bool taken = false;

    public WEAPON_STATE currentWeaponState;
    public Stance stance;

    Animator dropAnimation;
    Light dropLight;

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
        if (currentWeaponState == WEAPON_STATE.THROWN && cooldown <= Time.time)
        {
            Dropped();
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        else if (currentWeaponState == WEAPON_STATE.THROWN)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);               
            }

        }

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

    public void Equip()
    {
        currentWeaponState = WEAPON_STATE.WIELDED;
        SetWeaponState();
    }

    public void Dropped()
    {
        currentWeaponState = WEAPON_STATE.DROPPED;
        SetWeaponState();
    }

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

    void GetMass()
    {
        totalMass = 0.1f;
        foreach (WeaponDamage wd in damageDealers)
        {
            Rigidbody r = wd.GetComponent<Rigidbody>();
            totalMass += r.mass;
        }
    }

    public enum Stance
    {
        NoStance,
        OneHanded,
        TwoHanded,
        TwoHandedTwinblade,
        FistWeapon,
    }

    public enum WEAPON_STATE
    {
        DROPPED,
        WIELDED,
        THROWN,
    }

    IEnumerator CollidersOn()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        yield return new WaitForSeconds(1f);
        for (int i = 1; i <= colliders.Length - 1; i++)
        {
            colliders[i].enabled = true;
        }
    }
    
    public void SetWeaponState()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        joints = GetComponents<ConfigurableJoint>();
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        switch (currentWeaponState)
        {
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

    public void RayCastToGround()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 4) && currentWeaponState == WEAPON_STATE.DROPPED)
        {
            Dropped();
        }

        else
        {
            if (currentWeaponState == WEAPON_STATE.DROPPED)
            {
                Destroy(gameObject);
            }

            //if (hit.transform == false && currentWeaponState == WEAPON_STATE.DROPPED)
            //{
            //    Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

            //    foreach (Rigidbody body in rigidBodies)
            //    {
            //        body.isKinematic = false;
            //        body.useGravity = false;
            //    }
            //}
        }
    }
}
