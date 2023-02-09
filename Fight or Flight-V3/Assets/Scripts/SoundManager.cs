using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _music, _effects;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        _effects.PlayOneShot(audioClip);
    }

    public void ChangeMasterAudio(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ToggleEffects()
    {
        _effects.mute = !_effects.mute;
    }

    public void ToggleMusic()
    {
        _music.mute = !_music.mute;
    }
}
