using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting };

    [System.Serializable]
    public class Waves
    {
        public string names;
        public Transform enemy;
        public int spawnCount;
        public float swawnRate;

    }

    public Waves[] waves;
    public float timeBetweenWaves = 5f;
    public float waveCountDown;
    private int nextWaveNum = 0;

    public SpawnState states = SpawnState.Counting;

    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCountDown <= 0)
        {
            if(states != SpawnState.Spawning) { }
            else { }
        }
    }


}
