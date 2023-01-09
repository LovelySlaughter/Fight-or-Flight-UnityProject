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
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
