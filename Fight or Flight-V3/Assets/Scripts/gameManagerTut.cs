using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
//Coded By Mauricio

public class gameManagerTut : MonoBehaviour
{
    public static gameManagerTut instance;
    [Header("---- Player ----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;


    [Header("---- Game Goal ----")]
    public int enemiesRemaining;
    public int enemiesKilled;

    [Header("---- UI ----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject playerDeadMenu;
    public Image playerHPBar;
    public GameObject screenFlash;
    [SerializeField] TextMeshProUGUI enemiesRemainingText;
    [SerializeField] TextMeshProUGUI enemiesKilledText;

    public bool isPaused;
    float timeScaleOrig;


    // Start is called before the first frame update
    void Awake() //( In the order of operations it is the first thing that happens before Start() )
                 //only use awake in the gameManager or in another manager
    {
        instance = this; //Making sure you have one instance if there is two it will turn off one instance
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawnPos");

        timeScaleOrig = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
            {
                pause();
            }
            else
            {
                unPause();
            }
        }
        else if (Input.GetButtonDown("Cancel") && activeMenu == pauseMenu)
        {
            unPause();
            isPaused = !isPaused;
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateEnemyRemaining(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0"); //"F0" makes it an int/how many decimals you want behind it

        //Check to see if game in over based on enemy count <= 0
        if (enemiesRemaining <= 0)
        {
            pause();
            SceneManager.LoadScene("BetweenLvl1andLvl2 ");
        }

    }

    public void UpdateEnemiesKilled(int number)
    {
        enemiesKilled += number;
        enemiesKilledText.text = enemiesKilled.ToString();
    }

    public void playerDead()
    {
        pause();
        activeMenu = playerDeadMenu;
        activeMenu.SetActive(true);
    }

    public IEnumerator flashDamage()
    {
        screenFlash.SetActive(true);
        yield return new WaitForSeconds(0.40f);
        screenFlash.SetActive(false);
    }
}
