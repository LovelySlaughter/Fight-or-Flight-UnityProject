using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject options;
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void CloseSettings()
    {
        options.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenLoad()
    {

    }
    public void CloseLoad()
    {

    }

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
