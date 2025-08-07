using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject obstacle;          // Monster prefab
    public GameObject flockTarget;       // Flock target prefab

    GameObject[] agents;

    void Start()
    { 
        agents = GameObject.FindGameObjectsWithTag("agent");
    }

    void Update()
    {
        // Left Click: Spawn Monster
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Instantiate(obstacle, hit.point, obstacle.transform.rotation);
                foreach (GameObject a in agents)
                {
                    a.GetComponent<AIControl>().DetectNewObstacle(hit.point);
                }
            }
        }

        // Right Click: Spawn Flock Target
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Instantiate(flockTarget, hit.point, flockTarget.transform.rotation);
                foreach (GameObject a in agents)
                {
                    a.GetComponent<AIControl>().MoveToFlockTarget(hit.point);
                }
            }
        }
    }
}
