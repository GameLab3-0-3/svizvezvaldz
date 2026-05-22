using System;
using UnityEngine;

public class WaiPoint : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public int currentWaypointIndex = 0;
    public bool isLooping;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Vector3 destination = waypoints[currentWaypointIndex].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newPos;

        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.1f)
        {
            if(currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else if (isLooping)
            {
                if(isLooping)
                {
                    currentWaypointIndex = 0;
                }
            }
        }
    }
}
