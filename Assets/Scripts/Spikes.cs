using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    PlayerHealth health;
    Rigidbody spikes;
    Rigidbody tankBase;

    public float baseDamage = 16;
    public float cooldownTime = 1;
    public float knockback = 400000;

    float cooldown;
    float finalDamage;

    void Start()
    {
        spikes = GetComponent<Rigidbody>();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
        {
            health = FindHP(collision);
            tankBase = FindTank(collision);

            finalDamage = baseDamage * (Mathf.Clamp(collision.relativeVelocity.magnitude, 1, 15) / 10);       //Deal damage based on the damage values and the force of the impact
            health.TakeDamage(finalDamage);                                  //Tells how much damage to deal
            Vector3 dir = collision.transform.position - transform.position;
            dir.y = 0;
            tankBase.AddForce(dir.normalized * knockback);
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
