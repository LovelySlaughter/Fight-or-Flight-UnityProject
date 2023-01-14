using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicWeapon : MonoBehaviour
{
    [Header("--- Gun Components ---")]
    [SerializeField] GameObject bullets;

    [Header("--- Gun Stats ---")]
    public int rateOfFire;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;

    public int magazineSize;
    public int bulletsPerTap;
    public bool automatic;

    int bulletsRemaining;
    int bulletsFired;

    public int damage;
    public int shootRange;

    public float shootForce;
    public float upwardForce;


    // Bools
    bool isShooting;
    bool readyToShoot;
    bool reloading;

    // References
    [SerializeField] Camera cam;
    [SerializeField] Transform shootPOS;

    public bool allowInvoke = true;


    // Start is called before the first frame update
    void Awake()
    {
        // magazine must be full
        bulletsRemaining = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        // check if gun is automatic
        if (automatic)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
    }
}
