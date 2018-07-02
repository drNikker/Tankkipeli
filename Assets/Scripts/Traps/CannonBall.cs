using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CannonBall : MonoBehaviour
{
    public GameObject cannonBall;
    public GameObject[] cannonBalls;
    public int sizeOfTheList;

    private Vector3 cannonBallPosition;
    [Space(10)]
    public int cannonBallPositionMinX;
    public int cannonBallPositionMaxX;
    public float cannonBallPositionY;
    public float cannonBallPositionZ;

    private int pickRandomPositionX;
    private int randomedCannonBallPositionX;

    private bool cannonBallTimer;
    [Space(10)]
    public float cannonBallTimerTime;
    private float defaultCannonBallTimerTime;

    private bool generateNewRandom;

    void Start()
    {
        cannonBalls = new GameObject[sizeOfTheList];

        randomedCannonBallPositionX = 0;

        cannonBallTimer = true;

        defaultCannonBallTimerTime = cannonBallTimerTime;
    }

    void Update()
    {
        if (cannonBallTimer)
        {
            CannonBallTimer();
        }
    }

    private void SpawnCannonBalls()
    {
        for (int i = 0; i < cannonBalls.Length; i++)
        {
            pickRandomPositionX = Random.Range(cannonBallPositionMinX, cannonBallPositionMaxX);

            if (randomedCannonBallPositionX == pickRandomPositionX)
            {
                generateNewRandom = true;

                while (generateNewRandom)
                {
                    pickRandomPositionX = Random.Range(cannonBallPositionMinX, cannonBallPositionMaxX);

                    if (randomedCannonBallPositionX != pickRandomPositionX)
                    {
                        Debug.Log("jou uutta randomia generatee");
                        generateNewRandom = false;
                    }
                }
            }

            randomedCannonBallPositionX = pickRandomPositionX;

            cannonBallPosition = new Vector3(pickRandomPositionX, cannonBallPositionY, cannonBallPositionZ);

            GameObject spawnedCannonBall = Instantiate(cannonBall, cannonBallPosition, Quaternion.identity);
            cannonBalls[i] = spawnedCannonBall;

            Destroy(spawnedCannonBall, 5);
        }
    }

    private void CannonBallTimer()
    {
        cannonBallTimerTime -= Time.deltaTime;

        if (cannonBallTimerTime <= 0)
        {
            SpawnCannonBalls();

            cannonBallTimerTime = defaultCannonBallTimerTime;
        }
    }
}
