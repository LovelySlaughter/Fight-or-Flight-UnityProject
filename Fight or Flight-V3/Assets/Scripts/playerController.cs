using System.Collections;
using System.Collections.Generic;
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

        
        //Edit Mauricio
        if (!gameManager.instance.isPaused)
        {
            Movement();

           
                if (gunObjects.Count > 0 && !isShooting && Input.GetButton("Shoot"))
                    StartCoroutine(shoot());
            
        }


        bool sprint = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
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
}
