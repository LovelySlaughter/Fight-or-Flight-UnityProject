using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] private bool _musicSwitch, _effectsSwitch;
    // Start is called before the first frame update
    public void Toggle()
    {
        if (_effectsSwitch) { SoundManager.Instance.ToggleEffects(); }
        if (_musicSwitch) { SoundManager.Instance.ToggleMusic(); }
    }
}
