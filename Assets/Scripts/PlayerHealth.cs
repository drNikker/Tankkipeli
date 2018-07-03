using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    RoundManager roundManager;

    float maxHealth = 100;
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
        SetColor();
    }
	
    void SetColor()
    {
        GameObject player = roundManager.alivePlayers[roundManager.alivePlayers.Count - 1];
        Color color;
        switch (player.name)
        {
            case "Player1(Clone)":
                color = new Vector4(StatHolder.Player1ColorX, StatHolder.Player1ColorY, StatHolder.Player1ColorZ, StatHolder.Player1ColorW);
                break;
            case "Player2(Clone)":
                color = new Vector4(StatHolder.Player2ColorX, StatHolder.Player2ColorY, StatHolder.Player2ColorZ, StatHolder.Player2ColorW);
                break;
            case "Player3(Clone)":
                color = new Vector4(StatHolder.Player3ColorX, StatHolder.Player3ColorY, StatHolder.Player3ColorZ, StatHolder.Player3ColorW);
                break;
            case "Player4(Clone)":
                color = new Vector4(StatHolder.Player4ColorX, StatHolder.Player4ColorX, StatHolder.Player4ColorX, StatHolder.Player4ColorW);
                break;
            default:
                color = Color.clear;
                break;
        }
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);
        //rend[2].SetPropertyBlock(_propBlock);
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
        roundManager.alivePlayers.Remove(this.gameObject);
        roundManager.playerChecker();
        
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
                GetComponent<PhysicMovement1>().enabled = true;
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

                GetComponent<PhysicMovement1>().enabled = false;
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
                
                GetComponent<PhysicMovement1>().enabled = false;
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
