using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffset : MonoBehaviour {

    private Material mat;
    public float xSpeed;
    public float ySpeed;

    // Use this for initialization
    void Start () {
        mat = gameObject.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 oldOffset = mat.GetTextureOffset("_MainTex");
        Vector2 newOffset = new Vector2 (oldOffset.x + xSpeed * Time.deltaTime, oldOffset.y + ySpeed * Time.deltaTime)  ;
        mat.SetTextureOffset("_MainTex", newOffset);


	}
}
