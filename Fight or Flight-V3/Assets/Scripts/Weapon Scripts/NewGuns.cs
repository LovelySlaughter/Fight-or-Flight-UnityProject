using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewGuns : MonoBehaviour
{
    public Camera cameraMC;
    public Transform attackPos;
    public RaycastHit rayhit;
    public LayerMask whatIsEnemy;
    public int damage, magazineSize, bulletsPerPress;
    public string nameID;
    public string description;
    public float spread, range, timeBetweenShoots, timeBetweenShooting, reloadTime;
    public bool automatic;
    int bulletsInMag, bulletsShot;
    bool isShooting, readyToShoot, isReloading;
    public GameObject muzzzleFlash, bulletHole;


    private void Awake()
    {
        bulletsInMag = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        PlayerInput();
    }


    private void PlayerInput()
    {
        if (automatic) { isShooting = Input.GetKey(KeyCode.Mouse0); }
        else if (!automatic) { isShooting = Input.GetKeyDown(KeyCode.Mouse0); }

        if (Input.GetKeyDown(KeyCode.R) && bulletsInMag < magazineSize && !isReloading) { Reload(); }

        if (readyToShoot && isShooting && !isReloading && bulletsInMag > 0)
        {
                Shoot();
        }

    }

    private void Reload()
    {
        isReloading = true;
        Invoke("FinishedReload", reloadTime);
    }

    private void FinishedReload()
    {
        bulletsInMag = magazineSize;
        isReloading = false;
    }

    public void Shoot()
    {
        readyToShoot = false;

        // spread variables
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        //Direction with Spread
        Vector3 spreadDirection = cameraMC.transform.forward + new Vector3(spreadX, spreadY, 0);

        // Raycast
        if (Physics.Raycast(cameraMC.transform.position, spreadDirection, out rayhit, range, whatIsEnemy))
        {
            if (rayhit.collider.CompareTag("Enemy"))
            {
                rayhit.collider.GetComponent<enemyAI>().takeDamage(damage);
            }
        }

        // Flash and pullet Marks
        Instantiate(muzzzleFlash, attackPos.position, Quaternion.identity);
        Instantiate(bulletHole, rayhit.point, Quaternion.Euler(0, 180, 0));
       

        bulletsInMag--;
        bulletsShot++;
        Invoke("ResetShoot", timeBetweenShooting);


    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }
}
