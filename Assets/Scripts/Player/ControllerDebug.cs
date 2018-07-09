using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDebug : MonoBehaviour
{

    public bool P1RT;
    public bool P1LT;
    public bool P2RT;
    public bool P2LT;
    public bool P3RT;
    public bool P3LT;
    public bool P4RT;
    public bool P4LT;


    string[] controllers;
    // Use this for initialization
    void Start()
    {
        controllers = Input.GetJoystickNames();

        for (int i = 0; i < controllers.Length; i++)
        {
            Debug.Log(controllers[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Buttons();
    }

    void Buttons()
    {
        P1RT = Input.GetAxis("P1TankThreadRight") > 0.0;
        P1LT = Input.GetAxis("P1TankThreadLeft") > 0.0;

        P2RT = Input.GetAxis("P2TankThreadRight") > 0.0;
        P2LT = Input.GetAxis("P2TankThreadLeft") > 0.0;

        P3RT = Input.GetAxis("P3TankThreadRight") > 0.0;
        P3LT = Input.GetAxis("P3TankThreadLeft") > 0.0;

        P4RT = Input.GetAxis("P4TankThreadRight") > 0.0;
        P4LT = Input.GetAxis("P4TankThreadLeft") > 0.0;
    }
}
