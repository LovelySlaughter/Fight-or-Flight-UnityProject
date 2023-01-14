using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    //Graphics
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] TextMeshProUGUI ammoDisplay;

    // Recoil
    [SerializeField] Rigidbody rigidbody;
    public float recoilForce;

    // Bug Fix
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

        //Set Ammo Display
        if (ammoDisplay != null)
        {
           if(!automatic) { ammoDisplay.SetText(bulletsRemaining / bulletsPerTap + "/" + magazineSize / bulletsPerTap); }
           else { ammoDisplay.SetText(bulletsRemaining + "/" + magazineSize); }
        }
    }

    private void MyInput()
    {
        // check if gun is automatic
        if (automatic) { isShooting = Input.GetKey(KeyCode.Mouse0); }
        else { isShooting = Input.GetKeyDown(KeyCode.Mouse0); }

        // Reloading Early
        if(Input.GetKeyDown(KeyCode.R) && bulletsRemaining < magazineSize && !reloading) { Reload(); }
        // Mag Empty Auto Reload
        if(readyToShoot && isShooting && !reloading && bulletsRemaining <= 0) { Reload(); }

        // Shooting Script
        if (readyToShoot && isShooting && !reloading && bulletsRemaining > 0)
        {
            bulletsFired = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find Shooting Aim Location
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycastHit;

        // Check Hit
        Vector3 aimLocation;
        if (Physics.Raycast(ray, out raycastHit)) { aimLocation = raycastHit.point; }
        else { aimLocation = ray.GetPoint(shootRange); }

        //Calculate direction without spread
        Vector3 noSpreadDir = aimLocation - shootPOS.position;

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //New Direction with spread
        Vector3 yesSpreadDir = noSpreadDir + new Vector3(x, y, 0);

        //Spawn Bullet
        GameObject CurrentBullet = Instantiate(bullets, shootPOS.position, Quaternion.identity);
        //Rotate Bullet
        CurrentBullet.transform.forward = noSpreadDir.normalized;

        // Add forces
        CurrentBullet.GetComponent<Rigidbody>().AddForce(yesSpreadDir * shootForce, ForceMode.Impulse);
        CurrentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        

        //Activate Muzzle Flash
        if (muzzleFlash != null) { Instantiate(muzzleFlash, shootPOS.position, Quaternion.identity); }

        bulletsRemaining--;
        bulletsFired++;

        //to reset shot
        if (allowInvoke) 
        { 
            Invoke("ResetShot", timeBetweenShots);

            // Recoil Force
            rigidbody.AddForce(-yesSpreadDir.normalized * recoilForce, ForceMode.Impulse);
        }

        //burst or automatic reset shot
        if (bulletsFired < bulletsPerTap && bulletsRemaining > 0) { Invoke("Shoot", timeBetweenShots); }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsRemaining = magazineSize;
        reloading = false;
    }
}
