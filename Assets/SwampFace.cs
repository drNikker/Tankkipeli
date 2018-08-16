using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampFace : MonoBehaviour {

    private Animator myAnim;
    public Animator bridgeAni;

	// Use this for initialization
	void Start () {
        myAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(bridgeAni.GetBool("Open/Close") == true)
        {
            myAnim.SetBool("openEyes", true);
            myAnim.SetBool("openEyes", false);
        }
	}
}
