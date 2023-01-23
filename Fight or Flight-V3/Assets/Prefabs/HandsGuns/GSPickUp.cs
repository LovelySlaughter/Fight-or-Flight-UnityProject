using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSPickUp : MonoBehaviour
{

    [SerializeField] handsGuns gun;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.HandGunPickUp(gun);
            Destroy(gameObject);
        }
    }
}
