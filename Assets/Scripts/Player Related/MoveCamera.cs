using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float sens;
    public Transform orientation;

    [Header("Camera Tile Related")]
    public Movement movement;
    public float tiltAmount;
    public float maxZTilt;

    float xRotation;
    float yRotation;
    float tiltApplied;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sens = PlayerPrefs.GetFloat("Sensitivity", 5f);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sens = PlayerPrefs.GetFloat("Sensitivity", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens * 50;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens * 50;
        
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if(movement.lr != 0)
        {
            tiltApplied += tiltAmount * movement.lr * -1;
            if(Mathf.Abs(tiltApplied) > maxZTilt) tiltApplied = maxZTilt * movement.lr * -1;
        }
        if(movement.lr ==  0)
        {
            if(tiltApplied < -tiltAmount) tiltApplied += tiltAmount;
            if(tiltApplied > tiltAmount)  tiltApplied -= tiltAmount;
            if(tiltApplied > -tiltAmount && tiltApplied < tiltAmount) tiltApplied = 0f;
        }
        
        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0 + tiltApplied);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);


    }
}
