using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]

public class handsGuns : ScriptableObject
{
    [Header("--- Gun Stats ---")]
    public float Rate;
    public int Range;
    public int Damage;
    public GameObject weaponPrefab;
    public AudioClip gunShots;
    public Vector3 scale;
    [Range(0, 1)] public float gunShotsVolume;
    public Transform gunShotsTransform;
    public Animator animator;
    public AudioClip[] gunShotsAudio;
}
