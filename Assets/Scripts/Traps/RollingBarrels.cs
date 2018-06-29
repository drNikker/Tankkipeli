using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrels : MonoBehaviour
{
    public GameObject rollingBarrel;

    private Vector3 barrelPosition;
    private Vector3 rotation;
    [Space(10)]
    public int barrelPositionMinX;
    public int barrelPositionMaxX;
    public float barrelPositionY;
    public float BarrelpositionZ;

    private int pickRandomPositionX;
    [Space(10)]
    public float barrelWaitTimerTime;
    private float originalBarrelWaitTimerTime;

    private bool canSpawn;
    private bool barrelWaitTimer;

    void Start()
    {
        rotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);

        originalBarrelWaitTimerTime = barrelWaitTimerTime;

        canSpawn = true;
        barrelWaitTimer = true;
    }

    void Update()
    {
        if (canSpawn)
        {
            SpawnBarrels();
        }

        if (barrelWaitTimer)
        {
            BarrelWaitTimer();
        }
    }

    private void SpawnBarrels()
    {
        pickRandomPositionX = Random.Range(barrelPositionMinX, barrelPositionMaxX);
        barrelPosition = new Vector3(pickRandomPositionX, barrelPositionY, BarrelpositionZ);
        rollingBarrel = Instantiate(rollingBarrel, barrelPosition, Quaternion.Euler(rotation));

        canSpawn = false;
    }

    private void BarrelWaitTimer()
    {
        barrelWaitTimerTime -= Time.deltaTime;

        if (barrelWaitTimerTime <= 0)
        {
            canSpawn = true;
            barrelWaitTimerTime = originalBarrelWaitTimerTime;
        }
    }
}
