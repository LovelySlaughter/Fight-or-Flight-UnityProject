using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostTutorialMeniScript : MonoBehaviour
{
    public void GoToLevelOne()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SaveGame()
    {

    }

    public void QuitGame()
    {

    }
    
}
