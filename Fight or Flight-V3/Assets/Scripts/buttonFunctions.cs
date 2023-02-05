using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Coded By Mauricio
public class buttonFunctions : MonoBehaviour
{

    public void resume()
    {
        gameManager.instance.unPause();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void restart()
    {
        //not the best way to restart the scene but just did it in this class
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit(); //won't work in the editor
    }

    public void playerRespawn()
    {
        gameManager.instance.playerScript.respawnPlayer();
        gameManager.instance.unPause();
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;

    }
}
