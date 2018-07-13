using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour {


    // Set camera limits to be the size of the play area in the inspector. When Game starts Initialize() 
    [SerializeField] private float upperLimitX, lowerLimitX;
    [SerializeField] private float upperLimitZ, lowerLimitZ;
    [SerializeField] private bool showGizmos;

    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50F;

    private Vector3 velocity;
    private Camera cam;
    private float speed = 10f;

    bool beginCameraSet = false;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
        
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Debug.DrawRay(GetCenterPoint(), Vector3.up, Color.red);

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
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        Zoom();


    }

    void Zoom()
    {
        //Debug.Log(GetGreatestDistance());
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        centerPoint.x = Mathf.Clamp(centerPoint.x, lowerLimitX, upperLimitX);
        centerPoint.z = Mathf.Clamp(centerPoint.z, lowerLimitZ, upperLimitZ);

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Rotate()
    {
        float step = speed * Time.deltaTime;

        Vector3 centerPoint = GetCenterPoint();
        centerPoint.x = Mathf.Clamp(centerPoint.x, lowerLimitX, upperLimitX);
        centerPoint.z = Mathf.Clamp(centerPoint.z, lowerLimitZ, upperLimitZ);

        Debug.DrawRay(centerPoint, Vector3.up, Color.red);
        
        transform.LookAt(centerPoint);
        
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
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
    }
    //remove player from targets list
    public void RemoveTarget(string targetName)
    {
        Transform targetToRemove = targets.Single(targ => targ.name == targetName);
        targets.Remove(targetToRemove);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
            Gizmos.DrawWireCube(new Vector3((lowerLimitX + upperLimitX) / 2, 0, (lowerLimitZ + upperLimitZ) / 2), new Vector3(upperLimitX - lowerLimitX, 0, upperLimitZ - lowerLimitZ));
    }
    
}
