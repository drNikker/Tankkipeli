using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour {

    [HideInInspector] public PlayerHealth ownHP;
    PlayerHealth health;
    Rigidbody tankBase;
    Rigidbody weapon;


    public float baseDamage = 16;
    public float dmgMultiplier = 1f;
    public float knockbackMultiplier = 1f;
    public float cooldownTime = 1;
    public float knockback = 400000;

    float cooldown;
    float finalDamage;


    // Use this for initialization
    void Start () {
        weapon = GetComponent<Rigidbody>();
	}

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
        {
            health = FindHP(collision);
            tankBase = FindTank(collision);
            if (ownHP != health)
            {
                finalDamage = baseDamage * dmgMultiplier * (Mathf.Clamp(collision.relativeVelocity.magnitude, 1, 15) / 10);       //Deal damage based on the damage values and the force of the impact
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

    public PlayerHealth FindOwnHP()                  //Finds the players OWN hp
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
}
