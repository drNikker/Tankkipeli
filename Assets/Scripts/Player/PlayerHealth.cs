using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    RoundManager roundManager;
    AudioScript audioScript;
    //level camera for multicam
    MultiTargetCamera LevelCam;
    public PlayerStateEffect playerStateEffect;

    float maxHealth = 100;
    public float currHealth = 100;
    bool lastStand = false;
    //[HideInInspector]
    public PLAYER_STATE currentState;
    static Color Red = new Color(0.3962264f, 0.03551085f, 0.08502093f, 1f);
    static Color Blue = new Color(0.115744f, 0.1928815f, 0.4811321f, 0);
    static Color Cyan = new Color(0.05793876f, 0.5849056f, 0.429675f, 1);
    static Color Yellow = new Color(0.9433962f, 0.9042832f, 0.2002492f, 1);
    static Color Green = new Color(0, 0.1886792f, 0.0004195716f, 1);
    static Color Purple = new Color(0.4823529f, 0.1176471f, 0.479214f, 1);
    static Color Orange = new Color(0.8867924f, 0.3786893f, 0.1547704f, 1);
    static Color Lime = new Color(0.4082314f, 0.945098f, 0.2f, 1);
    Color[] colorSet = { Red, Blue, Cyan, Yellow, Green, Purple, Orange, Lime }; Color color;

    // Use this for initialization
    void Start()
    {
        playerStateEffect = gameObject.GetComponentInChildren<PlayerStateEffect>();
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        LevelCam = GameObject.FindWithTag("MainCamera").GetComponent<MultiTargetCamera>();
        LevelCam.AddTarget(transform);
        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        

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
                color = Color.cyan;
                break;
        }

        if (StatHolder.CurrentMode == StatHolder.Modes.TDM)
        {
            if (color == Color.blue)
            {
                roundManager.bluePlayers.Add(this.gameObject);
            }
            else if(color == Color.red)
            {
                roundManager.redPlayers.Add(this.gameObject);
            }
        }
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = player.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        rend[0].SetPropertyBlock(_propBlock);
        rend[1].SetPropertyBlock(_propBlock);

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
                playerStateEffect.criticalStart = true;
            }
        }
    }

    public void KillPlayer()
    {
        if (currentState != PLAYER_STATE.DEAD)
        {
            currentState = PLAYER_STATE.DEAD;
            SetPlayerState();
            LevelCam.RemoveTarget(transform.name);
            playerStateEffect.deadStart = true;

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
                    else if (color == Color.blue)
                    {
                        roundManager.alivePlayers.Remove(this.gameObject);
                        roundManager.bluePlayers.Remove(this.gameObject);
                    }
                    break;

            }
            roundManager.PlayerChecker();

            if (StatHolder.CurrentMode == StatHolder.Modes.DM && roundManager.alivePlayers.Count > 1)
            {
                audioScript.PlayKnockOutSound();
            }

            if (StatHolder.CurrentMode == StatHolder.Modes.TDM && roundManager.redPlayers.Count > 1 && roundManager.bluePlayers.Count > 1)
            {
                audioScript.PlayKnockOutSound();
            }
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
        //HeadUpright[] uprights = GetComponentsInChildren<HeadUpright>();
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
