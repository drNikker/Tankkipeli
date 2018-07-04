using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkers : MonoBehaviour
{
    private Rigidbody rb;

    private bool waitTimerUntilDrop;
    public float waitTimerUntilDropTime;

    private bool waitUntilDestroy;
    [Space(10)]
    public float waitUntilDestroyTime;
    [Space(10)]
    public float dropSpeed;

    private Rigidbody weaponRB;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        waitTimerUntilDrop = true;
    }

    void Update()
    {
        if (waitTimerUntilDrop)
        {
            Timer();
        }

        if (waitUntilDestroy)
        {
            WaitUntilDestroy();
        }
    }

    private void Timer()
    {
        waitTimerUntilDropTime -= Time.deltaTime;

        if (waitTimerUntilDropTime <= 0)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * dropSpeed);
            waitTimerUntilDropTime = 0;

            waitUntilDestroy = true;
        }
    }

    private void WaitUntilDestroy()
    {
        waitUntilDestroyTime -= Time.deltaTime;

        if (waitUntilDestroyTime <= 0)
        {
            waitUntilDestroyTime = 0;
            rb.AddForce(Vector3.zero);
            waitTimerUntilDrop = false;
            waitUntilDestroy = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Weapon"))
        {
            weaponRB = collider.gameObject.GetComponent<Rigidbody>();

            weaponRB.isKinematic = false;
            weaponRB.useGravity = true;
            weaponRB.AddForce(Vector3.down * 100 * Time.deltaTime);

            //collider.gameObject.GetComponentInChildren<Rigidbody>().isKinematic = false;
            //collider.gameObject.GetComponentInChildren<Rigidbody>().useGravity = true;

            Debug.Log("Lattia alta.");
        }
    }
}
