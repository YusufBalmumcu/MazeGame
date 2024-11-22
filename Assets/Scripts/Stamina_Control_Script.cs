using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina_Control_Script : MonoBehaviour
{
    // Player's current stamina and maximum stamina values
    public float playerStamina = 100f;
    [SerializeField]
    private float maxStamina = 100f;
    public bool staminaRegenerated = true; // Whether stamina is regenerating
    public bool playerIsSprinting = false; // Whether the player is sprinting

    // Player movement speeds for walking and sprinting
    public float walkSpeed = 1.5f;
    public float sprintSpeed = 3f; 

    // Stamina consumption and regeneration rates
    [SerializeField] private float staminaDrain = 0.5f; 
    [SerializeField] private float staminaRegen = 0.5f;

    // UI element for showing stamina progress
    public Image staminaProgressUI;

    // Sound played when stamina runs out and breathing intensifies
    public AudioSource breathingSound;

    // Reference to the Move_Player_Script for controlling player speed
    private Move_Player_Script movePlayerScript;

    void Start()
    {
        // Initialize the reference to Move_Player_Script
        movePlayerScript = GetComponent<Move_Player_Script>();
    }

    void Update()
    {
        // If the player is not sprinting, regenerate stamina
        if (!playerIsSprinting)
        {
            RegenerateStamina();
        }

        // Update the stamina progress bar UI
        UpdateStaminaBar();
    }

    // Regenerates stamina if it is not at maximum
    private void RegenerateStamina()
    {
        if (playerStamina < maxStamina)
        {
            playerStamina += staminaRegen * Time.deltaTime; // Regenerate stamina over time
            playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina); // Ensure stamina stays within valid bounds
        }
    }

    // Starts sprinting by draining stamina and increasing speed
    public void Sprint()
    {
        playerIsSprinting = true; // Mark the player as sprinting
        playerStamina -= staminaDrain * Time.deltaTime; // Drain stamina over time
        playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina); // Ensure stamina stays within valid bounds

        movePlayerScript.speed = sprintSpeed; // Increase movement speed for sprinting

        if (playerStamina <= 0)
        {
            // Stop sprinting if stamina is depleted
            StopSprinting();
            breathingSound.Play(); // Play a breathing sound when stamina runs out
        }
    }

    // Checks if the player has enough stamina to sprint
    public bool CanSprint()
    {
        return playerStamina > 0; // Return true if stamina is available, false otherwise
    }

    // Stops sprinting by restoring the player’s speed to normal walking speed
    public void StopSprinting()
    {
        playerIsSprinting = false; // Mark the player as not sprinting
        movePlayerScript.speed = walkSpeed; // Restore the walking speed
    }

    // Updates the stamina bar UI to reflect current stamina
    private void UpdateStaminaBar()
    {
        if (staminaProgressUI != null)
        {
            staminaProgressUI.fillAmount = playerStamina / maxStamina; // Update the stamina UI as a percentage
        }
    }
}
