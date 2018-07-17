using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    Rigidbody weapon;
    WeaponDamage[] damageDealers;
    public Transform weaponParent;

    float cooldown = 2;
    float totalMass;

    bool canEquip;
    [HideInInspector] public bool equipped;
    [HideInInspector] public bool canTake = false;

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
        weapon = weaponParent.GetComponent<Rigidbody>();
        currentWeaponState = WEAPON_STATE.DROPPED;
        SetWeaponState();
	}

    private void Update()
    {
        if (currentWeaponState == WEAPON_STATE.THROWN && weapon.velocity.x < 1 && weapon.velocity.z < 1 && cooldown <= Time.time)
        {
            Dropped();
            weaponParent.transform.position = new Vector3(weaponParent.transform.position.x, weaponParent.transform.position.y + 1, weaponParent.transform.position.z);
        }
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
        cooldown = Time.time + cooldown;
        currentWeaponState = WEAPON_STATE.THROWN;
        SetWeaponState();
        Rigidbody rb = weaponParent.GetComponent<Rigidbody>();
        rb.AddForce((front + new Vector3(0,0.2f,0)) * 8000 * totalMass);
        print(totalMass);
    }

    void GetMass()
    {
        totalMass = 0;
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
        Collider[] colliders = weaponParent.GetComponentsInChildren<Collider>();
        yield return new WaitForSeconds(0.2f);
        for (int i = 1; i <= colliders.Length - 1; i++)
        {
            colliders[i].enabled = true;
        }
    }

    public void SetWeaponState()
    {
        Collider[] colliders = weaponParent.GetComponentsInChildren<Collider>();
        ConfigurableJoint[] joints = weaponParent.GetComponents<ConfigurableJoint>();
        Rigidbody[] bodies = weaponParent.GetComponentsInChildren<Rigidbody>();

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
                weaponParent.GetComponent<BoxCollider>().enabled = true;
                weaponParent.parent = null;
                foreach (Rigidbody body in bodies)
                {
                    body.isKinematic = true;
                    body.useGravity = false;
                }
                dropLight.enabled = true;
                dropAnimation.enabled = true;
                weaponParent.transform.eulerAngles = new Vector3(0, 0, 0);
                equipped = false;

                break;

            case WEAPON_STATE.WIELDED:
                canTake = false;
                weaponParent.GetComponent<BoxCollider>().enabled = false;
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].xMotion = ConfigurableJointMotion.Limited;
                    joints[i].yMotion = ConfigurableJointMotion.Limited;
                    joints[i].zMotion = ConfigurableJointMotion.Limited;
                    joints[i].angularXMotion = ConfigurableJointMotion.Limited;
                    joints[i].angularYMotion = ConfigurableJointMotion.Limited;
                    joints[i].angularZMotion = ConfigurableJointMotion.Limited;
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
                weaponParent.GetComponent<BoxCollider>().enabled = true;
                weaponParent.parent = null;
                foreach (Rigidbody body in bodies)
                {
                    body.isKinematic = false;
                    body.useGravity = true;
                }

                break;
        }

    }

}
