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
            transform.Rotate(0, Time.deltaTime * 110, 0);
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
        yield return new WaitForSeconds(5f);
        speed = 0;
        maxSpeed = 0;
        IsSwerwing = false;
    }
    
}
