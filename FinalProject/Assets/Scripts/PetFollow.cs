using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PetFollow : MonoBehaviour
{
    public Transform target;
    public float followDistance = 2f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float groundOffset = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent physics from messing with rotation
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Desired position behind target
        Vector3 desiredPosition = target.position - target.forward * followDistance;

        // Raycast to find ground height
        RaycastHit hit;
        if (Physics.Raycast(desiredPosition + Vector3.up * 5f, Vector3.down, out hit, 20f))
        {
            desiredPosition.y = hit.point.y + groundOffset;
        }

        // Smooth follow
        Vector3 newPos = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Smooth rotation toward target
        Vector3 direction = target.position - transform.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
