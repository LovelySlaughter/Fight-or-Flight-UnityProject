using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting };

    [System.Serializable]
    public class Waves
    {
        public string names;
        public GameObject enemy;
        public int spawnCount;
        public float swawnRate;

    }

    public Waves[] waves;
    public float timeBetweenWaves = 5f;
    public float waveCountDown;
    private int nextWaveNum = 0;
    private float searchTimer = 1f;
    public bool infinteSpawn = false;
    public bool gameWon = false;


    public SpawnState states = SpawnState.Counting;

    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (states == SpawnState.Waiting)
        {
            if (!enemiesAlive())
            {
                // Insert Wave Completed stuff

                //Begin new wave
                WaveComplete();

            }
            else
            {
                return;
            }
        }
        if (waveCountDown <= 0)
        {
            if (states != SpawnState.Spawning) { StartCoroutine(SpawnWave(waves[nextWaveNum])); }
            else { waveCountDown -= Time.deltaTime; }
        }
    }

    bool enemiesAlive()
    {
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0f)
        {
            searchTimer = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy") == null) { return false; }
        }
        return true;
    }

    void WaveComplete()
    {
        Debug.Log("Wave Complete");

        states = SpawnState.Counting;
        waveCountDown = timeBetweenWaves;

        if (nextWaveNum + 1 > waves.Length - 1)
        {
            nextWaveNum = 0;
            Debug.Log("All waves complete");
            //Insert level finished or infinite here
            if (!infinteSpawn) { gameWon = true; }
            else
            {
                nextWaveNum = 0;
            }
        }
        else
        {
            nextWaveNum++;
        }

    }

    IEnumerator SpawnWave(Waves _waves)
    {
        states = SpawnState.Spawning;
        Debug.Log("Spawning Wave");
        for (int i = 0; i < _waves.spawnCount; i++)
        {
            SpawnEnemies(_waves.enemy);
            yield return new WaitForSeconds(1f / _waves.swawnRate);
        }

        // Enemy Spawn

        states = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemies(GameObject _enemy)
    {
        Instantiate(_enemy, transform.position, transform.rotation);
        Debug.Log("Spawning Enemy" + _enemy.name);
    }
}
