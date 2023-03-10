using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
//Coded By Mauricio

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("---- Player ----")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public cameraControls cameraStuff;

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
    public AudioSource talkingAudio;
    public WaveSpawner waveSpawner;

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
        Debug.Log(Time.timeScale);
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
        Debug.Log("TimeScale is 0");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        talkingAudio.Pause();
    }

    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Debug.Log("TimeScale is not 0");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
        talkingAudio.UnPause();
    }

    public void updateEnemyRemaining(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString("F0"); //"F0" makes it an int/how many decimals you want behind it

        //Check to see if game in over based on enemy count <= 0
        if (waveSpawner == null && enemiesRemaining == 0)
        {
            pause();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
        else if (waveSpawner.gameWon == true)
        {
            pause();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
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
