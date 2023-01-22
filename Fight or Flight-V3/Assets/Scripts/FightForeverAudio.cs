using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightForeverAudio : MonoBehaviour
{
    [SerializeField] AudioSource sounds;
    [SerializeField] AudioClip[] music;
    [Range(0, 1)][SerializeField] float musicVolume;

    // Start is called before the first frame update
    void Start()
    {

        sounds.PlayOneShot(music[Random.Range(0, music.Length - 1)], musicVolume);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
