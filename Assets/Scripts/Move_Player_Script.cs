using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move_Player_Script : MonoBehaviour
{
    // Reference to stamina control script for managing stamina while sprinting
    public Stamina_Control_Script staminaControlScript;

    public float speed = 1.5f; // Base movement speed

    // Variables to control acceleration and deceleration
    private float acceleration = 10f; // How quickly the player reaches target speed
    private float deceleration = 10f; // How quickly the player stops

    // Rigidbody and movement direction variables
    Rigidbody rb;
    Vector3 movedirection;
    public Vector3 currentVelocity;
    public Transform orientation;

    // Ground detection variables
    public LayerMask groundMask;
    public float playerHeight;
    public float groundDrag;
    bool grounded;

    void Start()
    {
        // Initialize stamina control script reference
        staminaControlScript = GetComponent<Stamina_Control_Script>();

        // Hide the cursor on start
        Cursor.visible = false; 
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        
        Time.timeScale = 1f;

        // Get the Rigidbody component and disable its rotation to prevent player from spinning
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Handle input when the application gains or loses focus
    public void OnApplicationFocus()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Make the cursor visible if Escape is pressed
            Cursor.visible = true; 
        }
        // Press Left Mouse Button to hide the cursor again
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        // Check for application focus and update necessary values
        OnApplicationFocus();

        // Update ground check, speed control, sprint handling, and apply ground drag
        CheckGround();
        ControlSpeed();
        HandleSprintInput();
        ApplyGroundDrag();
    }

    // Handle movement logic in FixedUpdate for better physics handling
    private void FixedUpdate()
    {
        Player_Movement();
    }

    // Apply drag when the player is grounded
    private void ApplyGroundDrag()
    {
        if (grounded)
        {
            rb.drag = groundDrag; // Apply drag if grounded
        }
        else
        {
            rb.drag = 0; // No drag when in the air
        }
    }

    // Check if the player is on the ground using a raycast
    private void CheckGround()
    {
        grounded =  Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (grounded && rb.velocity.y < 0)
        {
            // Stop downward velocity when grounded
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    // Handle player movement based on input
    public void Player_Movement()
    {
        // Get input for horizontal and vertical movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Determine whether the player is walking or sprinting
        bool playerIsWalking = !Input.GetKey(KeyCode.LeftShift);
        if (playerIsWalking)
        {
            staminaControlScript.playerIsSprinting = false;
        }
        else if(!playerIsWalking && currentVelocity.sqrMagnitude > 0)
        {
            // Sprinting logic
            if (staminaControlScript.playerStamina > 0)
            {
                staminaControlScript.playerIsSprinting = true;
                staminaControlScript.Sprint();
            }
            else
            {
                // Stop sprinting if stamina is depleted
                playerIsWalking = true;
            }
        }

        // Calculate movement direction based on player input and camera orientation
        movedirection = orientation.forward * vertical + orientation.right * horizontal;

        if (movedirection != Vector3.zero)
        {
            // Accelerate toward the desired velocity
            currentVelocity = Vector3.MoveTowards(currentVelocity, movedirection * speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate to a stop if no input is given
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // Apply the calculated velocity to the Rigidbody (horizontal movement)
        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }

    // Limit player speed to a defined maximum speed
    private void ControlSpeed()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
        // If the player exceeds the max speed, reduce their velocity
        if (flatvel.magnitude > speed)
        {
            Vector3 limitedvel = flatvel.normalized * speed;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }

    // Handle sprint input logic
    private void HandleSprintInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && staminaControlScript.CanSprint())
        {
            // Start sprinting if LeftShift is held and the player has enough stamina
            staminaControlScript.Sprint();
        }
        else
        {
            // Stop sprinting if LeftShift is released or stamina is insufficient
            staminaControlScript.StopSprinting();
        }
    }
}
