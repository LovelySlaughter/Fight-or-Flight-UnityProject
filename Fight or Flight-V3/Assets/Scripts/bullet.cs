using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Coded By Mauricio

public class bullet : MonoBehaviour
{
    public int bulletDamage;
    [SerializeField] int timer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
    //    if (other.CompareTag("Enemy"))
    //    {
    //        gameManager.instance.enemyScript.takeDamage(bulletDamage);
    //    }

        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(bulletDamage);
        }
        //else if(other.CompareTag("Untagged"))
        //{
        //    //this is so it doesnt get confused when hitting untagged things
        //}
        Destroy(gameObject);
    }
}
