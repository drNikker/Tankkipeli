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
    public float cooldownTime = 1;
    public float knockback = 50000;

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


    void OnCollisionEnter(Collision collision)
    {

            if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
            {
                health = FindHP(collision);
                tankBase = FindTank(collision);
                if (ownHP != health)
                {
                    finalDamage = baseDamage * dmgMultiplier * (collision.relativeVelocity.magnitude / 10);       //Deal damage based on the damage values and the force of the impact
                    health.TakeDamage(finalDamage);                                  //Tells how much damage to deal
                    print(collision.relativeVelocity.magnitude + " hit str");
                    print(finalDamage + " dmg");
                    Vector3 dir = collision.transform.position - transform.position;
                    dir.y = 0;
                    tankBase.AddForce(dir.normalized * (knockback * weapon.mass * collision.relativeVelocity.magnitude));
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
                    colliders[i].isTrigger = false;
                }
                this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;

                break;
        }

    }

}
