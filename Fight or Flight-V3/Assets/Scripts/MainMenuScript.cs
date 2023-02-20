using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using JetBrains.Annotations;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject loadGame;
    public GameObject transitionVid;
    public GameObject MainMenuVid;
    public AudioMixer mixer;
    public GameObject musicStuff;
    public void NewGame()
    {
        mainMenu.SetActive(false);
        MainMenuVid.SetActive(false);
        musicStuff.SetActive(false);
        transitionVid.SetActive(true);
        Invoke("waitingOnTheWorldToChange", 87);
        
        Time.timeScale = 1;
    }

    public void waitingOnTheWorldToChange() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
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
        SceneManager.LoadScene("FightForever");

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

    public void CreditsButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 8);
    }

    public void FullScreenToggle()
    {
        Screen.fullScreen= !Screen.fullScreen;
    }
}
