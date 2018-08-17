using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampFace : MonoBehaviour {

    static int openState = Animator.StringToHash(""); 

    private Animator myAnim;
    public Animator bridgeAni;

	// Use this for initialization
	void Start () {
        myAnim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(bridgeAni.GetCurrentAnimatorStateInfo(0).nameHash == openState)
        {
            myAnim.SetBool("openEyes", true);
           
        }
	}
}
