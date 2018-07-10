using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    RoundManager roundManager;

    float maxHealth = 100;
    public float currHealth = 100;
    bool lastStand = false;
    //[HideInInspector]
    public PLAYER_STATE currentState;
    Color[] colorSet = { Color.red, Color.blue, Color.green, Color.yellow, Color.white };
    Color color;

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
        switch (player.name)
        {
            case "Player1(Clone)":
                color = colorSet[StatHolder.Player1Color];
                break;
            case "Player2(Clone)":
                color = colorSet[StatHolder.Player2Color];
                break;
            case "Player3(Clone)":
                color = colorSet[StatHolder.Player3Color];
                break;
            case "Player4(Clone)":
                color = colorSet[StatHolder.Player4Color];
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
        rend[2].SetPropertyBlock(_propBlock);
        if(StatHolder.CurrentMode == StatHolder.Modes.TDM)
        {
            if (color == Color.red)
            {
                roundManager.redPlayers.Add(this.gameObject);
            }
            else if (color == Color.blue)
            {
                roundManager.bluePlayers.Add(this.gameObject);
            }
        }
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

    public void KillPlayer()
    {
        if (currentState != PLAYER_STATE.DEAD)
        {
            currentState = PLAYER_STATE.DEAD;
            SetPlayerState();
            //Game needs to recive info about player death
            switch (StatHolder.CurrentMode)
            {
                case StatHolder.Modes.DM:
                    roundManager.alivePlayers.Remove(this.gameObject);
                    break;
                case StatHolder.Modes.TDM:
                    if (color == Color.red)
                    {
                        roundManager.alivePlayers.Remove(this.gameObject);
                        roundManager.redPlayers.Remove(this.gameObject);
                    }
                    else if(color == Color.blue)
                    {
                        roundManager.alivePlayers.Remove(this.gameObject);
                        roundManager.bluePlayers.Remove(this.gameObject);
                    }
                    break;

            }
            roundManager.PlayerChecker();
        }
        
        
    }

    public enum PLAYER_STATE
    {
        ALIVE,
        STUNNED,
        DEAD,
    }


    public void SetPlayerState()
    {
        HandControls[] hands = GetComponentsInChildren<HandControls>();
        HeadUpright[] uprights = GetComponentsInChildren<HeadUpright>();
        FullRagdollMode[] ragmode = GetComponentsInChildren<FullRagdollMode>();

        switch (currentState)
        {
            case PLAYER_STATE.ALIVE:
                GetComponent<PhysicMovement1>().enabled = true;
                foreach (HandControls hf in hands)
                {
                    hf.enabled = true;
                }
               
                break;

            case PLAYER_STATE.STUNNED:

                foreach (HandControls hf in hands)
                {
                    hf.enabled = false;
                }
                break;

            case PLAYER_STATE.DEAD:
                
                GetComponent<PhysicMovement1>().enabled = false;
                foreach (HandControls hf in hands)
                {
                    hf.enabled = false;
                }

                foreach (FullRagdollMode rag in ragmode)
                {
                    rag.RagdollMode();
                }
                break;
        }
      
    }
}
