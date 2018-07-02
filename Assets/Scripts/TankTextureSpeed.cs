﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTextureSpeed : MonoBehaviour {

    public float speedL;
    public float speedR;
    Vector2 newTrackOffsetL;
    Vector2 newTrackOffsetR;
    Vector2 newCylinderOffsetL;
    Vector2 newCylinderOffsetR;

    float newWheelRotL;
    float newWheelRotR;
    public int trackMatNoL;
    public int trackMatNoR;
    public int wheelsMatNoL;
    public int wheelsMatNoR;
    public int cylinderMatNoL;
    public int cylinderMatNoR;

    public float mult;

    // Use this for initialization
    void Start () {

        newTrackOffsetL = new Vector2(0, 0);

    }
	
	// Update is called once per frame
	void Update () {

        //LIMIT LEFT SPEED
        if (speedL > 1)
        {
            speedL = 0.99f;
        }

        if (speedL < -1)
        {
            speedL = -0.99f;
        }

        //LIMIT RIGHT SPEED
        if (speedR > 1)
        {
            speedR = 0.99f;
        }

        if (speedR < -1)
        {
            speedR = -0.99f;
        }

        //LEFT
        if (speedL > 0.0019f || speedL < -0.0019f)
        {
            //TRACKS L
            newTrackOffsetL = new Vector2(newTrackOffsetL.x, gameObject.GetComponent<MeshRenderer>().materials[trackMatNoL].GetTextureOffset("_MainTex").y - speedL/10);
            gameObject.GetComponent<MeshRenderer>().materials[trackMatNoL].SetTextureOffset("_MainTex", newTrackOffsetL);

            //WHEEL SIDE L
            gameObject.GetComponent<MeshRenderer>().materials[wheelsMatNoL].SetFloat("_RotationSpeed", speedL * 1000f);

            //WHEELS CYLINDER L
            newCylinderOffsetL = new Vector2(newCylinderOffsetL.x, gameObject.GetComponent<MeshRenderer>().materials[cylinderMatNoL].GetTextureOffset("_MainTex").y - speedL / 10 * 1.25f);
            gameObject.GetComponent<MeshRenderer>().materials[cylinderMatNoL].SetTextureOffset("_MainTex", newCylinderOffsetL);

        }

        else
        {
            gameObject.GetComponent<MeshRenderer>().materials[wheelsMatNoL].SetFloat("_RotationSpeed", 0);
        }

        //RIGHT
        if (speedR > 0.0019f || speedR < -0.0019f)
        {
            //TRACKS R
            newTrackOffsetR = new Vector2(newTrackOffsetR.x, gameObject.GetComponent<MeshRenderer>().materials[trackMatNoR].GetTextureOffset("_MainTex").y - speedR/10);
            gameObject.GetComponent<MeshRenderer>().materials[trackMatNoR].SetTextureOffset("_MainTex", newTrackOffsetR);

            //WHEEL SIDE R
            gameObject.GetComponent<MeshRenderer>().materials[wheelsMatNoR].SetFloat("_RotationSpeed", speedR * 1000f);
            //Debug.Log("Heres");

            //WHEELS CYLINDER R
            newCylinderOffsetR = new Vector2(newCylinderOffsetR.x, gameObject.GetComponent<MeshRenderer>().materials[cylinderMatNoR].GetTextureOffset("_MainTex").y - speedR / 10 * 1.25f);
            gameObject.GetComponent<MeshRenderer>().materials[cylinderMatNoR].SetTextureOffset("_MainTex", newCylinderOffsetR);
        }

        else
        {
            gameObject.GetComponent<MeshRenderer>().materials[wheelsMatNoR].SetFloat("_RotationSpeed", 0);
        }
    }
}
