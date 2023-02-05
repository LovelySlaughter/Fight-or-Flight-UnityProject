using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlaySound(_audioClip);
    }

   
}
