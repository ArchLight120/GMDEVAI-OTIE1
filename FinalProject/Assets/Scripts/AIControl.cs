using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIBehaviorType { Pursuer, Hider, Evader }

public class AIControl : MonoBehaviour
{
    public AIBehaviorType behaviorType;

    private NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;

    public float detectionRange = 15f;

    Vector3 wanderTarget;

    private enum Mode { Wander, Pursue, Hide, Evade }
    private Mode currentMode = Mode.Wander;  // Track current mode

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
        LogModeChange(Mode.Wander);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        switch (behaviorType)
        {
            case AIBehaviorType.Pursuer:
                if (distance <= detectionRange)
                    SetMode(Mode.Pursue, Pursue);
                else
                    SetMode(Mode.Wander, Wander);
                break;

            case AIBehaviorType.Hider:
                if (distance <= detectionRange /* && canSeeTarget() */)
                    SetMode(Mode.Hide, CleverHide);
                else
                    SetMode(Mode.Wander, Wander);
                break;

            case AIBehaviorType.Evader:
                if (distance <= detectionRange)
                    SetMode(Mode.Evade, Evade);
                else
                    SetMode(Mode.Wander, Wander);
                break;
        }
    }

    // Helper to call mode methods and detect change
    void SetMode(Mode newMode, System.Action behaviorMethod)
    {
        if (currentMode != newMode)
        {
            LogModeChange(newMode);
            currentMode = newMode;
        }

        behaviorMethod.Invoke();
    }

    void LogModeChange(Mode newMode)
    {
        Debug.Log($"{gameObject.name} switched to {newMode} mode.");
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - transform.position;
        agent.SetDestination(transform.position - fleeDirection);
    }

    void Pursue()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    void Wander()
    {
        float wanderRadius = 20;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void CleverHide()
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenObj = null;

        foreach (GameObject spot in World.Instance.GetHidingSpots())
        {
            Vector3 hideDir = spot.transform.position - target.transform.position;
            Vector3 hidePos = spot.transform.position + hideDir.normalized * 5;

            float dist = Vector3.Distance(transform.position, hidePos);
            if (dist < distance)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenObj = spot;
                distance = dist;
            }
        }

        if (chosenObj != null)
        {
            Collider hideCol = chosenObj.GetComponent<Collider>();
            Ray back = new Ray(chosenSpot, -chosenDir.normalized);
            RaycastHit hitInfo;

            if (hideCol.Raycast(back, out hitInfo, 100f))
            {
                Seek(hitInfo.point + chosenDir.normalized * 5);
            }
            else
            {
                Seek(chosenSpot);
            }
        }
    }

    bool canSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - transform.position;

        if (Physics.Raycast(transform.position, rayToTarget, out raycastInfo))
        {
            Debug.DrawRay(transform.position, rayToTarget, Color.red); // visualize in Scene view
            Debug.Log($"{name} Ray hit: {raycastInfo.transform.name}");

            return raycastInfo.transform.CompareTag("Player");
        }

        Debug.Log($"{name} Raycast hit nothing.");
        return false;
    }

}
