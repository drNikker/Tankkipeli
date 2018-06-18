using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    PlayerHealth health;
    Rigidbody tankBase;


    public float baseDamage = 5;
    public float dmgMultiplier = 1.2f;
    public float cooldownTime = 1;
    public float knockback = 100;

    float cooldown;
    float finalDamage;


	// Use this for initialization
	void Start () {
	}
	

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
        {
            health = FindHP(collision);
            tankBase = FindTank(collision);
            finalDamage = baseDamage * dmgMultiplier * (collision.relativeVelocity.magnitude/10);       //Deal damage based on the damage values and the force of the impact
            if (finalDamage >= 25)
            {
                finalDamage = 25;                                           //Damage is capped at 25 for now
            }
            health.TakeDamage(finalDamage);
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


}
