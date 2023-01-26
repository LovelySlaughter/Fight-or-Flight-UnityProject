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

    public bool pilotControls;
    public bool invertY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseVertical = Input.GetAxis("Mouse Y") * Time.deltaTime * ySensitivity;

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

        xRotation = Mathf.Clamp(xRotation, verticalMin, verticalMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseHorizontal);
       
    }
}
