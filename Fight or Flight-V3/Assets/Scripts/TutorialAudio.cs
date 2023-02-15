using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudio : MonoBehaviour
{
    public AudioSource TutorialPlayer;
    public AudioClip tutorialMessage;

    private void OnTriggerEnter(Collider other)
    {
        TutorialPlayer.PlayOneShot(tutorialMessage);
        gameObject.SetActive(false);
    }
}
