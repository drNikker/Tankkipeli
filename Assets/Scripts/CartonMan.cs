using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartonMan : MonoBehaviour {

    private Animator anim;
    public float animSpeedMin;
    public float animSpeedMax;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
        anim.speed = Random.Range(animSpeedMin, animSpeedMax);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
