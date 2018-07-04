using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 12f;                 // How fast the tank moves forward and back.
    public float turnSpeed = 180f;            // How fast the tank turns in degrees per second.

    float turnCheck;                          // How fast to turn, if at all.
    float moveCheck;                          // How fast to go, if at all.

    Rigidbody rg;

    // Use this for initialization
    void Start () {
        rg = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        KeyPresses();
    }

    void Turn(float rot)
    {
        float turn = turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, 0f, turn * rot);

        rg.MoveRotation(rg.rotation * turnRotation);
    }

    void Move(float move)
    {
        Vector3 movement = transform.up * -1 * speed * move * Time.deltaTime;

        rg.MovePosition(rg.position + movement);
    }

    void KeyPresses()
    {
        moveCheck = 0;
        turnCheck = 0;

        if (Input.GetKey(KeyCode.Keypad9))
        {
            moveCheck += 1;
            turnCheck -= 1;
        }
        if (Input.GetKey(KeyCode.Keypad7))
        {
            moveCheck += 1;
            turnCheck += 1;
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            moveCheck -= 1;
            turnCheck += 1;
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            moveCheck -= 1;
            turnCheck -= 1;
        }
        Turn(turnCheck);
        Move(moveCheck);
    }

}
