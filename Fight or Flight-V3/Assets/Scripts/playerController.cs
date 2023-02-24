using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


// Coded by Kat
public class playerController : MonoBehaviour
{

    [Header("--- Character Components ---")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject MainCamera;
    [SerializeField] NewGuns weapon;
    [SerializeField] AudioSource sounds;

    [Header("--- Character Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumpAmount;
    [SerializeField] int playerSpeed;
    [SerializeField] int sprintMod;
    [SerializeField] int gravity;
    [SerializeField] int pushBackTime;

    [Header("---  Audio ---")]
    [SerializeField] AudioClip[] playerDamageAudio;
    [Range(0, 1)][SerializeField] float damageAudioVolume;
    [SerializeField] AudioClip[] playerJumpAudio;
    [Range(0, 1)][SerializeField] float jumpAudioVolume;
    [SerializeField] AudioClip[] playerWalkAudio;
    [Range(0, 1)][SerializeField] float walkAudioVolume;

    //Gun Stats Update by Mauricio and Kat
    [Header("---- Gun Stats ----")]
    [SerializeField] List<gunStats> gunObjects = new List<gunStats>();
    [SerializeField] Transform shootPos;
    public GameObject MuzzleFlash;
    public GameObject bulletEffect;
    [SerializeField] GameObject bullet;
    [Range(15, 35)][SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [Range(1, 9)][SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;


    bool canGrab;

    public Vector3 pushBack;

    int selectedGun;
    int jumpCounter;
    Vector3 movement;
    Vector3 velocity;
    int HPOrig;
    int playerGravOrg;

    //Connor's wallrunning stuff
    public LayerMask whatIsWall;
    public float wallRunForce;
    public float maxWallSpeed;
    bool isWallLeft;
    bool isWallRight;
    bool isWallRunning;
    Vector3 normVec;
    float time;

    bool isWalking;

    bool isShooting;

    bool isSprinting;
    //End of Connor's wallrunning stuff

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    public void ResetTimer()
    {

    }

    void Start()
    {
        //Update by Mauricio
        playerGravOrg = gravity;
        HPOrig = healthPoints;
        updatePlayerHP();
        respawnPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        //int sprintSpeed = playerSpeed;

        //Edit Mauricio
        if (!gameManager.instance.isPaused)
        {
            if (!isWalking && characterController.velocity.magnitude > 0)
            {
                StartCoroutine("StepSounds");
            }

            pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);


            Movement();
            Sprint();
            SelectGun();

            //Connor's wallrunning check
            if ((characterController.collisionFlags & CollisionFlags.Sides) != 0 && !characterController.isGrounded)
            {
                isWallRunning = true;
            }
            else
            {
                isWallRunning = false;
                gravity = playerGravOrg;
            }


            if (gunObjects.Count > 0 && !isShooting && Input.GetButtonDown("Shoot")) //{ Shoot(); }
            {
                StartCoroutine(Shoot());
                
             
                    Instantiate(MuzzleFlash, shootPos.position, Quaternion.identity);
                
               

            }


        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isShooting == true)
        {
            StartCoroutine("Shoot");
        }

    }

    void Movement()
    {



        //reset jump counter
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpCounter = 0;
        }
        // sprint activator

        movement = (transform.right * Input.GetAxis("Horizontal")) +
            (transform.forward * Input.GetAxis("Vertical"));


        characterController.Move(playerSpeed * Time.deltaTime * movement); //controls our move input

        // jump controlss
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumpAmount)
        {
            velocity.y = jumpHeight;
            jumpCounter++;
            sounds.PlayOneShot(playerJumpAudio[Random.Range(0, playerJumpAudio.Length - 1)], jumpAudioVolume);

        }

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move((velocity + pushBack) * Time.deltaTime);
    }

    //Connor's Updates
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!characterController.isGrounded && hit.normal.y < 0.1f)
        {
            isWallRight = Physics.Raycast(transform.position, characterController.transform.right, 1f, whatIsWall);
            isWallLeft = Physics.Raycast(transform.position, -characterController.transform.right, 1f, whatIsWall);

            if (!isWallLeft && !isWallRight)
            {
                gravity = playerGravOrg;
                isWallRunning = false;
            }
            if (isWallLeft || isWallRight)
            {
                //jumpCounter = 0;
                gravity = playerGravOrg;
            }
            if(isWallLeft && Input.GetButtonDown("Jump"))
            {
                jumpCounter = maxJumpAmount;
            }
            if (isWallRight && Input.GetButtonDown("Jump"))
            {
                jumpCounter = maxJumpAmount;
            }
            if (!isWallRight || !isWallRight)
            {
                jumpCounter = 0;
            }

            if (Input.GetKey(KeyCode.D) && isWallRight)
            {
                gravity = 0;

                if (velocity.magnitude <= maxWallSpeed)
                {
                    movement = (hit.normal * wallRunForce * Time.deltaTime);

                    if (isWallRight)
                    {
                        movement = (hit.normal * wallRunForce / 5 * Time.deltaTime);
                    }
                    else
                    {
                        movement = (-hit.normal * wallRunForce / 5 * Time.deltaTime);
                    }
                }
            }
            if (Input.GetKey(KeyCode.A) && isWallLeft)
            {
                gravity = 0;

                if (velocity.magnitude <= maxWallSpeed)
                {
                    movement = (-hit.normal * wallRunForce * Time.deltaTime);

                    if (isWallLeft)
                    {
                        movement = (-hit.normal * wallRunForce / 5 * Time.deltaTime);
                    }
                    else
                    {
                        movement = (hit.normal * wallRunForce / 5 * Time.deltaTime);
                    }
                }
            }

        }
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }

