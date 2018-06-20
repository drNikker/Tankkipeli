using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public bool plöö;

    public GameObject fist;
    public Transform target;

    protected bool punchTimer;
    [Space(10)]
    public float startPunchTimerTime;
    public float defaultPunchTimerTime;

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

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    protected virtual void Start()
    {
        punchRB = gameObject.GetComponent<Rigidbody>();

        punchTimer = true;

        originalFistDistanceTimerTime = fistDistanceTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
    }

    void Update()
    {

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

            fistDistanceTimerTime = originalFistDistanceTimerTime;

            holdOffTimer = true;
            fistDistanceTimer = false;
        }
    }

    protected void FistGoesBack(Transform targetPOS)
    {
        //punchRB.velocity = fist.transform.position = new Vector3(-1, -0.2f, 0);

        //punchRB.velocity = fist.transform.position = new Vector3(-1, 0.58f, -1 * punchSpeed);

        //punchRB.velocity = Vector3.MoveTowards(transform.localPosition, targetPOS.position, 1) * punchSpeed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(transform.position, targetPOS.position, ref velocity, smoothTime);

        //transform.position = Vector3.Lerp(transform.position, targetPOS.position, 1) * Time.deltaTime;

        if (transform.position == targetPOS.position)
        {
            plöö = false;
        }

        punchTimer = true;
    }
}
