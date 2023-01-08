using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    [Header("--- Camera Stats ---")]
    [SerializeField] int ySensitivity;
    [SerializeField] int xSensitivity;
    float xRotation;
    float yRotation;

    [Header("--- Clamp Stats ---")]
    [SerializeField] int verticalMax;
    [SerializeField] int verticalMin;

    bool pilotControls;
    bool invertY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseHorizontal = Input.GetAxis("Horizantal") * Time.deltaTime * xSensitivity;
        float mouseVertical = Input.GetAxis("Vertical") * Time.deltaTime * ySensitivity;

        if (pilotControls)
        {
            xRotation += mouseVertical;
        }
        else
        {
            xRotation -= mouseVertical;
        }
        if (invertY)
        {
            yRotation += mouseHorizontal;
        }
        else
        {
            yRotation -= mouseHorizontal;
        }


       
    }
}
