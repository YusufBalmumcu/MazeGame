using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move_Player_Script : MonoBehaviour
{

    public float speed = 3f;

    
        
    Rigidbody rb;

    Vector3 movedirection;
    public Transform orientation;

    public float groundDrag;
    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;



    void Start()
    {
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    private void Update(){
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        
        ControlSpeed();
        
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    
    private void FixedUpdate()
    {
        Player_Movement();
    }

        public void Player_Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movedirection = orientation.forward * vertical + orientation.right * horizontal;

        rb.AddForce(movedirection.normalized * speed, ForceMode.Force);

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
}
