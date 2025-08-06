using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal;
    float speed = 6.0f;
    float accuracy = 1.0f;
    float rotSpeed = 2.0f;
    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWaypointIndex = 0;
    Graph graph;

    void Start()
    {
        wps = wpManager.GetComponent<WaypointManager>().waypoints;
        graph = wpManager.GetComponent<WaypointManager>().graph;
        currentNode = wps[0];
    }

    void LateUpdate()
    {
        if (graph.getPathLength() == 0 || currentWaypointIndex == graph.getPathLength())
        {
            return;
        }

        currentNode = graph.getPathPoint(currentWaypointIndex);

        if (Vector3.Distance(currentNode.transform.position, transform.position) < accuracy)
        {
            currentWaypointIndex++;
        }

        if (currentWaypointIndex < graph.getPathLength())
        {
            goal = graph.getPathPoint(currentWaypointIndex).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        Time.deltaTime * rotSpeed);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    public void GoToHelipad()
    {
        graph.AStar(currentNode, wps[0]);
        currentWaypointIndex = 0;
    }

    public void GoToMountains()
    {
        graph.AStar(currentNode, wps[3]);
        currentWaypointIndex = 0;
    }

    public void GoToBarracks()
    {
        graph.AStar(currentNode, wps[14]);
        currentWaypointIndex = 0;
    }

    public void GoToRadar()
    {
        graph.AStar(currentNode, wps[4]);
        currentWaypointIndex = 0;
    }

    public void GoToRuins()
    {
        graph.AStar(currentNode, wps[6]);
        currentWaypointIndex = 0;
    }

    public void GoToRefinery()
    {
        graph.AStar(currentNode, wps[9]);
        currentWaypointIndex = 0;
    }

    public void GoToTankers()
    {
        graph.AStar(currentNode, wps[10]);
        currentWaypointIndex = 0;
    }

    public void GoToMiddle()
    {
        graph.AStar(currentNode, wps[16]);
        currentWaypointIndex = 0;
    }

    public void GoToCommand()
    {
        graph.AStar(currentNode, wps[5]);
        currentWaypointIndex = 0;
    }
}
