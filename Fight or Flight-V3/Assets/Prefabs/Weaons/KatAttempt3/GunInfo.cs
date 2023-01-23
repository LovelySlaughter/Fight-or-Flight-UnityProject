using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]

public class GunInfo : ScriptableObject
{
    [Header("--- Gun Info ---")]
    public new string name;

    [Header("--- Shooting ---")]
    public float damage;
    public float maxDistance;

    [Header("--- Reloading ---")]
    public int ammo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public bool reloading;
}
