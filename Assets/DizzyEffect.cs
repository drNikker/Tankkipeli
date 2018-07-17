using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyEffect : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject smokePartObj;

    private bool changeToDead;
    private bool changeToDizzy;

    public bool dizzy;
    public bool dead;
    private bool hided;

    public GameObject DeathSprite;
    public GameObject StarSprite;
    public int starCount = 5;
    public List<GameObject> spawnedStars;
    public GameObject spawnedDeath;
    public float dizzyRotSpeed;
    public float yOffset;

    public float radius = 3f;
    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;

    private float velocity; 

    // Use this for initialization
    void Start()
    {
        playerHealth = gameObject.transform.root.GetComponent<PlayerHealth>();
        //Spawnes ste star structure at the start
        Theta = 0f;
        Size = (int)((starCount) + 1f);

        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * 1 / starCount);
            float x = transform.position.x + radius * Mathf.Cos(Theta);
            float z = transform.position.z + radius * Mathf.Sin(Theta);

            GameObject newStar = Instantiate(StarSprite, new Vector3(x, transform.position.y + yOffset, z), transform.rotation);
            newStar.transform.parent = gameObject.transform;
            spawnedStars.Add(newStar);
      
        }
        
        //Hides stars at the start
        for (int i = 0; i < spawnedStars.Count; i++)
        {
            spawnedStars[i].GetComponent<SpriteRenderer>().enabled = false;
        }

        //Spawns and hides dead icon
        spawnedDeath = Instantiate(DeathSprite, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), transform.rotation);
        spawnedDeath.transform.parent = gameObject.transform;

        spawnedDeath.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;

        //Death particles Off
        smokePartObj.GetComponent<ParticleSystem>().Stop();

    }

    // Update is called once per frame
    void Update()
    {

        //Effect rotating and scaling. 
        if (!hided && !dead)
        {
            {
                for (int i = 0; i < spawnedStars.Count; i++)
                {
                    spawnedStars[i].transform.LookAt(Camera.main.transform.position, Vector3.up);
                }

                transform.Rotate(0, dizzyRotSpeed, 0 * Time.deltaTime);
            }
        }

        if (!dizzy && !dead)
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, 0, ref velocity, 10f * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        
        //Scale to 1
        if(transform.localScale.x < 1)
        {
            if(dead)
            {
                float newScale = Mathf.SmoothDamp(transform.localScale.x, 1, ref velocity, 10f * Time.deltaTime);
                transform.localScale = new Vector3(newScale, newScale, newScale);
                hided = false;
                smokePartObj.GetComponent<ParticleSystem>().Play();
            }

            if (dizzy)
            {
                float newScale = Mathf.SmoothDamp(transform.localScale.x, 1, ref velocity, 10f * Time.deltaTime);
                transform.localScale = new Vector3(newScale, newScale, newScale);
                hided = false;
                for (int i = 0; i < spawnedStars.Count; i++)
                {
                    spawnedStars[i].GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }

        //If hided
        if (transform.localScale.x < 0.05f)
        {
            hided = true;
            for (int i = 0; i < spawnedStars.Count; i++)
            {
                spawnedStars[i].GetComponent<SpriteRenderer>().enabled = false;
                
            }

            spawnedDeath.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
            smokePartObj.GetComponent<ParticleSystem>().Stop();


            if (changeToDead)
            {
                dead = true;
                changeToDead = false;
            }
            if (changeToDizzy)
            {
                dizzy = true;
                changeToDizzy = false;
            }

        }

        if(dead)
        {
            spawnedDeath.transform.LookAt(Camera.main.transform.position, Vector3.up);
            spawnedDeath.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;

        }    
        
        //if(playerHealth.currentState == ALIVE)
        //{

        //}
    }
}
