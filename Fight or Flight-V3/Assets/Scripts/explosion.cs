using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int pushBackAmount;

    [SerializeField] bool pull;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!pull)
            {
                gameManager.instance.playerScript.pushBack = (gameManager.instance.player.transform.position - transform.position).normalized * pushBackAmount;
            }
            else
            {
                gameManager.instance.playerScript.pushBack = (transform.position - gameManager.instance.player.transform.position).normalized * pushBackAmount;
            }
        }
    }

}
