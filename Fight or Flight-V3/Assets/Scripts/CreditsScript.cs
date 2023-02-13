using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CreditsScript : MonoBehaviour
{
    public GameObject credits;
    public GameObject assetCredits;
    private void Start()
    {
        StartCoroutine(CreditSwap());
    }
    public void SkipButton()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
            SceneManager.LoadScene("MainMenuScene");
        }
	}

    public IEnumerator CreditSwap()
    {
        yield return new WaitForSeconds(54f);
        credits.SetActive(false);
        assetCredits.SetActive(true);
    }
}
