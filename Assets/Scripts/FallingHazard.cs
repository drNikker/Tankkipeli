﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{

    public int spawnTime = 10;
    public bool randomPosition;
    public int XPositionLowerLimit;
    public int XPositionUpperLimit;
    public int ZPositionLowerLimit;
    public int ZPositionUpperLimit;
    public float sizeUpperLimit;
    public float sizeLoweLimit;

    public List<GameObject> objects = new List<GameObject>();


    // Use this for initialization
    void Start()
    {
        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), this.transform.position.y, Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }

       StartCoroutine(SpawnObject());
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray spawnRay = new Ray(this.gameObject.transform.position, Vector3.down);

        if (!Physics.Raycast(spawnRay, out hit, 10000) && randomPosition || hit.collider.tag != "Environment" && randomPosition)
        {

            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), this.transform.position.y, Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));

        }
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(spawnTime);
        CreateFallingObject();
    }
    void CreateFallingObject()
    {
        GameObject fallingObject = Instantiate(objects[Random.Range(0, objects.Count)], this.gameObject.transform.position, Quaternion.AngleAxis(Random.Range(0,360), Vector3.one));
        fallingObject.transform.localScale = new Vector3(Random.Range(sizeLoweLimit, sizeUpperLimit), Random.Range(sizeLoweLimit, sizeUpperLimit), Random.Range(sizeLoweLimit, sizeUpperLimit));
        if (randomPosition)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(XPositionLowerLimit, XPositionUpperLimit), this.transform.position.y, Random.Range(ZPositionLowerLimit, ZPositionUpperLimit));
        }
        StartCoroutine(SpawnObject());

    }

}
