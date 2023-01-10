using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Coded By Mauricio
public class flightChase : MonoBehaviour
{
    public flyingEnemyAI[] enemyArray;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (flyingEnemyAI enemy in enemyArray)
            {
                enemy.chase = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (flyingEnemyAI enemy in enemyArray)
            {
                enemy.chase = false;
            }
        }
    }
}
