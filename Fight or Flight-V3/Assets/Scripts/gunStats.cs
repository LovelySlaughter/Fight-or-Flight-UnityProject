using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    [Header("--- Gun Stats ---")]
    public float Rate;
    public int Range;
    public int Damage;
    public GameObject weaponModel;
    public AudioClip gunShots;
}
