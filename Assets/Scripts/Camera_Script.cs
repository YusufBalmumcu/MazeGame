using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera_Script : MonoBehaviour
{
    // Camera bobbing settings
    public float cameraBobbingHeight = 0.5f;
    public float cameraBobbingFrequency = 6f;
    public float cameraBobbingSpeed = 0.1f;

    private Vector3 originalCameraPosition; // Stores the initial position of the camera
    private float timer; // Timer to track time for bobbing effect

    // Camera rotation settings
    public float xRotation; 
    public float yRotation; 
    public float sensitivity = 150f; // Mouse sensitivity for camera rotation

    public Transform orientation; // Reference for player orientation

    private Move_Player_Script movePlayerScript; // Reference to the player's movement script

    void Start()
    {
        originalCameraPosition = transform.localPosition; // Save initial camera position
        movePlayerScript = GetComponentInParent<Move_Player_Script>(); // Get movement script from the parent
    }

    void Update()
    {
        Player_Camera(); // Handle player camera rotation based on mouse input
        CameraBobbing(); // Apply bobbing effect while moving
    }

    // Function to handle camera rotation with mouse input
    public void Player_Camera()
    {
        // Get mouse movement for X and Y axis
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Update the rotation values
        yRotation += mouseX; // Horizontal rotation
        xRotation -= mouseY; // Vertical rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation to avoid flipping

        // Apply rotation to camera and player orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f); 
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f); 
    }

    // Function for the camera bobbing effect when the player moves
    private void CameraBobbing()
    {
        // Check if player is moving
        if (movePlayerScript.currentVelocity.magnitude > cameraBobbingSpeed)
        {
            // Increase timer based on movement speed
            timer += Time.deltaTime * cameraBobbingFrequency;

            // Calculate new Y position for the bobbing effect
            float newY = originalCameraPosition.y + Mathf.Sin(timer) * cameraBobbingHeight;

            // Apply new camera position with bobbing
            transform.localPosition = new Vector3(originalCameraPosition.x, newY, originalCameraPosition.z);
        }
        else
        {
            // Reset timer and return to original position when not moving
            timer = 0;
            transform.localPosition = originalCameraPosition;
        }
    }
}
