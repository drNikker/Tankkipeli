using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public GameObject fist;
    public Transform target;

    protected bool punchTimer;
    [Space(10)]
    public float punchTimerTime;
    protected float originalPunchTimerTime;

    protected bool fistDistanceTimer;
    public float fistDistanceTimerTime;
    protected float originalFistDistanceTimerTime;

    protected bool holdOffTimer;
    public float holdOffTimerTime;
    protected float originalHoldOffTimerTime;

    [Space(10)]
    public float punchSpeed;

    [Space(10)]
    public float x;
    public float y;
    public float z;

    private Rigidbody punchRB;

    protected virtual void Start()
    {
        punchRB = fist.GetComponent<Rigidbody>();

        punchTimer = true;
        originalPunchTimerTime = punchTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
    }

    void Update()
    {
        /*
        currentPos = target.transform.localPosition;
        if (plöö)
        {
            Debug.Log(currentPos);
            Vector3 pos = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y * punchSpeed, target.transform.localPosition.z);
            transform.position = pos;
            plöö = false;
        }*/
    }

    protected void Punch(float x, float y, float z)
    {   
        punchRB.velocity = fist.transform.localPosition = new Vector3(x, y, z * punchSpeed);

        //punchRB.velocity = new Vector3(10, 0, 0) * punchSpeed;

        fistDistanceTimer = true;

    }

    protected void FistDistanceTimer()
    {
        fistDistanceTimerTime -= Time.deltaTime;

        if (fistDistanceTimerTime <= 0)
        {
            punchRB.velocity = Vector3.zero;
            punchRB.angularVelocity = Vector3.zero;

            holdOffTimer = true;
            fistDistanceTimer = false;
        }
    }

    

    protected void FistGoesBack(Transform targetPOS)
    {
        //punchRB.velocity = fist.transform.position = new Vector3(-1, -0.2f, 0);

        //punchRB.velocity = fist.transform.position = new Vector3(-1, 0.58f, -1 * punchSpeed);

        //punchRB.velocity = Vector3.MoveTowards(transform.localPosition, target.position, 1) * punchSpeed;

        transform.position = Vector3.Lerp(transform.position, targetPOS.position, 1);
        punchTimer = true;
    }
}
