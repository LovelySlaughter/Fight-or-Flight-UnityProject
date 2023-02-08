using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudio : MonoBehaviour
{

    public AudioSource impactAudio;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 1)
        {
            impactAudio.Play();
        }
    }
}
