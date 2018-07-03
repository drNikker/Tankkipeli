using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public Animator anim;
    private Rigidbody punchRB;

    [HideInInspector]
    public bool punchTimer, holdOffTimer, waitTimer;
    [Space(10)]
    public float punchTimerTime;
    protected float defaultPunchTimerTime;

    public float holdOffTimerTime;
    protected float originalHoldOffTimerTime;

    public float waitTimerTime;
    protected float originalWaitTimerTime;
    [Space(10)]
    public float stopRotation;
    [Space(10)]
    public float damage;

    protected float cooldownTime = 1;
    protected float cooldown;

    protected virtual void Start()
    {
        punchRB = gameObject.GetComponent<Rigidbody>();

        waitTimer = true;

        defaultPunchTimerTime = punchTimerTime;
        originalHoldOffTimerTime = holdOffTimerTime;
        originalWaitTimerTime = waitTimerTime;
    }
}
