using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    [Header("Distances")]
    public float stoppingDistance = 2f;   // how close agents will follow
    public float pushBackDistance = 1f;   // how close before agent moves back
    public float pushBackSpeed = 3f;      // how fast to back away

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        // Auto-find player by tag
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pushBackDistance)
        {
            // Push back: move in opposite direction from the player
            Vector3 dirAway = (transform.position - player.position).normalized;
            Vector3 newPos = transform.position + dirAway * pushBackSpeed * Time.deltaTime;

            agent.isStopped = true; // temporarily stop NavMesh pathing
            agent.Move(dirAway * pushBackSpeed * Time.deltaTime);
        }
        else
        {
            agent.isStopped = false; // resume NavMesh pathing
            agent.SetDestination(player.position);
        }
    }
}
