using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkers : MonoBehaviour
{
    private CheckersList checkersList;

    private Rigidbody rb;
    private Rigidbody weaponRB;

    void Start()
    {
        checkersList = GameObject.FindGameObjectWithTag("CheckersManager").GetComponent<CheckersList>();
        checkersList.checkers.Add(gameObject);

        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        
    }

    /*
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
    */
}
