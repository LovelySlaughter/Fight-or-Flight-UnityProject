using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

public class CreditsScript : MonoBehaviour
{
    //public GameObject credits;
    //public GameObject assetCredits;
    private void Awake()
    {
        StartCoroutine(CreditSwap());
    }
    public void SkipButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 8);
    }

    public IEnumerator CreditSwap()
    {
        yield return new WaitForSeconds(54f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 8);
        //credits.SetActive(false);
        //assetCredits.SetActive(true);
    }
}
