using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


// Coded by Kat
public class playerController : MonoBehaviour
{

    [Header("--- Character Components ---")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject weapon;
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

    //Gun Stats Update by Mauricio and Kat
    [Header("---- Gun Stats ----")]
    [SerializeField] List<gunStats> gunObjects = new List<gunStats>();
    [SerializeField] Transform shootPos;
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

    bool isShooting;

    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        //Update by Mauricio
        HPOrig = healthPoints;
        updatePlayerHP();
        respawnPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        int sprintSpeed = playerSpeed;

        //Edit Mauricio
        if (!gameManager.instance.isPaused)
        {

            pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);


            Movement();
            Sprint();
            SelectGun();


            if (gunObjects.Count > 0 && !isShooting && Input.GetButton("Shoot"))
                StartCoroutine(Shoot());

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isShooting == true)
        {
            StartCoroutine("Shoot");
        }


        bool sprint = (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
        bool isSpringting = sprint;

        if (isSpringting)
        {
            sprintSpeed *= sprintMod;
        }

        characterController.Move(movement * Time.deltaTime * sprintSpeed);



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
        }

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move((velocity + pushBack) * Time.deltaTime);
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


    //Updated Shoot Method By Mauricio
    IEnumerator Shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = MainCamera.transform.forward * bulletSpeed;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
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
        gunObjects.Add(gunObj);

        shootRate = gunObj.Rate;
        shootDist = gunObj.Range;
        shootDamage = gunObj.Damage;


        gunModel.GetComponent<MeshFilter>().sharedMesh = gunObj.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObj.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunModel.transform.localScale = gunObj.scale;
    }

    void ChangeGun()
    {
        shootRate = gunObjects[selectedGun].Rate;
        shootDist = gunObjects[selectedGun].Range;
        shootDamage = gunObjects[selectedGun].Damage;

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
