using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    [Header("--- Gun Stats ---")]
    public float Rate;
    public int Range;
    public int Damage;
    public GameObject weaponModel;
    public AudioClip gunShots;
    public Vector3 scale;
    [Range(0, 1)] public float gunShotsVolume;
    public GameObject muzzleFlash;
    public GameObject bulletHoles;
}
