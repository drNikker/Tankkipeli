using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    PlayerHealth ownHP;
    PlayerHealth health;
    Rigidbody tankBase;
    Rigidbody weapon;
    public Transform weaponParent;


    public float baseDamage = 5;
    public float dmgMultiplier = 2f;
    public float cooldownTime = 1;
    float knockback = 2000;

    float cooldown;
    float finalDamage;

    bool canEquip;

    public WEAPON_STATE currentWeaponState;


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

        if (canEquip == false && Input.GetKeyDown(KeyCode.K) && currentWeaponState == WEAPON_STATE.WIELDED /* || canEquip == false && Input.GetButton(player + "RB") && currentWeaponState == WEAPON_STATE.WIELDED*/)
        {
            //if (transform.parent.tag == "LeftHand")
            //{
            currentWeaponState = WEAPON_STATE.DROPPED;
            SetWeaponState();
            //Throw left hand weapon


            //}

        }
        if (canEquip == false && Input.GetKeyDown(KeyCode.L) && currentWeaponState == WEAPON_STATE.WIELDED /* || canEquip == false && Input.GetButton(player + "LB") && currentWeaponState == WEAPON_STATE.WIELDED*/)
        {
            //if (transform.parent.tag == "RightHand")
            //{
                currentWeaponState = WEAPON_STATE.DROPPED;
                SetWeaponState();
                //Throw right hand weapon
                ConfigurableJoint[] joints = GetComponentsInParent<ConfigurableJoint>();
                for (int i = 0; i <= joints.Length - 1; i++)
                {
                    joints[i].breakForce = 0;
            }
                transform.parent = null;
            //}
        }
    }

    public void Equip()
    {
        currentWeaponState = WEAPON_STATE.WIELDED;
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
                    if (finalDamage >= 25)
                    {
                        finalDamage = 25;                                           //Damage is capped at 25 for now
                    }
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
    void OnTriggerEnter(Collider collision)
    {
       canEquip = true;
    }
    void OnTriggerExit(Collider collision)
    {
       canEquip = false;
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
            if (parentOb.tag == "Tankbase")
            {
                Rigidbody rig = parentOb.GetComponent<Rigidbody>();
                return rig;
            }
            parentOb = parentOb.transform.parent;
        }

        Debug.LogWarning("Bodypart took a hit, but player tankbase was not found");
        return null;
    }



    public enum WEAPON_STATE
    {
        DROPPED,
        WIELDED,
        THROWN,
    }


    public void SetWeaponState()
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        ConfigurableJoint[] joints = weaponParent.GetComponents<ConfigurableJoint>();

        switch (currentWeaponState)
        {
            case WEAPON_STATE.DROPPED:
                for (int i = 0; i <= colliders.Length -1; i++)
                {
                    colliders[i].enabled = false;
                }
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].connectedBody = null;
                }

                weaponParent.parent = null;
                weaponParent.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                weaponParent.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                weaponParent.transform.eulerAngles = new Vector3(0, 0, 0);

                break;

            case WEAPON_STATE.WIELDED:
                canEquip = false;
                for (int i = 0; i <= colliders.Length -1; i++)
                {
                    colliders[i].enabled = true;
                }
                weaponParent.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                weaponParent.gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
                ownHP = FindOwnHP();

                break;

            case WEAPON_STATE.THROWN:
                canEquip = false;
                for (int i = 0; i <= colliders.Length -1; i++)
                {
                    colliders[i].isTrigger = false;
                }
                this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;

                break;
        }

    }

}
