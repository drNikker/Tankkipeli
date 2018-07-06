using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersList : MonoBehaviour
{
    public List<GameObject> checkers = new List<GameObject>();

    private GameObject gameObjectToDrop;
    private Rigidbody rb;

    private bool waitTimerUntilDrop;
    [Space(10)]
    public float waitTimerUntilDropTime;
    private float originalWaitTimerUntilDropTime;

    private bool waitUntilDestroy;
    [Space(10)]
    public float waitUntilDestroyTime;
    [Space(10)]
    public float dropSpeed;

    private int pickRandomIndex;
    public bool checkForNewGameObject;
    public int checkedDroppedObjects = 0;

    void Start()
    {
        originalWaitTimerUntilDropTime = waitTimerUntilDropTime;

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

        checkers.RemoveAll(list_item => list_item == null);
    }

    private void Timer()
    {
        waitTimerUntilDropTime -= Time.deltaTime;

        if (waitTimerUntilDropTime <= 0)
        {
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

            rb = gameObjectToDrop.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * dropSpeed);

            waitTimerUntilDropTime = originalWaitTimerUntilDropTime;

            checkers.RemoveAt(pickRandomIndex);
            Destroy(gameObjectToDrop, 6);

            checkedDroppedObjects++;
        }
    }

    private void WaitUntilDestroy()
    {
        waitUntilDestroyTime -= Time.deltaTime;

        if (waitUntilDestroyTime <= 0)
        {
            waitUntilDestroyTime = 0;
            rb.AddForce(Vector3.zero);
            waitTimerUntilDrop = true;
            waitUntilDestroy = false;
            Destroy(gameObject);
        }
    }
}
