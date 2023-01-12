using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Coded By Mauricio

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public GameObject player;
    public playerController playerScript;
    public GameObject enemy;
    public enemyAI enemyScript;
    public int enemiesRemaining;

    // Start is called before the first frame update
    void Awake() //( In the order of operations it is the first thing that happens before Start() )
                 //only use awake in the gameManager or in another manager
    {
        instance = this; //Making sure you have one instance if there is two it will turn off one instance
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<enemyAI>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void updateEnemyRemaining(int amount)
    {
        enemiesRemaining += amount;

        //Check to see if game in over based on enemy count <= 0
        if (enemiesRemaining <= 0)
        {
            Debug.Log("You win!!!");
        }
    }
}
