using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//coded by Miguel
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] int timer;
    [SerializeField] Transform spawnPOS;
    [SerializeField] bool infiniteSpawn = false;


    bool isSpawning;
    bool playerInRange;
    int enemiesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.updateEnemyRemaining(enemiesToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (infiniteSpawn && !isSpawning && gameManager.instance.enemiesRemaining <= 1)
        {
            StartCoroutine(spawn());
        }
        if (playerInRange && !isSpawning && enemiesSpawned < enemiesToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player found");
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        Instantiate(enemy, new Vector3(spawnPOS.position.x + Random.Range(-6, 6), 0, spawnPOS.position.z + Random.Range(-6, 6)), enemy.transform.rotation);
        if (!infiniteSpawn)
        {
            enemiesSpawned++;
        }
        else
        {
            timer = 3;
            enemiesSpawned = 0;
        }
        yield return new WaitForSeconds(timer);

        isSpawning = false;
    }
}