    IEnumerator StepSounds()
    {
        if (characterController.isGrounded)
        {
            isWalking = true;
            if (isSprinting)
            {
                sounds.PlayOneShot(playerWalkAudio[Random.Range(0, playerWalkAudio.Length - 1)], walkAudioVolume);
                yield return new WaitForSeconds(0.25f);
            }
            else if (!isSprinting)
            {
                sounds.PlayOneShot(playerWalkAudio[Random.Range(0, playerWalkAudio.Length - 1)], walkAudioVolume);
                yield return new WaitForSeconds(0.5f);
            }

            isWalking = false;
        }
    }


    //Updated Shoot Method By Mauricio
    IEnumerator Shoot()
    {
        isShooting = true;
        //weapon.GetComponent<NewGuns>().Shoot();
        

        sounds.PlayOneShot(gunObjects[selectedGun].gunShots, gunObjects[selectedGun].gunShotsVolume);
        //Instantiate(MuzzleFlash, shootPos.position, Quaternion.identity);
     
        RaycastHit hit;
       

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
          
            //Destroy(gunObjects[selectedGun].muzzleFlash, 0.2f);
           Instantiate(bulletEffect, hit.point, shootPos.rotation);

            if (hit.collider.GetComponent<IDamage>() != null /*&& hit.collider == hit.collider.GetComponent<CapsuleCollider>()*/)
            {
                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                
            }
        }

        
        yield return new WaitForSeconds(shootRate);
        
        isShooting = false;
    }

    //Player damage done here
    //Coded/Updated By Mauricio
    public void takeDamage(int dmg)
    {
        healthPoints -= dmg;
        updatePlayerHP();
        StartCoroutine(gameManager.instance.flashDamage());
        sounds.PlayOneShot(playerDamageAudio[Random.Range(0, playerDamageAudio.Length - 1)], damageAudioVolume);

        if (healthPoints <= 0)
        {
         
            gameManager.instance.playerDead();
            
        }
    }

    public void updatePlayerHP()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)healthPoints / (float)HPOrig;
    }

    public void respawnPlayer()
    {
        characterController.enabled = false;
        healthPoints = HPOrig;
        updatePlayerHP();
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        characterController.enabled = true;
    }

    //Updated by Kat
    public void GunPickUp(gunStats gunObj)
    {
        if (gunModel.CompareTag("Untagged"))
        {
            gunObjects.Add(gunObj);

            shootRate = gunObj.Rate;
            shootDist = gunObj.Range;
            shootDamage = gunObj.Damage;
            MuzzleFlash = gunObj.muzzleFlash;
            bulletEffect = gunObj.bulletHoles;
            //gunObj.gunBulletPos = shootPos;

            gunModel.GetComponent<MeshFilter>().sharedMesh = gunObj.weaponModel.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObj.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
            gunModel.transform.localScale = gunObj.scale;

            selectedGun = gunObjects.Count - 1;
        }
    }

    void ChangeGun()
    {
        shootRate = gunObjects[selectedGun].Rate;
        shootDist = gunObjects[selectedGun].Range;
        shootDamage = gunObjects[selectedGun].Damage;
        MuzzleFlash = gunObjects[selectedGun].muzzleFlash;
        bulletEffect = gunObjects[selectedGun].bulletHoles;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunObjects[selectedGun].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObjects[selectedGun].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunModel.transform.localScale = gunObjects[selectedGun].scale;
    }

    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunObjects.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }
}
