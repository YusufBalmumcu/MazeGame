using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery_Control_Script : MonoBehaviour
{
    public int playerBatteryLife = 3;
    private int maxBatteryLife = 3;
    private float batteryDrainInterval = 30f;

    public Image batteryProgressUI;

    public GameObject flashlight;

    private void Start()
    {
        StartCoroutine(DrainBattery());
    }

    private void Update()
    {
        if (playerBatteryLife <= 0)
        {
            flashlight.SetActive(false);
        }
        else if (playerBatteryLife > 0)
        {
            flashlight.SetActive(true);
        }
    }

    private IEnumerator DrainBattery()
    {
        while (playerBatteryLife > 0)
        {
            yield return new WaitForSeconds(batteryDrainInterval);
            playerBatteryLife--;
            batteryProgressUI.fillAmount = (float)playerBatteryLife / (float)maxBatteryLife;
        }
    }
}
