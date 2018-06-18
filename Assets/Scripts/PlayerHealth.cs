using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    float maxHealth = 100;
    float currHealth = 100;

	// Use this for initialization
	void Start () {
        currHealth = maxHealth;
	}
	

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)            //If out of hp, kill player
        {
            KillPlayer();
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
