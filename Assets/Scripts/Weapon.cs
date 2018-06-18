using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float dmgMultiplier;
    public float hitCooldown;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bodypart")
        {
           // FindHP(collision);
        }
    }

    //https://answers.unity.com/questions/28581/traverse-up-the-hierarchy-to-find-first-parent-wit.html

    /* static GameObject FindHP(Collision col)
     {
         PlayerHealth hp = col.gameObject.GetComponentInParent<PlayerHealth>();
         GameObject parentOb = col.gameObject;

         while (parentOb != null)
         {
             parentOb = col.gameObject;

             if (hp != null)
             {
                 return hp;
             }
         }

         if (hp == null)
         {
             print("ei löytyny");
         }
         else
         {
             print("löyty");
         }
     }

     */

}
