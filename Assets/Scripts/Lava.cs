using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    PlayerHealth health;
    //Rigidbody spikes;
    Rigidbody tankBase;

    public float baseDamage = 5;
    public float dmgMultiplier = 2f;
    public float cooldownTime = 1;
    public float knockback = 200;

    float cooldown;
    float finalDamage;

    void Start()
    {
        //spikes = GetComponent<Rigidbody>();
    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
        {
            health = FindHP(collision);
        tankBase = FindTank(collision);

        finalDamage = baseDamage * dmgMultiplier * (collision.relativeVelocity.magnitude / 10);       //Deal damage based on the damage values and the force of the impact

        health.TakeDamage(finalDamage);                                  //Tells how much damage to deal
        print(collision.relativeVelocity.magnitude + " hit str");
        print(finalDamage + " dmg");
        Vector3 dir = collision.transform.position - transform.position;
        dir.y = 0;
        tankBase.AddForce(dir.normalized * (knockback * collision.relativeVelocity.magnitude));
        cooldown = Time.time + cooldownTime;                             //Puts the weapon on cooldown to avoid spam


        }
    }


    static PlayerHealth FindHP(Collision col)                  //Finds the players hp
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
