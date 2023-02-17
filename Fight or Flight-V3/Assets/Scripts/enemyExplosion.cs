using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyExplosion : MonoBehaviour
{
    public int explosionDamage;
   

    [Header("---- Explosion Sounds ----")]
    [SerializeField] AudioSource explosionSource;
    [SerializeField] AudioClip explosionAudio;
    [Range(0, 1)] [SerializeField] float explosionAudioVolume;
    [SerializeField] float timer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(explosionDamage / 2);
            explosionSource.PlayOneShot(explosionAudio, explosionAudioVolume);
        }
        
        Destroy(gameObject, timer);
    }
}
