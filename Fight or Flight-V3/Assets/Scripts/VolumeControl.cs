using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.ChangeMasterAudio(_slider.value);
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterAudio(val));
    }
}
