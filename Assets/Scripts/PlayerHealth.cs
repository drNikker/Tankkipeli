using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    RoundManager roundManager;

    float maxHealth = 26;
    float currHealth = 100;
    bool lastStand = false;
    [HideInInspector]
    public bool isAlive = true;

	// Use this for initialization
	void Start () {
        roundManager = GameObject.Find("GameManager").GetComponent<RoundManager>();
        currHealth = maxHealth;
        roundManager.alivePlayers.Add(this.gameObject);
	}
	

    public void TakeDamage(float damage)
    {
        if (isAlive)
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
        if (isAlive)
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
    {                                                           //Disable player scripts
        GetComponent<PhysicMovement>().enabled = false;
        HandForce[] hands = GetComponentsInChildren<HandForce>();
        foreach(HandForce hf in hands)
        {
            hf.enabled = false;
        }
        HeadUpright[] uprights = GetComponentsInChildren<HeadUpright>();
        foreach (HeadUpright up in uprights)
        {
            up.enabled = false;
        }
        FullRagdollMode[] ragmode = GetComponentsInChildren<FullRagdollMode>();
        foreach (FullRagdollMode rag in ragmode)
        {
            rag.RagdollMode();
        }
        //Game needs to recive info about player death
        roundManager.playerChecker();
        isAlive = false;
        roundManager.alivePlayers.Remove(this.gameObject);
        
    }

}
