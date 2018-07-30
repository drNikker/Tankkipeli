using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlEffect : MonoBehaviour {

    public List<ParticleSystem> partSys;
    int whirlLevel;

	// Use this for initialization
	void Start () {
        SetLevel(0);
	}
	
	// Update is called once per frame
	public void SetLevel (int level) {
        for (int i = 0; i < partSys.Count; i++)
        {
            if(i < level)
            {
                partSys[i].gameObject.SetActive(true); 
            }
            else
            {
                partSys[i].gameObject.SetActive(false);
            }
        }
	}
}
