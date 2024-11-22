using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Script : MonoBehaviour
{
    // References to different item game objects in the scene
    public GameObject key;
    public GameObject snowball1;
    public GameObject snowball2;
    public GameObject snowball3;
    public GameObject battery1;
    public GameObject battery2;
    public GameObject battery3;

    void Start()
    {
        // No initialization is needed here, as all items are being manipulated in the Update method
    }

    void Update()
    {
        // Rotate the key object around the Z-axis to create a spinning effect
        key.transform.Rotate(0, 0, -0.8f);
        
        // Rotate all snowball objects around the Y-axis to create a spinning effect
        snowball1.transform.Rotate(0, -0.5f, 0);
        snowball2.transform.Rotate(0, -0.5f, 0);
        snowball3.transform.Rotate(0, -0.5f, 0);
        
        // Rotate all battery objects around the Y-axis to create a spinning effect
        battery1.transform.Rotate(0, -0.5f, 0);
        battery2.transform.Rotate(0, -0.5f, 0);
        battery3.transform.Rotate(0, -0.5f, 0);
    }
}
