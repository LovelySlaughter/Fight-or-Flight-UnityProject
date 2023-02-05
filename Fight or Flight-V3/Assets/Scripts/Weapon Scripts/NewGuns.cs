using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGuns : MonoBehaviour
{
    public Camera camera;
    public Transform shootPos;
    public RaycastHit raycast;
    public LayerMask _enemies;
    public int damage, magazineSize, bulletsPerPress;
    public float timeBetweenShots, range, reloadTime, spread, timeBetweenShooting;
    public bool automatic;
    int bulletsInMag, bulletsShot;
    bool isShooting, isReadyToShoot, isReloading;

    public void PlayerInput()
    {
        if (automatic) { isShooting = Input.GetKey(KeyCode.Mouse0); }
        else if (!automatic) { isShooting = Input.GetKeyDown(KeyCode.Mouse0); }

        if (isReadyToShoot && isShooting && !isReloading && bulletsInMag > 0) { Shoot(); }
    }

    private void Reload()
    {
        if (isReadyToShoot) { }
    }

    private void Shoot()
    {
        isReadyToShoot = false;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out raycast, range, _enemies))
        {
            if (raycast.collider.CompareTag("Enemy"))
            {
                raycast.collider.GetComponent<enemyAI>().takeDamage(damage);
            }
        }

        bulletsInMag--;
        Invoke("ResetShoot", timeBetweenShooting);
    }

    private void ResetShoot()
    {
        isReadyToShoot = false;
    }
}
