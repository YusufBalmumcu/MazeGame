using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Script : MonoBehaviour
{
    // Flag to track if the ghost is stopped
    public bool ghoststopped = false;

    // Reference to the player object
    public GameObject player;
    // Movement speed of the ghost
    public float speed = 0.4f;
    // Speed at which the ghost rotates
    public float rotationspeed = 1f;

    // Update is called once per frame to move the ghost
    void Update()
    {
        GhostMovement(); // Call the GhostMovement method
    }

    // Handles the ghost movement towards the player
    public void GhostMovement()
    {
        if (ghoststopped == false) // If the ghost is not stopped
        {
            // Calculate the direction vector from the ghost to the player
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Calculate the rotation to face the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationspeed); // Smoothly rotate the ghost to face the player

            // Move the ghost towards the player
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // Handles the collision detection with other objects (e.g., snowballs)
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "snowball") // If the ghost collides with a snowball
        {
            Destroy(other.gameObject); // Destroy the snowball
            GhostStop(); // Call the GhostStop method to stop the ghost
        }
    }

    // Stops the ghost's movement
    public void GhostStop()
    {
        ghoststopped = true; // Set the ghost stopped flag to true
        StartCoroutine(GhostStopBreak()); // Start a coroutine to unfreeze the ghost after a delay
    }

    // Coroutine to wait for 5 seconds and then allow the ghost to start moving again
    IEnumerator GhostStopBreak()
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds
        ghoststopped = false; // Allow the ghost to start moving again
    }
}
