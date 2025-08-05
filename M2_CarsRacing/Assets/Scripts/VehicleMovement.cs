using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public enum DrivingStyle { Cautious, Normal, Daredevil }
    public DrivingStyle drivingStyle = DrivingStyle.Normal;

    public Transform goal;

    public float speed = 0;
    public float rotSpeed = 1;
    public float acceleration;
    public float deceleration;
    public float minSpeed = 0;
    public float maxSpeed;
    public float breakAngle;

    void Start()
    {
        SetDrivingStyle(); // Apply behavior based on selected style
    }

    void LateUpdate()
    {
        Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
        Vector3 direction = lookAtGoal - transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              Quaternion.LookRotation(direction),
                                              Time.deltaTime * rotSpeed);

        if (Vector3.Angle(goal.forward, transform.forward) > breakAngle && speed > 2)
        {
            speed = Mathf.Clamp(speed - (deceleration * Time.deltaTime), minSpeed, maxSpeed);
        }
        else
        {
            speed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), minSpeed, maxSpeed);
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void SetDrivingStyle()
    {
        switch (drivingStyle)
        {
            case DrivingStyle.Cautious:
                acceleration = 2f;
                deceleration = 6f;
                maxSpeed = 6f;
                breakAngle = 10f;
                break;

            case DrivingStyle.Normal:
                acceleration = 4f;
                deceleration = 4f;
                maxSpeed = 10f;
                breakAngle = 20f;
                break;

            case DrivingStyle.Daredevil:
                acceleration = 6f;
                deceleration = 2f;
                maxSpeed = 15f;
                breakAngle = 30f;
                break;
        }
    }
}