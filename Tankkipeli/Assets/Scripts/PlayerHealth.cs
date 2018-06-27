﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    RoundManager roundManager;

    float maxHealth = 26;
    float currHealth = 100;
    bool lastStand = false;
    //[HideInInspector]
    public PLAYER_STATE currentState;

    // Use this for initialization
    void Start () {
        roundManager = GameObject.Find("GameManager").GetComponent<RoundManager>();
        currHealth = maxHealth;
        roundManager.alivePlayers.Add(this.gameObject);
        currentState = PLAYER_STATE.ALIVE;
        SetPlayerState();
    }
	

    public void TakeDamage(float damage)
    {
        if (currentState != PLAYER_STATE.DEAD)
        {
            currHealth -= damage;
            CheckHP(currHealth);
            if (currHealth <= 0)            //If out of hp, kill player
            {
                KillPlayer();
            }
        }
    }

    void CheckHP(float hp)
    {
        if (currentState != PLAYER_STATE.DEAD)
        {
            if (hp <= 25 && lastStand == false)
            {
                lastStand = true;
                currHealth = 25;
                HatRemover hat = GetComponentInChildren<HatRemover>();
                hat.RemoveHat();
            }
        }
    }

    void KillPlayer()
    {
        currentState = PLAYER_STATE.DEAD;
        SetPlayerState();
        //Game needs to recive info about player death
        roundManager.playerChecker();
        roundManager.alivePlayers.Remove(this.gameObject);
        
    }

    public enum PLAYER_STATE
    {
        ALIVE,
        STUNNED,
        DEAD,
    }


    public void SetPlayerState()
    {
        HandForce[] hands = GetComponentsInChildren<HandForce>();
        HeadUpright[] uprights = GetComponentsInChildren<HeadUpright>();
        FullRagdollMode[] ragmode = GetComponentsInChildren<FullRagdollMode>();

        switch (currentState)
        {
            case PLAYER_STATE.ALIVE:
                GetComponent<PhysicMovement>().enabled = true;
                foreach (HandForce hf in hands)
                {
                    hf.enabled = true;
                }
                foreach (HeadUpright up in uprights)
                {
                    up.enabled = true;
                }

                break;

            case PLAYER_STATE.STUNNED:

                GetComponent<PhysicMovement>().enabled = false;
                foreach (HandForce hf in hands)
                {
                    hf.enabled = false;
                }
                foreach (HeadUpright up in uprights)
                {
                    up.enabled = false;
                }
                break;

            case PLAYER_STATE.DEAD:
                
                GetComponent<PhysicMovement>().enabled = false;
                foreach (HandForce hf in hands)
                {
                    hf.enabled = false;
                }
                foreach (HeadUpright up in uprights)
                {
                    up.enabled = false;
                }
                foreach (FullRagdollMode rag in ragmode)
                {
                    rag.RagdollMode();
                }
                break;
        }
      
    }
}