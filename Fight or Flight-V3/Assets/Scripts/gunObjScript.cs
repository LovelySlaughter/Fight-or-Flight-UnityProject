using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunObjScript : ScriptableObject
{
    public float Rate;
    public int Range;
    public int Damage;

    public GameObject gun;
    public AudioClip gunShots;

}
