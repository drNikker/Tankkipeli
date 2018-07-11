using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersList : MonoBehaviour
{
    public List<GameObject> checkers = new List<GameObject>();

    private GameObject gameObjectToDrop;
    private Rigidbody rb;
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
            rb = gameObjectToDrop.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * dropSpeed);

            checkers.RemoveAt(pickRandomIndex);
            checkers.RemoveAll(list_item => list_item == null);
            Destroy(gameObjectToDrop, 6);

            waitTimerUntilDropTime = originalWaitTimerUntilDropTime;

            setRandomNumberTimer = true;
            waitTimer = false;
        }
    }

}
