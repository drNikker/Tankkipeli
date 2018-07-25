using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistDamage : MonoBehaviour {

    [HideInInspector] public PlayerHealth ownHP;
    RoundManager roundManager;
    PlayerHealth health;
    WeaponAudio weaponAudio;
    Rigidbody tankBase;
    static Color color;
    static Color color2;
    public float baseDamage = 2;
    public float cooldownTime = 1;

    // Hit Particles-------------
    ParticleSystem hitParticle;
    int finalDamageVFX;
    // Hit Particles-------------

    float cooldown;


    // Use this for initialization
    void Start()
    {
        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        weaponAudio = gameObject.GetComponentInParent<WeaponAudio>();
        hitParticle = GetComponent<ParticleSystem>();
        if (weaponAudio == null)
        {
            weaponAudio = transform.root.GetComponent<WeaponAudio>();
        }

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bodypart" && cooldown <= Time.time)
        {
            health = FindHP(collision);
            ownHP = FindOwnHP();
            if (ownHP != health)
            {

                // Hit Particles--------------------------------------
                finalDamageVFX = Mathf.RoundToInt(baseDamage);
                hitParticle.startLifetime = (0.05f * finalDamageVFX);
                hitParticle.startSpeed = (1f * finalDamageVFX);
                hitParticle.Emit(5 * finalDamageVFX);
                // Hit Particles--------------------------------------

                if (StatHolder.CurrentMode == StatHolder.Modes.TDM)
                {
                    if ((!roundManager.redPlayers.Contains(this.gameObject.transform.root.gameObject) && roundManager.redPlayers.Contains(collision.transform.root.gameObject)) || (!roundManager.bluePlayers.Contains(this.gameObject.transform.root.gameObject) && roundManager.bluePlayers.Contains(collision.transform.root.gameObject)))
                    {
                        health.TakeDamage(baseDamage);                                  //Tells how much damage to deal
                    }
                }
                else
                {
                    health.TakeDamage(baseDamage);
                }

                print("Somebody got slapped LUL");
                cooldown = Time.time + cooldownTime;                             //Puts the weapon on cooldown to avoid spam
            }

        }

    }

    static PlayerHealth FindHP(Collision col)                  //Finds the enemy players hp
    {

        PlayerHealth hp = col.gameObject.GetComponentInParent<PlayerHealth>();
        Transform parentOb = col.gameObject.transform;
        GameObject parentObject = col.gameObject;

        while (parentOb != null)
        {


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
}
