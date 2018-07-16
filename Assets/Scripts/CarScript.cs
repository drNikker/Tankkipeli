using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    float swerweAmount;
    public float maxSwerweAmount;
    public float minSwerweAmount;
    public float maxSpeed;
    public float baseDamage;
    float speed;
    float acceleration;
    Rigidbody rb;
    public bool IsSwerwing;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 0;
        acceleration = Random.Range(0.01f, 0.05f);
        swerweAmount = Random.Range(minSwerweAmount, maxSwerweAmount);
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = this.gameObject.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", Random.ColorHSV());
        rend[0].SetPropertyBlock(_propBlock);

    }
    private void Update()
    {
        if (IsSwerwing)
        {
            transform.Rotate(0, Time.deltaTime * swerweAmount, 0);
        }
    }

    void FixedUpdate()
    {
        if (speed<maxSpeed)
        {
            speed += acceleration;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
            rb.AddForce(Vector3.down * 1500);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            StartCoroutine(carStop());
        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(speed*baseDamage);
        }
    }
    IEnumerator carStop()
    {
        yield return new WaitForSeconds(0.5f);
        speed = 0;
        maxSpeed = 0;
        IsSwerwing = false;
    }
    
}
