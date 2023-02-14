using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.GunPickUp(gun);
            gameManager.instance.rbPlayerScript.GunPickUp(gun);
            Destroy(gameObject);
        }
    }
}
