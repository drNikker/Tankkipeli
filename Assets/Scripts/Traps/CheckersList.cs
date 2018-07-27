using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersList : MonoBehaviour
{
    public List<GameObject> checkers = new List<GameObject>();

    private Weapon weaponScript;

    private GameObject gameObjectToDrop;
    private Rigidbody rb;
    private Rigidbody fenceRB;
    private Animator anim;

    private bool setRandomNumberTimer;
    [Space(10)]
    public float setRandomNumberTime;
    private float originalSetRandomNumberTime;

    private bool waitTimer;
    [Space(10)]
    public float waitTimerUntilDropTime;
    private float originalWaitTimerUntilDropTime;
    [Space(10)]
    public float dropSpeed;

    private int pickRandomIndex;
    private bool checkForNewGameObject;
    private int checkedDroppedObjects = 0;

    void Start()
    {
        originalWaitTimerUntilDropTime = waitTimerUntilDropTime;
        originalSetRandomNumberTime = setRandomNumberTime;

        setRandomNumberTimer = true;
    }

    void Update()
    {
        if (setRandomNumberTimer)
        {
            Timer();
        }

        if (waitTimer)
        {
            WaitTimer();
        }
    }

    private void Timer()
    {
        setRandomNumberTime -= Time.deltaTime;

        if (setRandomNumberTime <= 0)
        {
            if (checkers.Count <= 0)
            {
                setRandomNumberTime = 0;
                setRandomNumberTimer = false;
            }

            if (checkedDroppedObjects < 4)
            {
                pickRandomIndex = Random.Range(0, checkers.Count);
                gameObjectToDrop = checkers[pickRandomIndex];

                if (gameObjectToDrop.CompareTag("Respawn"))
                {
                    checkForNewGameObject = true;
                }

                while (checkForNewGameObject)
                {
                    pickRandomIndex = Random.Range(0, checkers.Count);
                    gameObjectToDrop = checkers[pickRandomIndex];

                    if (gameObjectToDrop.tag != ("Respawn"))
                    {
                        checkForNewGameObject = false;
                    }
                }
            }

            if (checkedDroppedObjects >= 4)
            {
                pickRandomIndex = Random.Range(0, checkers.Count);
                gameObjectToDrop = checkers[pickRandomIndex];
            }
            anim = gameObjectToDrop.GetComponent<Animator>();
            anim.SetTrigger("Blink");

            setRandomNumberTime = originalSetRandomNumberTime;

            checkedDroppedObjects++;

            waitTimer = true;
            setRandomNumberTimer = false;
        }
    }

    private void WaitTimer()
    {
        waitTimerUntilDropTime -= Time.deltaTime;

        if (waitTimerUntilDropTime <= 0)
        {
            //RayCastToUp();

            /*
            if (weaponScript != null)
            {
                Debug.Log(weaponScript);
                weaponScript.RayCastToGround();
            }
            */

            rb = gameObjectToDrop.GetComponentInParent<Rigidbody>();
            fenceRB = gameObjectToDrop.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = false;
            fenceRB.isKinematic = false;

            rb.AddForce(Vector3.down * dropSpeed, ForceMode.Impulse);

            checkers.RemoveAt(pickRandomIndex);
            checkers.RemoveAll(list_item => list_item == null);
            Destroy(gameObjectToDrop, 5);

            waitTimerUntilDropTime = originalWaitTimerUntilDropTime;

            setRandomNumberTimer = true;
            waitTimer = false;
        }
    }

    public void RayCastToUp()
    {
        RaycastHit hit;

        if (Physics.SphereCast(gameObjectToDrop.transform.position, 10, Vector3.up, out hit, 10))
        {
            Debug.Log(hit);
            if (hit.transform.tag != null && hit.transform.tag == "Weapon")
            {
                Debug.Log(hit.transform.tag);
                weaponScript = hit.transform.root.GetComponent<Weapon>();
            }
        }
        else
        {
            //Debug.Log(hit);
        }
    }
}
