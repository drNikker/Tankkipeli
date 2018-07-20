﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

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
    //TEST

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 20;
        if (nascarCar)
        {
            i = Random.Range(0, 30);
            if (i < 1)
            {
                acceleration = 0.00015f;
                speed = 20;
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
        swerweAmount = Random.Range(minSwerweAmount, maxSwerweAmount);
        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer[] rend = this.gameObject.GetComponentsInChildren<Renderer>();
        rend[0].GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", Random.ColorHSV());
        rend[0].SetPropertyBlock(_propBlock);


    }
    private void Update()
    {
        RaycastHit hit ;

        if (Physics.Raycast(transform.position, -Vector3.up* -0.5f, out hit) || hit.collider != null && hit.collider.tag != "Environment")
        {
            speed = 0;
            maxSpeed = 0;
            acceleration = 0;
            IsSwerwing = false;
        }
        if (IsSwerwing)
        {
            if (nascarCar)
            {

                if (i > 1)
                {
                    transform.Rotate(0, Time.deltaTime * 100, 0);
                }
                else
                {
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
        if (speed<maxSpeed && speed >5)
        {
            speed += acceleration;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
            rb.AddForce(Vector3.down * 1500);
        }
        else
        {
            IsSwerwing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Untagged" && collision.gameObject.transform.root.tag != "Player")
        {
            StartCoroutine(carStop());
        }
        if(collision.gameObject.tag == "Player" && speed > 5)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(speed*baseDamage);
        }

    }
    IEnumerator carStop()
    {
        yield return new WaitForSeconds(0.2f);
        acceleration = 0;
        speed = 0;
        maxSpeed = 0;
        IsSwerwing = false;
    }
    
}