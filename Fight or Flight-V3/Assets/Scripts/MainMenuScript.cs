using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject loadGame;
    public AudioMixer mixer;
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

    public void FightForever()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);

    }

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void SetVolume(float Volume)
    {
        mixer.SetFloat("MasterVolume", Volume);
    }
}
