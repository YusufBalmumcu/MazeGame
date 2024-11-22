using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera_Script : MonoBehaviour
{

    public float cameraBobbingHeight = 0.5f;
    public float cameraBobbingFrequency = 6f;
    public float cameraBobbingSpeed = 0.1f;

    private Vector3 originalCameraPosition;
    private float timer;


    public float xRotation;
    public float yRotation;
    public float sensitivity = 1000f;

    public Transform orientation;

    private Move_Player_Script movePlayerScript;

    void Start()
    {
        originalCameraPosition = transform.localPosition;

        movePlayerScript = GetComponentInParent<Move_Player_Script>();
    }


    void Update()
    {
        Player_Camera();
        CameraBobbing();
    }


    public void Player_Camera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }


    private void CameraBobbing()
    {

        if (movePlayerScript.currentVelocity.magnitude > cameraBobbingSpeed)
        {
            timer += Time.deltaTime * cameraBobbingFrequency;
        
            float newY = originalCameraPosition.y + Mathf.Sin(timer) * cameraBobbingHeight;

            transform.localPosition = new Vector3(originalCameraPosition.x, newY, originalCameraPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = originalCameraPosition;
        }
    
    
    }

    
}
