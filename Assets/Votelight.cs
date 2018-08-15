using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Votelight : MonoBehaviour {



    public Animator VoteLight;
    public float count = 0;
    public Collider Box;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Spine01")
        {
           
            count++;

        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Spine01")
        {
            count--;
        }
    }


    void Update ()
    {
        VoteLight.SetFloat("Blend", count);
	}
}
