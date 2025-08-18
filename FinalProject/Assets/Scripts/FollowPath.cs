using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public GameObject wpManager;
    private GameObject[] wps;
    private Graph graph;

    private int currentWaypointIndex = 0;
    private int targetWaypoint = 1; // always start from 0 -> 1

    [Header("Tank Movement Settings")]
    public float speed = 6.0f;
    public float accuracy = 1.5f;
    public float rotSpeed = 2.0f;

    private GameObject currentNode;

    void Start()
    {
        wps = wpManager.GetComponent<WaypointManager>().waypoints;
        graph = wpManager.GetComponent<WaypointManager>().graph;

        if (wps.Length > 1)
        {
            currentNode = wps[0]; // starting node
            graph.AStar(currentNode, wps[targetWaypoint]);
            currentWaypointIndex = 0;
            bool pathFound = graph.AStar(currentNode, wps[targetWaypoint]);
            Debug.Log("Path found? " + pathFound + " | Path length: " + graph.getPathLength()); 
        }
    }

    void LateUpdate()
    {
        if (graph.getPathLength() == 0 || currentWaypointIndex >= graph.getPathLength())
            return;

        GameObject nextNode = graph.getPathPoint(currentWaypointIndex);

        // Move towards nextNode
        Vector3 lookAtGoal = new Vector3(nextNode.transform.position.x, transform.position.y, nextNode.transform.position.z);
        Vector3 direction = lookAtGoal - transform.position;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotSpeed
        );
        transform.Translate(0, 0, speed * Time.deltaTime);

        // Check if close enough to the node
        if (Vector3.Distance(nextNode.transform.position, transform.position) < accuracy)
        {
            Debug.Log("Moving toward: " + nextNode.name);
    
            currentNode = nextNode; // update current
            currentWaypointIndex++;

            // If reached the end of path → compute new path to next global waypoint
            if (currentWaypointIndex >= graph.getPathLength())
            {
                targetWaypoint++;
                if (targetWaypoint >= wps.Length)
                    targetWaypoint = 0;

                graph.AStar(currentNode, wps[targetWaypoint]);
                currentWaypointIndex = 0;
            }
        }
    }
}
