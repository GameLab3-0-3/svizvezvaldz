using System;
using System.Collections.Generic;
using UnityEngine;

public class WaiPoint : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();  
    public int WayPointSparizione;
    public bool IsMoving;
    public int wayPointIndex;
    public float moveSpeed;
    public bool isLooping;
    public float rotationSpeed;

    void Start()
    {
        StartMoving();
    }
    public void StartMoving()
    {
        wayPointIndex = 0;
        IsMoving = true;
    }
    void Update()
    {
        if(!IsMoving)
        {
            return;
        }
        if(wayPointIndex < waypoints.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].position, moveSpeed * Time.deltaTime);

            var direction = transform.position - waypoints[wayPointIndex].position;
            var targetRotation = Quaternion.LookRotation(-direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            var distance = Vector3.Distance(transform.position, waypoints[wayPointIndex].position);
            if(distance < 0.1f)
            {
                wayPointIndex++;
                if(isLooping && wayPointIndex >= waypoints.Count)
                {
                    wayPointIndex = 0;
                }
            }
        }
        // if(wayPointIndex >= WayPointSparizione)
        // {
        //     gameObject.SetActive(false);
        // }
    }
}