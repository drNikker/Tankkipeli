using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    public bool raycast = true;
    public float swerweAmount;
    public float maxSwerweAmount;
    public float minSwerweAmount;
    public float maxSpeed;
    public float baseDamage;
    public float speed;
    float acceleration;
    Rigidbody rb;
    public bool IsSwerwing;
    public bool nascarCar;
    int i;
    private AudioScript audioScript;
    private AudioClip currentAudioClip;
    private AudioSource audioSource;
    float cooldown;
    ParticleSystem Nitro1;
    ParticleSystem Nitro2;

    public List<Material> colorMats;

    void Start()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        Nitro1 = gameObject.transform.Find("NascarNitroPartSys").GetComponent<ParticleSystem>();
        Nitro2 = gameObject.transform.Find("NascarNitroPartSys (1)").GetComponent<ParticleSystem>();

        //speed = 6;
        if (nascarCar)
        {
            speed = 20;
            i = Random.Range(0, 30);
            if (i < 1)
            {
                playHonkSound();
                acceleration = 0.00015f;
                maxSpeed = 20.14f;
                baseDamage = 0.6f;
            }
            else
            {
                baseDamage = 10;
                acceleration = Random.Range(0.024f, 0.026f);
            }
        }
        else
        {
            acceleration = Random.Range(0.02f, 0.05f);
        }
        //swerweAmount = Random.Range(minSwerweAmount, maxSwerweAmount);
        //MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        //Renderer[] rend = this.gameObject.GetComponentsInChildren<Renderer>();
        //rend[0].GetPropertyBlock(_propBlock);
        //_propBlock.SetColor("_Color", Random.ColorHSV());
        //rend[0].SetPropertyBlock(_propBlock);
        //_propBlock.SetColor("_Color", Color.red);
        //rend[1].SetPropertyBlock(_propBlock);
        //rend[2].SetPropertyBlock(_propBlock);
        //rend[3].SetPropertyBlock(_propBlock);
        //rend[4].SetPropertyBlock(_propBlock);
        //rend[5].SetPropertyBlock(_propBlock);
        //rend[6].SetPropertyBlock(_propBlock);
        //rend[7].SetPropertyBlock(_propBlock);
        //rend[8].SetPropertyBlock(_propBlock);
        
        var myRend = gameObject.transform.Find("NascarCarMesh").GetComponent<SkinnedMeshRenderer>();
        Material[] tempMats = myRend.materials;

        int rndNo1 = Random.Range(0, colorMats.Count);
        tempMats[0] = colorMats[rndNo1];

        int rndNo2 = Random.Range(0, colorMats.Count);

        while (rndNo1 == rndNo2)
        {
            rndNo2 = Random.Range(0, colorMats.Count);
        }
        
        tempMats[2] = colorMats[rndNo2];

        myRend.materials = tempMats;

        StartCoroutine(carDespawn(10));

    }
    private void Update()
    {
        RaycastHit hit ;
        if (raycast)
        {
            if (Physics.Raycast(transform.position, Vector3.up * 0.5f, out hit) || hit.collider != null && hit.collider.gameObject.tag != "Environment")
            {
                if (hit.collider.tag != "Weapon" && hit.collider.tag != "PlayArea" && hit.collider.tag != "Car")
                {
                    Nitro1.Stop();
                    Nitro2.Stop();
                    Debug.DrawRay(transform.position, Vector3.up * 5f, Color.green);
                    if (hit.collider.tag == "Untagged")
                    {
                        Debug.LogWarning("Stop");
                    }
                    speed = 0;
                    maxSpeed = 0;
                    acceleration = 0;
                    IsSwerwing = false;
                }
            }
        }
        if (IsSwerwing)
        {
            if (nascarCar)
            {

                if (i >= 1)
                {
                    transform.Rotate(0, Time.deltaTime * 100, 0);
                }
                else
                {
                    print(i);
                    print("prkl");
                    transform.Rotate(0, Time.deltaTime * Random.Range(40,50), 0);
                }

            }
            else
            {
                transform.Rotate(0, Time.deltaTime * swerweAmount, 0);
            }
        }
        else
        {
            transform.Rotate(0,0,0);
        }
        if(transform.position.y < -10)
        {

            Destroy(this.gameObject);
        }

    }

    void FixedUpdate()
    {
        if (speed >1)
        {
            if (speed >= maxSpeed)
            {
                acceleration = 0;
            }

            speed += acceleration;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
            rb.AddForce(Vector3.down * 20000);
        }
        else
        {
            IsSwerwing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged" && collision.gameObject.transform.root.tag != "Player" && collision.gameObject.transform.root.tag != "Weapon")
        {
            print(collision.gameObject.name);
            StartCoroutine(carStop(0.2f));
        }
        if(collision.gameObject.tag == "Player" && speed > 5 && rb.velocity.magnitude > 1)
        {
            if (cooldown <= Time.time)
            {

                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(speed*baseDamage);
                playSound();
                cooldown = Time.time + 2;
            }
        }
        if (collision.gameObject.tag == "Weapon" || collision.gameObject.transform.root.tag == "Weapon")
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider);
        }
        if (collision.gameObject.tag == "PlayArea")
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.tag == "Weapon" || other.gameObject.tag == "Weapon")
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other);
        }

    }


    IEnumerator carStop(float stoptime)
    {
        yield return new WaitForSeconds(stoptime);
        acceleration = 0;
        speed = 0;
        maxSpeed = 0;
        IsSwerwing = false;
        GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
        Nitro1.Stop();
        Nitro2.Stop();
    }
    IEnumerator carDespawn(float despawnTime)
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }

    private void playSound()
    {
        currentAudioClip = audioScript.hazardAudioList[5];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
    private void playHonkSound()
    {
        currentAudioClip = audioScript.hazardAudioList[6];
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }
}
