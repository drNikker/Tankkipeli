using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPull : MonoBehaviour {

    public ConfigurableJoint joint;
    public GameObject weapon;
    //Rigidbody rb;
    //public float power = 20;
    bool canEquip = false;


    void Start () {

	}

    void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button9) && canEquip == true)
        {
            print("r");
            //rb = weapon.GetComponent<Rigidbody>();
            //rb.MovePosition(transform.position + p1RightHand * power * Time.deltaTime);
            joint = weapon.GetComponent<ConfigurableJoint>();
            joint.connectedBody = GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = new Vector3(0.1f,0.15f,0);
            Transform t = weapon.GetComponent<Transform>();
            weapon.transform.parent = this.transform;
            t.localEulerAngles = new Vector3(0,0,0);


        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            canEquip = true;
            weapon = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canEquip = false;
        joint = null;
    }

}
