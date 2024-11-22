using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Xml.Serialization;

public class Collision_Script : MonoBehaviour
{
    // Audio sources for various sounds in the game
    public AudioSource gamemusic;
    public AudioSource keyaudio;
    public AudioSource needkeyaudio;
    public GameObject NeedKeyText;

    // Flag to track if the player has collected the key
    public bool keycollected = false;

    // Variables for snowball management
    public int currentSnowballs = 0;
    public GameObject snowballPrefab;
    public float throwForce = 5f;

    // UI text to show the current number of snowballs
    public TextMeshProUGUI snowballText;

    // Reference to the battery control script for managing battery life
    public Battery_Control_Script batteryControlScript;

    // Initialize script and update snowball text UI at the start
    void Start()
    {
        batteryControlScript = GetComponent<Battery_Control_Script>();
        UpdateSnowballText();
    }

    // Update is called once per frame to check for user input
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) // Check if player pressed space to throw a snowball
        {
            ThrowSnowball(); // Throw the snowball
        }
    }

    // Handles throwing the snowball when the space key is pressed
    void ThrowSnowball()
    {
        if (currentSnowballs >= 1) // Check if the player has any snowballs to throw
        {
            Transform cameraTransform = Camera.main.transform; // Get the camera's position and forward direction
            GameObject snowball = Instantiate(snowballPrefab, cameraTransform.position + cameraTransform.forward, Quaternion.identity); // Instantiate snowball in front of the camera

            Rigidbody x = snowball.GetComponent<Rigidbody>(); // Get the snowball's Rigidbody component
            if (x != null)
            {
                x.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse); // Apply force to throw the snowball
            }
            currentSnowballs--; // Decrease snowball count
            UpdateSnowballText(); // Update the UI text for snowballs
        }
    }

    // Handle collisions with various objects in the game
    public void OnTriggerEnter(Collider other)
    {
        // If the player collides with the key, collect it
        if(other.gameObject.tag == "key")
        {
            Destroy(other.gameObject); // Remove the key from the game world
            keycollected = true; // Set key as collected
            keyaudio.Play(); // Play key collection sound
        }

        // If the player collides with the door and has the key, open the door and trigger the win scene
        if(other.gameObject.tag == "door" && keycollected == true)
        {
            other.gameObject.transform.rotation = Quaternion.Euler(0f, -90f, 0f); // Rotate the door to open
            Invoke("WinScene", 1f); // Trigger the win scene after a delay
        }

        // If the player collides with the door and does not have the key, show a message
        if (other.gameObject.tag == "door" && keycollected == false)
        {
            NeedKeyText.SetActive(true); // Show the "need key" text
            needkeyaudio.Play(); // Play the "need key" audio
            StartCoroutine(NeedKeyTextBreak()); // Hide the "need key" text after a delay
        }

        // If the player collides with a snowball, collect it
        if (other.gameObject.tag == "snowball")
        {
            Destroy(other.gameObject); // Remove the snowball from the game world
            currentSnowballs++; // Increase the snowball count
            UpdateSnowballText(); // Update the snowball UI
        }

        // If the player collides with a ghost, trigger the game over scene
        if (other.gameObject.tag == "ghost")
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible
            SceneManager.LoadScene(3); // Load the game over scene
        }

        // If the player collides with a battery, collect it
        if (other.gameObject.tag == "battery")
        {
            Debug.Log("Battery Collected"); // Log the battery collection
            Destroy(other.gameObject); // Remove the battery from the game world
            batteryControlScript.playerBatteryLife++; // Increase battery life
            batteryControlScript.batteryProgressUI.fillAmount = (float)batteryControlScript.playerBatteryLife / (float)batteryControlScript.maxBatteryLife; // Update battery UI
        }
    }

    // Load the win scene
    public void WinScene()
    {
        SceneManager.LoadScene(2); // Load the win scene
    }

    // Coroutine to hide the "Need Key" text after a delay
    IEnumerator NeedKeyTextBreak()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        NeedKeyText.SetActive(false); // Hide the "Need Key" text
    }

    // Update the UI text showing the number of snowballs the player has
    void UpdateSnowballText()
    {
        snowballText.text = "Snowballs: " + currentSnowballs; // Update the snowball count text
    }
}
