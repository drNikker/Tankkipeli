﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{


    // Set camera limits to be the size of the play area in the inspector. When Game starts Initialize() 
    [SerializeField] private float upperLimitX = 0, lowerLimitX = 0;
    [SerializeField] private float upperLimitZ = 0, lowerLimitZ = 0;
    [SerializeField] private bool showGizmos;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.2f;
    public float decreaseFactor = 1.5f;

    //winzoom
    private int targetsCount = 0;
    private int prevTargetsCount = 0;

    public List<Transform> targets;
    public Vector3 offset;
    public Vector3 centerPointOffset;
    public float smoothTime = 0.5f;
    public float winSmoothTime = 1f;
    public float minZoom = 40f;
    public float maxZoom = 30f;
    public float winZoom = 10f;
    public float zoomLimiter = 50F;

    private Vector3 velocity;
    private Camera cam;

    bool beginCameraSet = false;

    private void Start()
    {
        cam = GetComponent<Camera>();

    }

    private void LateUpdate()
    {
        //if (targets.Count == 0)
        //{
        //    return;
        //}

        Debug.DrawRay(GetCenterPoint(), Vector3.up, Color.red);

        //sets camera immediately to the position it should be in the beginning no matter where it is first in scene
        if (beginCameraSet == false)
        {
            Vector3 centerPoint = GetCenterPoint();
            centerPoint.x = Mathf.Clamp(centerPoint.x, lowerLimitX, upperLimitX);
            centerPoint.z = Mathf.Clamp(centerPoint.z, lowerLimitZ, upperLimitZ);
            Vector3 newPosition = centerPoint + offset;
            transform.position = newPosition;

            float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
            cam.fieldOfView = newZoom;

            beginCameraSet = true;
        }

        Move();
        Rotate();
        Zoom();

    }

    void Zoom()
    {
        if (targets.Count == 0)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, minZoom, Time.deltaTime); ;
        }
        else if (targets.Count == 1 && prevTargetsCount == 2 && targetsCount == 1 && StatHolder.CurrentMode == StatHolder.Modes.DM) //winzoom
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, winZoom, Time.deltaTime);
        }
        else
        {
            float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }

    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        //winzoom
        if (targets.Count == 1 && prevTargetsCount == 2 && targetsCount == 1 && StatHolder.CurrentMode == StatHolder.Modes.DM) //winzoom
        {
            centerPoint.x = targets[0].position.x;
        }
        else
        {
        centerPoint.x = Mathf.Clamp(centerPoint.x, lowerLimitX, upperLimitX);
        }


        centerPoint.z = Mathf.Clamp(centerPoint.z, lowerLimitZ, upperLimitZ);

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

        //camera shake
        if (shakeDuration > 0)
        {
            transform.position = transform.position + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    void Rotate()
    {
        //win zoom
        if (targets.Count == 1 && prevTargetsCount == 2 && targetsCount == 1 && StatHolder.CurrentMode == StatHolder.Modes.DM)
        {
            var finalTargetRotation = Quaternion.LookRotation(targets[0].position - transform.position, Vector3.up);
            finalTargetRotation.y = 0; finalTargetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, finalTargetRotation, Time.deltaTime * 2.0f);
        }
        else
        {

            Vector3 centerPoint = GetCenterPoint();
            centerPoint += centerPointOffset;

            centerPoint.x = Mathf.Clamp(centerPoint.x, lowerLimitX, upperLimitX);
            centerPoint.z = Mathf.Clamp(centerPoint.z, lowerLimitZ, upperLimitZ);
            centerPoint.y = 0;

            Debug.DrawRay(centerPoint, Vector3.up, Color.red);

            // Smoothly rotates towards target 
            var targetRotation = Quaternion.LookRotation(centerPoint - transform.position, Vector3.up);
            targetRotation.y = 0; targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        }
    }

    //Get greatest distance on X axis between players
    float GetGreatestDistance()
    {
        if (targets.Count == 0)
        {
            return 1;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    //get center position between all players
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 0)
        {
            return new Vector3((lowerLimitX + upperLimitX) / 2, 0, (lowerLimitZ + upperLimitZ) / 2);
        }
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;

    }

    //add player to targets list
    public void AddTarget(Transform targetToAdd)
    {
        targets.Add(targetToAdd);

        targetsCount++;
    }

    //remove player from targets list
    public void RemoveTarget(string targetName)
    {
        Transform targetToRemove = targets.Single(targ => targ.name == targetName);
        targets.Remove(targetToRemove);
        shakeDuration = 0.5f;

        prevTargetsCount = targetsCount;
        targetsCount--;

        //säädetään myöhemmin
        if (prevTargetsCount == 2 && targetsCount == 1)
        {
            smoothTime = winSmoothTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
            Gizmos.DrawWireCube(new Vector3((lowerLimitX + upperLimitX) / 2, 0, (lowerLimitZ + upperLimitZ) / 2), new Vector3(upperLimitX - lowerLimitX, 0, upperLimitZ - lowerLimitZ));
    }
}
