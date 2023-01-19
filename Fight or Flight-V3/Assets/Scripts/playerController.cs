using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Coded by Kat
public class playerController : MonoBehaviour 
{

    [Header("--- Character Components ---")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject MainCamera;
    [SerializeField] int sprintModifier;
    [SerializeField] GameObject weapon;

    [Header("--- Character Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumpAmount;
    [SerializeField] int playerSpeed;
    [SerializeField] int gravity;

    //Gun Stats Update by Mauricio
    [Header("---- Gun Stats ----")]
    [SerializeField] List<gunObjScript> gunObjects = new List<gunObjScript>();
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(15, 35)] [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [Range(1, 9)] [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel; 
    
    [Header("--- Weapon Select Stuff ---")]
    public Transform weaponTransform;
    public float distance = 10f;
    GameObject currentWeapon;
    GameObject targetWeapon;

    bool canGrab;

    int selectedGun;
    int jumpCounter;
    Vector3 movement;
    Vector3 velocity;
    int HPOrig;

    bool isShooting;

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
        WeaponSetUp();
        
        //Edit Mauricio
        if (!gameManager.instance.isPaused)
        {
            Movement();
            SelectGun();


            if (gunObjects.Count > 0 && !isShooting && Input.GetButton("Shoot"))
                    StartCoroutine(shoot());
            
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isShooting == true)
        {
            StartCoroutine("Shoot");
        }


        bool sprint = (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
        bool isSpringting = sprint;

        if (isSpringting)
        {
            sprintSpeed *= sprintModifier;
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

        characterController.Move(velocity * Time.deltaTime);
    }
    

    //Updated Shoot Method By Mauricio
    IEnumerator shoot()
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

    public void GunPickUp(gunObjScript gunObj)
    {
        gunObjects.Add(gunObj);

        shootRate = gunObj.Rate;
        shootDist = gunObj.Range;
        shootDamage = gunObj.Damage;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunObj.gun.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObj.gun.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void ChangeGun()
    {
        shootRate = gunObjects[selectedGun].Rate;
        shootDist = gunObjects[selectedGun].Range;
        shootDamage = gunObjects[selectedGun].Damage;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunObjects[selectedGun].gun.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunObjects[selectedGun].gun.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunObjects.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }

    private void WeaponCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if (hit.transform.tag == "CanGrab")
            {
                canGrab = true;
                targetWeapon = hit.transform.gameObject;
            }
            else
            {
                canGrab = false;
            }
        }
    }

    private void PickUp()
    {
        currentWeapon = targetWeapon;
        currentWeapon.transform.position = weaponTransform.position;
        currentWeapon.transform.parent = weaponTransform;
        currentWeapon.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void DropDown()
    {
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon = null;
    }

    void WeaponSetUp()
    {
        WeaponCheck();

        if (canGrab)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentWeapon != null)
                {
                    DropDown();
                }
                PickUp();
            }
        }
        if (currentWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropDown();
            }
        }
    }
}
