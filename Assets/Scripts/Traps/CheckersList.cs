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
    [Space(10)]
    public float dropSpeed;

    private int pickRandomIndex;
    private bool checkForNewGameObject;
    private int checkedDroppedObjects = 0;

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
    }

    private void Timer()
    {
        waitTimerUntilDropTime -= Time.deltaTime;

        if (waitTimerUntilDropTime <= 0)
        {
            if (checkers.Count <= 0)
            {
                waitTimerUntilDropTime = 0;
                waitTimerUntilDrop = false;
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

            rb = gameObjectToDrop.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * dropSpeed);

            waitTimerUntilDropTime = originalWaitTimerUntilDropTime;

            checkers.RemoveAt(pickRandomIndex);
            checkers.RemoveAll(list_item => list_item == null);
            Destroy(gameObjectToDrop, 6);

            checkedDroppedObjects++;
        }
    }
}
