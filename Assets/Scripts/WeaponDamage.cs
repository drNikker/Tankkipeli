﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [HideInInspector] public PlayerHealth ownHP;
    RoundManager roundManager;
    [HideInInspector]
    public PlayerHealth health;
    WeaponAudio weaponAudio;
    Rigidbody tankBase;
    static Color color;
    static Color color2;
    public float baseDamage = 16;
    public float dmgMultiplier = 1f;
    public float knockbackMultiplier = 1f;
    public float cooldownTime = 1;
    public float knockback = 400000;
    MultiTargetCamera MultiTargetCamera;

    // Hit Particles-------------
    ParticleSystem hitParticle;
    ParticleSystem.MainModule hitPartMain;
    int finalDamageVFX;
    // Hit Particles-------------
    float cooldown;
    [HideInInspector]
    public float finalDamage;


    // Use this for initialization
    void Start()
    {
        MultiTargetCamera = GameObject.Find("Main Camera").GetComponent<MultiTargetCamera>();
        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        weaponAudio = gameObject.GetComponentInParent<WeaponAudio>();
        hitParticle = GetComponent<ParticleSystem>();
        hitPartMain = hitParticle.main;
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
            tankBase = FindTank(collision);
            if (ownHP != health)
            {
                finalDamage = baseDamage * dmgMultiplier * (Mathf.Clamp(GetWeaponVelocity(), 1, 15) / 10);       //Deal damage based on the damage values and the force of the impact


                if (StatHolder.CurrentMode == StatHolder.Modes.TDM)
                {
                    if ((!roundManager.redPlayers.Contains(this.gameObject.transform.root.gameObject) && roundManager.redPlayers.Contains(collision.transform.root.gameObject)) || (!roundManager.bluePlayers.Contains(this.gameObject.transform.root.gameObject) && roundManager.bluePlayers.Contains(collision.transform.root.gameObject)))
                    {
                        health.TakeDamage(finalDamage);                                  //Tells how much damage to deal
                    }
                }
                else
                {
                    health.TakeDamage(finalDamage);
                }
                if (collision.relativeVelocity.magnitude >= 5)
                {
                    if (gameObject.transform.parent.name == "GreatAxe(Clone)" || gameObject.transform.parent.name == "GreatAxeScaled(Clone)")
                    {
                        gameObject.transform.parent.GetComponent<AxeSound>().playSound();
                    }
                    else
                    {
                        weaponAudio.RandomizeWeaponAudio();
                    }
                }  
                Vector3 dir = collision.transform.position - transform.position;
                dir.y = 0;
                tankBase.AddForce(dir.normalized * (knockback * Mathf.Clamp(GetWeaponVelocity(), 1, 15) * knockbackMultiplier));
                cooldown = Time.time + cooldownTime;                             //Puts the weapon on cooldown to avoid spam

                // Hit Particles--------------------------------------
                finalDamageVFX = Mathf.RoundToInt(finalDamage);
                hitPartMain.startLifetime = (0.05f * finalDamageVFX);
                hitPartMain.startSpeed = (1f * finalDamageVFX);
                hitParticle.Emit(5 * finalDamageVFX);
                // Hit Particles--------------------------------------
            }

        }


    }

    float GetWeaponVelocity()
    {
        float speed = GetComponent<Rigidbody>().velocity.magnitude;
        if (speed >= 15)
        {
            MultiTargetCamera.shakeDuration = 0.3f;
        }
        return speed;
        
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
