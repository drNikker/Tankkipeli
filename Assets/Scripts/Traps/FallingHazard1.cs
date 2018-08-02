using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard1 : MonoBehaviour
{
    public float maxSpawnTime = 10;
    public float minSpawnTime = 9;
    private bool canSpawn = false;
    public bool randomRotation;

    public List<GameObject> objects = new List<GameObject>();
    
    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnObject());
        }
    }

    //spawn falling objects. 
    IEnumerator SpawnObject()
    {
        canSpawn = false;
        CreateFallingObject();
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        canSpawn = true;
    }
    
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(3);
        canSpawn = true;
    }
    //Create object on top of a player. Player health used for locating players since it is destroyed when player dies.
    void CreateFallingObject()
    {
        PlayerHealth[] playerLocations = FindObjectsOfType<PlayerHealth>();
        Vector3 temp = playerLocations[Random.Range(0, playerLocations.Length)].transform.position;
        temp.y = 20;

        GameObject fallingObject = Instantiate(objects[Random.Range(0, objects.Count)], temp, transform.rotation);
        if (randomRotation)
        {
            fallingObject.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.one);
        }
    }

    //object destroying when they fall off the map
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FallingStuff")
        {
            Destroy(collider.gameObject);
        }
    }
}

