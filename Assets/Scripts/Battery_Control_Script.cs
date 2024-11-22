using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery_Control_Script : MonoBehaviour
{
    // Current battery life of the player
    public int playerBatteryLife = 3;

    // Maximum battery life the player can have
    public int maxBatteryLife = 3;

    // Time interval in seconds for draining the battery
    private float batteryDrainInterval = 20f;

    // UI element to display the battery progress
    public Image batteryProgressUI;

    // Reference to the flashlight GameObject
    public GameObject flashlight;

    private void Start()
    {
        // Start the coroutine to periodically drain the battery
        StartCoroutine(DrainBattery());
    }

    private void Update()
    {
        // Check the player's battery life
        if (playerBatteryLife <= 0)
        {
            // Turn off the flashlight if the battery is empty
            flashlight.SetActive(false);
        }
        else if (playerBatteryLife > 0)
        {
            // Turn on the flashlight if the battery is not empty
            flashlight.SetActive(true);
        }
    }

    private IEnumerator DrainBattery()
    {
        // Continuously drain the battery while the player's battery life is greater than 0
        while (playerBatteryLife > 0)
        {
            // Wait for the specified drain interval
            yield return new WaitForSeconds(batteryDrainInterval);

            // Decrease the battery life
            playerBatteryLife--;

            // Update the UI to reflect the current battery level
            batteryProgressUI.fillAmount = (float)playerBatteryLife / (float)maxBatteryLife;
        }
    }
}