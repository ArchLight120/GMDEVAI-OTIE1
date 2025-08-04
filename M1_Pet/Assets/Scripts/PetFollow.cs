using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public Transform target;
    public float followDistance = 2f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // Desired position behind the target
        Vector3 desiredPosition = target.position - target.forward * followDistance;

        // Smooth position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);

        // Rotate toward the target smoothly using Slerp
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}