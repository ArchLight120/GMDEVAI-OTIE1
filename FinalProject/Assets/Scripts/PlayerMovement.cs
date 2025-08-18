using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{   
    public float gravity = -9.81f;      // gravity force
    public float jumpHeight = 1.5f;     // optional jump

    public Camera playerCamera;

    float verticalRotation = 0f;
    CharacterController controller;

    Vector3 velocity; // stores current vertical velocity

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Make sure this is the Player
        gameObject.tag = "Player";
    }

    void Update()
    {
        // --- Gravity ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // keep grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- Jump (optional, press Space) ---
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
