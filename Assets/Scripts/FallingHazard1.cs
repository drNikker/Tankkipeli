using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard1 : MonoBehaviour
{

    public float maxSpawnTime = 10;
    public float minSpawnTime = 9;
    private bool canSpawn = true;
    public bool randomRotation;

    public List<GameObject> objects = new List<GameObject>();


    // Use this for initialization
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnObject()
    {
        
        CreateFallingObject();
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
    }

    void CreateFallingObject()
    {
        GameObject fallingObject = Instantiate(objects[Random.Range(0, objects.Count)], this.gameObject.transform.position, transform.rotation);
        if(randomRotation)
        {
            fallingObject.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.one);
        }
        
        PlayerHealth[] playerLocations = FindObjectsOfType<PlayerHealth>();
        Vector3 temp = playerLocations[Random.Range(0, playerLocations.Length - 1)].transform.position;
        temp.y = 50;
        this.gameObject.transform.position = temp;
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FallingStuff")
        {
            Destroy(collider.gameObject);
        }
    }
}

