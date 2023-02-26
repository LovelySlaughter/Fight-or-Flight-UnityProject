using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject levelScreen;

    private void OnTriggerEnter(Collider other)
    {
        levelScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        gameManager.instance.cameraStuff.CameraOn = false;
    }
    private void OnTriggerExit(Collider other)
    {
        levelScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        gameManager.instance.cameraStuff.CameraOn = true;
    }
}
