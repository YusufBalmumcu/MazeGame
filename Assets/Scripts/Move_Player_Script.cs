using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move_Player_Script : MonoBehaviour
{

    public Stamina_Control_Script staminaControlScript;


    public float speed = 1.5f; 

    private float acceleration = 10f; // How quickly the player reaches target speed
    private float deceleration = 10f; // How quickly the player stops


    Rigidbody rb;
    Vector3 movedirection;
    public Vector3 currentVelocity;
    public Transform orientation;

    public LayerMask groundMask;
    public float playerHeight;
    public float groundDrag;
    bool grounded;

    void Start()
    {
        staminaControlScript = GetComponent<Stamina_Control_Script>();

        // Hide the cursor
        Cursor.visible = false; 
        
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

        public void OnApplicationFocus()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Make the cursor visible
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
        OnApplicationFocus();
        CheckGround();
        ControlSpeed();
        HandleSprintInput();
        ApplyGroundDrag();

        
    }

        private void FixedUpdate()
    {
        Player_Movement();
    }



    private void ApplyGroundDrag()
    {
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    } 

    private void CheckGround()
    {
        grounded =  Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (grounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    public void Player_Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool playerIsWalking = !Input.GetKey(KeyCode.LeftShift);
        if (playerIsWalking)
        {
            staminaControlScript.playerIsSprinting = false;
        }
        else if(!playerIsWalking && currentVelocity.sqrMagnitude > 0)
        {
            if (staminaControlScript.playerStamina > 0)
            {
                staminaControlScript.playerIsSprinting = true;
                staminaControlScript.Sprint();
            }
            else
            {
                playerIsWalking = true;
            }

        }

        // Calculate movement direction
        movedirection = orientation.forward * vertical + orientation.right * horizontal;

        if (movedirection != Vector3.zero)
        {
            // Accelerate toward the desired velocity
            currentVelocity = Vector3.MoveTowards(currentVelocity, movedirection * speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate to a stop
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // Apply the velocity to the rigidbody
        rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
    }

    private void ControlSpeed()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //Limits the speed of the player to the desired speed
        if (flatvel.magnitude > speed)
        {
            Vector3 limitedvel = flatvel.normalized * speed;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }

    private void HandleSprintInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && staminaControlScript.CanSprint())
        {
            // Sprinting
            staminaControlScript.Sprint();
        }
        else
        {
            // Walking
            staminaControlScript.StopSprinting();
        }
    }
}
