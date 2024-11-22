using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina_Control_Script : MonoBehaviour
{
    public float playerStamina = 100f;
    [SerializeField]
    private float maxStamina = 100f;
    public bool staminaRegenerated = true;
    public bool playerIsSprinting = false;

    public float walkSpeed = 1.5f;
    public float sprintSpeed = 3f; 

    [SerializeField] private float staminaDrain = 0.5f;
    [SerializeField] private float staminaRegen = 0.5f;

    public Image staminaProgressUI;

    public AudioSource breathingSound;

    private Move_Player_Script movePlayerScript;

    private void Start()
    {
        movePlayerScript = GetComponent<Move_Player_Script>();

    }

    private void Update()
    {
        if (!playerIsSprinting)
        {
            RegenerateStamina();
        }
        UpdateStaminaBar();
    }

        private void RegenerateStamina()
    {
        if (playerStamina < maxStamina)
        {
            playerStamina += staminaRegen * Time.deltaTime;
            playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina);
        }
    }

    public void Sprint()
    {
        playerIsSprinting = true;
        playerStamina -= staminaDrain * Time.deltaTime;
        playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina);

        movePlayerScript.speed = sprintSpeed;

        if (playerStamina <= 0)
        {
            StopSprinting();
            breathingSound.Play();
        }
    }

    public bool CanSprint()
    {
        if(playerStamina >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

        public void StopSprinting()
    {
        playerIsSprinting = false;
        movePlayerScript.speed = walkSpeed;
    }


    private void UpdateStaminaBar()
    {
        if (staminaProgressUI != null)
        {
            staminaProgressUI.fillAmount = playerStamina / maxStamina;
        }
    }



}
