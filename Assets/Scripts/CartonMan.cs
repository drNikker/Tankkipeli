using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartonMan : MonoBehaviour {

    public List<Material> colorMats;

    private Animator anim;
    public float animSpeedMin;
    public float animSpeedMax;

	// Use this for initialization
	void Start () {

        //Randomize the material
        var myRend = gameObject.transform.Find("CartonManMesh").GetComponent<SkinnedMeshRenderer>();
        Material[] tempMats = myRend.materials;

        int rndNo1 = Random.Range(0, colorMats.Count);
        tempMats[0] = colorMats[rndNo1];

        myRend.materials = tempMats;

        //Get the animator
        anim = gameObject.GetComponent<Animator>();
        anim.speed = Random.Range(animSpeedMin, animSpeedMax);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
