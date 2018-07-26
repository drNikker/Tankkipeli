using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMesher : MonoBehaviour {

    public List<GameObject> startClouds;
    public List<GameObject> cloudsToMove;

    public float movementSpeed;

    // Use this for initialization
    void Start () {
        cloudsToMove.AddRange(startClouds);
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < cloudsToMove.Count; i++)
        {
            Vector3 newPos = new Vector3(cloudsToMove[i].transform.position.x + movementSpeed, cloudsToMove[i].transform.position.y, cloudsToMove[i].transform.position.z);
            cloudsToMove[i].transform.position = newPos;
        }
		
	}
}
