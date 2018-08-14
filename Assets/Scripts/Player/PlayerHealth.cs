using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    RoundManager roundManager;
    PhysicMovement1 movement;
    AudioScript audioScript;
    //level camera for multicam
    MultiTargetCamera LevelCam;
    public PlayerStateEffect playerStateEffect;
    float maxHealth = 100;
    public float currHealth = 100;
    bool lastStand = false;
    //[HideInInspector]
    public PLAYER_STATE currentState;
    static Color Red = new Color(0.3962264f, 0.03551085f, 0.08502093f, 1);
    static Color Blue = new Color(0.115744f, 0.1928815f, 0.4811321f, 1);
    static Color Cyan = new Color(0.05793876f, 0.5849056f, 0.429675f, 1);
    static Color Yellow = new Color(0.9433962f, 0.9042832f, 0.2002492f, 1);
    static Color Green = new Color(0, 0.1886792f, 0.0004195716f, 1);
    static Color Purple = new Color(0.4823529f, 0.1176471f, 0.479214f, 1);
    static Color Orange = new Color(0.8867924f, 0.3786893f, 0.1547704f, 1);
    static Color Lime = new Color(0.4082314f, 0.945098f, 0.2f, 1);
    static Color Pink = new Color(1, 0.259434f, 0.3445413f, 1);
    static Color Crimson = new Color(0.8018868f, 0.1080861f, 0.05673727f, 1);
    static Color LightBlue = new Color(0, 0.629f, 1, 1);
    static Color DarkBlue = new Color(0.001972034f, 0, 0.8392157f, 1);
    Color[] colorSet = { Red, Blue, Cyan, Yellow, Green, Purple, Orange, Lime };
    Color color;
    Color skinColor;
    ParticleSystem vfxWin;

    // Use this for initialization
    void Start()
    {
        playerStateEffect = gameObject.GetComponentInChildren<PlayerStateEffect>();
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        movement = GetComponent<PhysicMovement1>();
        LevelCam = GameObject.FindWithTag("MainCamera").GetComponent<MultiTargetCamera>();
        LevelCam.AddTarget(transform);
        roundManager = GameObject.Find("GameManager1").GetComponent<RoundManager>();
        vfxWin = this.gameObject.transform.Find("VFX_Win").gameObject.GetComponent<ParticleSystem>();

        currHealth = maxHealth;
        roundManager.alivePlayers.Add(this.gameObject);
        currentState = PLAYER_STATE.ALIVE;
        SetPlayerState();
        SetColor();
    }

    void SetColor()
    {
        if (StatHolder.CurrentMode == StatHolder.Modes.TDM)
        {
            int colorInt = 0;
            int c = 0;
            GameObject player = roundManager.alivePlayers[roundManager.alivePlayers.Count - 1];
            ParticleSystem.MainModule winMain = vfxWin.main;
            switch (player.name)
        {
            case "Player1(Clone)":
                color = colorSet[StatHolder.Player1Color];
                    colorInt = StatHolder.Player1Color;
                    c = StatHolder.Player1SkinColor;
                    break;
            case "Player2(Clone)":
                color = colorSet[StatHolder.Player2Color];
                    colorInt = StatHolder.Player2Color;
                    c = StatHolder.Player2SkinColor;
                    break;
            case "Player3(Clone)":
                color = colorSet[StatHolder.Player3Color];
                    colorInt = StatHolder.Player3Color;
                    c = StatHolder.Player3SkinColor;
                    break;
            case "Player4(Clone)":
                color = colorSet[StatHolder.Player4Color];
                    colorInt = StatHolder.Player4Color;
                    c = StatHolder.Player4SkinColor;
                    break;
            default:
                color = Color.cyan;
                break;
        }

            if (colorInt == 1)
            {
                roundManager.bluePlayers.Add(this.gameObject);
                winMain.startColor = new ParticleSystem.MinMaxGradient(Blue, Color.white);

                switch (c)
                {
                    case 0:
                        skinColor = LightBlue;
                        break;
                    case 1:
                        skinColor = DarkBlue;
                        break;
                }
            }
            else if (colorInt == 0)
            {
                roundManager.redPlayers.Add(this.gameObject);
                winMain.startColor = new ParticleSystem.MinMaxGradient(Color.red, Color.white);
                switch (c)
                {
                    case 0:
                        skinColor = Pink;
                        break;
                    case 1:
                        skinColor = Crimson;
                        break;
                }
            }
            //}
            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
            Renderer[] rend = player.GetComponentsInChildren<Renderer>();
            _propBlock.SetColor("_Color", color);
            rend[6].SetPropertyBlock(_propBlock); //Base
            rend[3].SetPropertyBlock(_propBlock); //Helmet
            _propBlock.SetColor("_Color", skinColor);
            rend[17].SetPropertyBlock(_propBlock);

        }

    }

    public void TakeDamage(float damage)
    {
        if (currentState != PLAYER_STATE.DEAD)
        {
            movement.StopAllCoroutines();
            movement.edgeRecovery = false;
            StartCoroutine(movement.RecoveryTimer(4));
            currHealth -= damage;
            CheckHP(currHealth);
            if (currHealth <= 0)            //If out of hp, kill player
            {
                KillPlayer();
            }
        }
    }

    public void VFX_Win()
    {
        vfxWin.Play();
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
                    if (color == Red)
                    {
                        roundManager.alivePlayers.Remove(this.gameObject);
                        roundManager.redPlayers.Remove(this.gameObject);
                    }
                    else if (color == Blue)
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

            if (StatHolder.CurrentMode == StatHolder.Modes.TDM && roundManager.redPlayers.Count > 0 && roundManager.alivePlayers.Count > 1 || StatHolder.CurrentMode == StatHolder.Modes.TDM && roundManager.bluePlayers.Count > 0 && roundManager.alivePlayers.Count > 1)
            {
                if (roundManager.teamWon == false)
                {
                    audioScript.PlayKnockOutSound();
                }
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
