using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    public float swerweAmount;
    public float maxSwerweAmount;
    public float minSwerweAmount;
    public float maxSpeed;
    public float baseDamage;
    float speed;
    float acceleration;
    Rigidbody rb;
    public bool IsSwerwing;
    public bool nascarCar;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 20;
        acceleration = Random.Range(0.02f, 0.03f);
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

        if (Physics.Raycast(transform.position, -Vector3.up* -5, out hit) || hit.collider != null && hit.collider.tag != "Environment")
        {
            speed = 0;
            maxSpeed = 0;
            IsSwerwing = false;
        }
        if (IsSwerwing)
        {
            if (nascarCar)
            {
                int i = Random.Range(0, 1);
                switch (i)
                {
                    case 0:
                    transform.Rotate(0, Time.deltaTime * 110, 0);
                        break;
                    case 1:
                        speed = 50;
                        transform.Rotate(0, Time.deltaTime * 40, 0);
                        break;
                }
            }
            else
            {
                transform.Rotate(0, Time.deltaTime * swerweAmount, 0);
            }
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
        yield return new WaitForSeconds(10);
        speed = 0;
        maxSpeed = 0;
        IsSwerwing = false;
    }
    
}
