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
    public int cannonBallPositionMinZ;
    public int cannonBallPositionMaxZ;

    private int pickRandomPositionX;
    private int pickRandomPositionZ;
    private int randomedCannonBallPositionX;
    private int randomedCannonBallPositionZ;

    private bool cannonBallTimer;
    public float timerOffset; //Used to create alternating volleys
    [Space(10)]
    public float cannonBallTimerTime; //Max time the timer will reset to
    private float currentCannonBallTimer; //Displays current time

    private bool generateNewRandom;

    void Start()
    {
        cannonBalls = new GameObject[sizeOfTheList];

        randomedCannonBallPositionX = 0;
        randomedCannonBallPositionZ = 0;


        currentCannonBallTimer = 0;
    }

    void Update()
    {

        startOffset();

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

            if (randomedCannonBallPositionX == pickRandomPositionX || randomedCannonBallPositionX == (pickRandomPositionX + 1) || randomedCannonBallPositionX == (pickRandomPositionX -1))
            {
                generateNewRandom = true;

                while (generateNewRandom)
                {
                    pickRandomPositionX = Random.Range(cannonBallPositionMinX, cannonBallPositionMaxX);

                    if (randomedCannonBallPositionX != pickRandomPositionX)
                    {
                        //Debug.Log("jou uutta randomia generatee");
                        generateNewRandom = false;
                    }
                }
            }

            pickRandomPositionZ = Random.Range(cannonBallPositionMinZ, cannonBallPositionMaxZ);

            if (randomedCannonBallPositionZ == pickRandomPositionZ || randomedCannonBallPositionZ == (pickRandomPositionZ + 1) || randomedCannonBallPositionZ == (pickRandomPositionZ - 1))
            {
                generateNewRandom = true;

                while (generateNewRandom)
                {
                    pickRandomPositionZ = Random.Range(cannonBallPositionMinZ, cannonBallPositionMaxZ);

                    if (randomedCannonBallPositionZ != pickRandomPositionZ)
                    {
                        //Debug.Log("jou uutta randomia generatee");
                        generateNewRandom = false;
                    }
                }
            }

            randomedCannonBallPositionX = pickRandomPositionX;

            cannonBallPosition = new Vector3(pickRandomPositionX, cannonBallPositionY, pickRandomPositionZ);

            GameObject spawnedCannonBall = Instantiate(cannonBall, cannonBallPosition, Quaternion.identity);
            cannonBalls[i] = spawnedCannonBall;

            Destroy(spawnedCannonBall, 5);
        }
    }

    private void CannonBallTimer()
    {
        currentCannonBallTimer -= Time.deltaTime;

        if (currentCannonBallTimer <= 0)
        {
            SpawnCannonBalls();

            currentCannonBallTimer = cannonBallTimerTime;
        }
    }

    private void startOffset()
    {
        if (timerOffset > 0)
        {
            timerOffset -= Time.deltaTime;
        }
        else if (timerOffset <= 0)
        {
            cannonBallTimer = true;
        }
    }
}
