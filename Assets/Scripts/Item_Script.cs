using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Script : MonoBehaviour
{

    

    public GameObject key;
    public GameObject snowball;

    void Start()
    {
        
    }

    void Update()
    {
        key.transform.Rotate(0,0,-0.8f);
        snowball.transform.Rotate(0, -0.5f, 0);
    }
}
