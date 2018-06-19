using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    float maxHealth = 100;
    float currHealth = 100;
    bool lastStand = false;

	// Use this for initialization
	void Start () {
        currHealth = maxHealth;
	}
	

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        CheckHP(currHealth);
        if (currHealth <= 0)            //If out of hp, kill player
        {
            KillPlayer();
        }
    }

    void CheckHP(float hp)
    {
        if (hp <= 25 && lastStand == false)
        {
            lastStand = true;
            currHealth = 25;
            HatRemover hat = GetComponentInChildren<HatRemover>();
            hat.RemoveHat();
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
    }

}
