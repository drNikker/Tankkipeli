using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Votelight2 : MonoBehaviour {



    public Animator Mayhem;
    public Animator Clash;
    public float count = 0;

    PlayerJoining joining;


    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name == "Spine01")
    //    {

    //        count++;

    //    }

    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.name == "Spine01")
    //    {
    //        count--;
    //    }
    //}
    private void Start()
    {
        joining = transform.parent.GetComponent<PlayerJoining>();
    }


    void Update ()
    {
        if (StatHolder.HowManyPlayers > 1)
        {
            count = StatHolder.HowManyPlayers;
        }
        else
        {
            count = 2;
        }

        Mayhem.SetFloat("Stars", count);
        Clash.SetFloat("Stars", count);


        Clash.SetFloat("Lights", joining.votes[0]);
        Mayhem.SetFloat("Lights", joining.votes[1]);
    }
}
