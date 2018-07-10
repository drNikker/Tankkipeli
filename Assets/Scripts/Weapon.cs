using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    PlayerHealth ownHP;
    PlayerHealth health;
    Rigidbody tankBase;
    Rigidbody weapon;
    public Transform weaponParent;


    public float baseDamage = 16;
    public float dmgMultiplier = 1f;
    public float knockbackMultiplier = 1f;
    public float cooldownTime = 1;
    public float knockback = 400000;

    float cooldown;
    float finalDamage;

    bool canEquip;
    public bool equipped;

    public WEAPON_STATE currentWeaponState;
    public Stance stance;


	// Use this for initialization
	void Start ()
    {
        weapon = GetComponent<Rigidbody>();
        ownHP = FindOwnHP();
        currentWeaponState = WEAPON_STATE.DROPPED;
        SetWeaponState();
	}

    private void Update()
    {
        if (currentWeaponState == WEAPON_STATE.THROWN && weapon.velocity.x < 1 && weapon.velocity.z < 1 && cooldown <= Time.time)
        {
            Dropped();
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
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
        cooldown = Time.time + cooldownTime;
        currentWeaponState = WEAPON_STATE.THROWN;
        SetWeaponState();
        Rigidbody rb = weaponParent.GetComponent<Rigidbody>();
        rb.AddForce((front + new Vector3(0,0.2f,0)) * 8000 * weapon.mass);
    }


    void OnCollisionEnter(Collision collision)
    {

            if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
            {
                health = FindHP(collision);
                tankBase = FindTank(collision);
                if (ownHP != health)
                {
                    finalDamage = baseDamage * dmgMultiplier * (Mathf.Clamp(collision.relativeVelocity.magnitude,1,15) / 10);       //Deal damage based on the damage values and the force of the impact
                    health.TakeDamage(finalDamage);                                  //Tells how much damage to deal
                    print(Mathf.Clamp(collision.relativeVelocity.magnitude, 1, 15) + " hit str");
                    print(finalDamage + " dmg");
                    Vector3 dir = collision.transform.position - transform.position;
                    dir.y = 0;
                    tankBase.AddForce(dir.normalized * (knockback * weapon.mass * Mathf.Clamp(collision.relativeVelocity.magnitude, 1, 15) * knockbackMultiplier));
                    cooldown = Time.time + cooldownTime;                             //Puts the weapon on cooldown to avoid spam
                }

            }
        
    }

    static PlayerHealth FindHP(Collision col)                  //Finds the enemy players hp
     {

         PlayerHealth hp = col.gameObject.GetComponentInParent<PlayerHealth>();
         Transform parentOb = col.gameObject.transform;

         while (parentOb != null)
         {
            hp = parentOb.GetComponent<PlayerHealth>();

             if (hp != null)
             {
                 return hp;
             }
            parentOb = parentOb.transform.parent;
        }

        Debug.LogWarning("Bodypart took a hit, but player health was not found");
        return null;
     }

    PlayerHealth FindOwnHP()                  //Finds the players OWN hp
    {
        PlayerHealth hp = GetComponentInParent<PlayerHealth>();
        Transform parentOb = gameObject.transform;
        
        while (parentOb != null)
        {
            hp = parentOb.GetComponent<PlayerHealth>();

            if (hp != null)
            {
                return hp;
            }
            parentOb = parentOb.transform.parent;
        }

        Debug.LogWarning("Players health was not found! If the weapon is not assigned to hand, ignore this");
        return null;
    }

    static Rigidbody FindTank(Collision col)                  //Finds the players tank
    {
        Transform parentOb = col.gameObject.transform;

        while (parentOb != null)
        {
            if (parentOb.tag == "Player")
            {
                Rigidbody rig = parentOb.GetComponent<Rigidbody>();
                return rig;
            }
            parentOb = parentOb.transform.parent;
        }

        Debug.LogWarning("Bodypart took a hit, but player tankbase was not found");
        return null;
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



    public void SetWeaponState()
    {
        Collider[] colliders = weaponParent.GetComponentsInChildren<Collider>();
        ConfigurableJoint[] joints = weaponParent.GetComponents<ConfigurableJoint>();
        Rigidbody[] bodies = weaponParent.GetComponentsInChildren<Rigidbody>();

        switch (currentWeaponState)
        {
            case WEAPON_STATE.DROPPED:
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

                weaponParent.transform.eulerAngles = new Vector3(0, 0, 0);
                equipped = false;

                break;

            case WEAPON_STATE.WIELDED:
                canEquip = false;
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
                for (int i = 1; i <= colliders.Length -1; i++)
                {
                    colliders[i].enabled = true;
                }
                foreach (Rigidbody body in bodies)
                {
                    body.isKinematic = false;
                    body.useGravity = true;
                }
                ownHP = FindOwnHP();
                equipped = true;

                break;

            case WEAPON_STATE.THROWN:
                canEquip = false;
                for (int i = 1; i <= colliders.Length -1; i++)
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
                }
                weaponParent.GetComponent<BoxCollider>().enabled = true;
                weaponParent.parent = null;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;

                break;
        }

    }

}
