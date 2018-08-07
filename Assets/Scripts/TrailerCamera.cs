using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCamera : MonoBehaviour
{

    // Use this for initialization
    public float smoothTime = 500f;
    private Vector3 velocity;
    private Vector3 newPosition;
    private void Start()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("c"))
        {
            //transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);  // you can set the position as a whole, just not individual fields
            transform.Translate(new Vector3(0, 1 * Time.deltaTime, 0));
            transform.Rotate(Time.deltaTime * 12, 0, 0);

        }
        if (Input.GetKey("x"))
        {
            //transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);  // you can set the position as a whole, just not individual fields
            transform.Translate(new Vector3(0, 0, -1 * Time.deltaTime));

        }


    }
}
