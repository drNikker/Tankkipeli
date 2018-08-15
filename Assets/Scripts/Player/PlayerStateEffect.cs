using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateEffect : MonoBehaviour
{

    public GameObject smokePartObj01;
    public GameObject smokePartObj02;

    public bool dizzyStart;
    public bool criticalStart;
    public bool deadStart;

    public bool effectStop;

    [SerializeField] public bool dizzyUpdate;
    [SerializeField] public bool criticalUpdate;
    [SerializeField] public bool deadUpdate;

    private bool hided;

    public GameObject DeathSprite;
    public GameObject StarSprite;
    public GameObject PlayerNoSprite;

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
        //gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

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
        spawnedDeath.transform.Find("DeathMesh").GetComponent<MeshRenderer>().enabled = false;

        //Critical particles Off
        smokePartObj01.GetComponent<ParticleSystem>().Stop();
        //Death particles Off
        smokePartObj02.GetComponent<ParticleSystem>().Stop();

    }

    // Update is called once per frame
    void Update()
    {
        //Start part
        if (dizzyStart)
        {
            if (deadUpdate)
            {
                effectStop = true;
                deadUpdate = false;
            }
            dizzyUpdate = true;
            dizzyStart = false;
        }

        if (criticalStart)
        {
            criticalUpdate = true;
            smokePartObj01.GetComponent<ParticleSystem>().Play();
            criticalStart = false;
        }

        if (deadStart)
        {
            deadUpdate = true;
            smokePartObj02.GetComponent<ParticleSystem>().Play();
            smokePartObj01.GetComponent<ParticleSystem>().Stop();
            dizzyUpdate = false;
            deadStart = false;
        }
        
        if (effectStop)
        {
            smokePartObj01.GetComponent<ParticleSystem>().Stop();
            smokePartObj02.GetComponent<ParticleSystem>().Stop();
            dizzyUpdate = false;
            criticalUpdate = false;
            deadUpdate = false;

            effectStop = false;
        }

        //Effect rotating and scaling.
        if (!hided && !deadUpdate && !criticalUpdate)
        {
            {
                for (int i = 0; i < spawnedStars.Count; i++)
                {
                    spawnedStars[i].transform.LookAt(Camera.main.transform.position, Vector3.up);
                }
                if (dizzyUpdate)
                {
                    transform.Rotate(0, dizzyRotSpeed * Time.deltaTime,  0);
                }
                
            }
        }
        
        if (!dizzyUpdate && !deadUpdate && !criticalUpdate)
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, 0, ref velocity, 10f * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        
        //Scale to 1
        if (transform.localScale.x < 1)
        {
            if (dizzyUpdate)
            {
                float newScale = Mathf.SmoothDamp(transform.localScale.x, 1, ref velocity, 10f * Time.deltaTime);
                transform.localScale = new Vector3(newScale, newScale, newScale);
                hided = false;
                for (int i = 0; i < spawnedStars.Count; i++)
                {
                    spawnedStars[i].GetComponent<SpriteRenderer>().enabled = true;
                    spawnedStars[1].GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }

            if (criticalUpdate)
            {
                float newScale = Mathf.SmoothDamp(transform.localScale.x, 1, ref velocity, 10f * Time.deltaTime);
                transform.localScale = new Vector3(newScale, newScale, newScale);
                hided = false;

            }
            if (deadUpdate)
            {
                float newScale = Mathf.SmoothDamp(transform.localScale.x, 1, ref velocity, 10f * Time.deltaTime);
                transform.localScale = new Vector3(newScale, newScale, newScale);
                spawnedDeath.transform.LookAt(Camera.main.transform.position, Vector3.up);
                spawnedDeath.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
                spawnedDeath.transform.Find("DeathMesh").GetComponent<MeshRenderer>().enabled = true;
                hided = false;

            }
        }
        
        //If hided
        if (transform.localScale.x < 0.05f)
        {
            hided = true;
            for (int i = 0; i < spawnedStars.Count; i++)
            {
                spawnedStars[i].GetComponent<SpriteRenderer>().enabled = false;
                spawnedStars[i].GetComponentInChildren<MeshRenderer>().enabled = false;
            }
            spawnedDeath.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    
}
