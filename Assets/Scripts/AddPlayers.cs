using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayers : MonoBehaviour {


    bool joined1;
    bool joined2;
    bool joined3;
    bool joined4;


    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!joined1 && Input.GetButtonDown("0"))
        {
            joined1 = true;
            StatHolder.HowManyPlayers += 1;
        }
        if (!joined2 && Input.GetButtonDown("0"))
        {
            joined2 = true;
            StatHolder.HowManyPlayers += 1;
        }
        if (!joined3 && Input.GetButtonDown("0"))
        {
            joined3 = true;
            StatHolder.HowManyPlayers += 1;
        }
        if (!joined4 && Input.GetButtonDown("0"))
        {
            joined4 = true;
            StatHolder.HowManyPlayers += 1;
        }
    }
}
