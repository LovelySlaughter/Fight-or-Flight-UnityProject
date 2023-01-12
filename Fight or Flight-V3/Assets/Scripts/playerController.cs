using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Coded by Kat
public class playerController : MonoBehaviour //, gunParent
{

    [Header("--- Character Components ---")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject MainCamera;
    //[SerializeField] int sprintModifier;

    [Header("--- Character Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumpAmount;
    [SerializeField] int playerSpeed;
    [SerializeField] int gravity;

    //Gun Stats Update by Mauricio
    [Header("---- Gun Stats ----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(15, 35)] [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [Range(1, 9)] [SerializeField] int shootDamage;

    ////Missing Gun Stats and Guns
    //[Header("--- Gun stats ---")]
    //[Range(0.01f, 3)] [SerializeField] float shootRate;
    //[Range(1, 20)] [SerializeField] int shootDamage;
    //[Range(15, 50)] [SerializeField] int bulletSpeed;
    //[SerializeField] bool automatic;
    ////projectile size is part of bullet class
    //[SerializeField] Transform P1ShootPOS;
    //[SerializeField] GameObject bullet;
    //[Range(1, 4)] [SerializeField] int gunID;
    //bool isShooting;
    //private GameObject MainCamera;

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
        //int sprintSpeed = playerSpeed;


        //Edit Mauricio
        if (!gameManager.instance.isPaused)
        {
            Movement();

           
                if (!isShooting && Input.GetButton("Shoot"))
                    StartCoroutine(shoot());
            
        }


        /*bool sprint = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        bool isSpringting = sprint;

        if (isSpringting)
        {
            sprintSpeed *= sprintModifier;
        }

        characterController.Move(movement * Time.deltaTime * sprintSpeed);

        if (!isShooting)
        {
            selectGun(gunID);
            if (!automatic && Input.GetButtonDown("Shoot"))
            {
                StartCoroutine(shoot());
            }
            else if (automatic && Input.GetButton("Shoot"))
            {
                StartCoroutine(shoot());
            }
        }*/

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
    //Shoot method missing here
    //yee ye i hear you
    //coded by Miguel
    /*GameObject bulletClone;
    IEnumerator shoot()
    {
        isShooting = true;
        //creates bullet
        bulletClone = Instantiate(bullet, P1ShootPOS.position, bullet.transform.rotation);

        // bulletClone.GetComponent<SphereCollider>().radius = 4;   this changes BULLET size

        //makes bullet go where your facing
        SetRotate(bulletClone.GetComponent<Rigidbody>().gameObject, MainCamera);

        //sets speed and direction
        bulletClone.GetComponent<Rigidbody>().velocity = bulletClone.GetComponent<Rigidbody>().transform.forward * bulletSpeed;

        //sets damage
        bulletClone.GetComponent<bullet>().bulletDamage = shootDamage;


        //sets shootrate
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }*/


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

    /*void SetRotate(GameObject toRotate, GameObject camera)
    {

        bulletClone.transform.rotation = Quaternion.Lerp(toRotate.transform.rotation, camera.transform.rotation, 500 * Time.deltaTime);
    }
    */
    /*public void selectGun(int x)
    {
        switch (gunID)
        {
            case 4:
                //pistol
                shootRate = 0.4f;
                shootDamage = 1;
                bulletSpeed = 15;
                automatic = false;
                break;
            case 3:
                //ar rifle
                shootRate = 0.4f;
                shootDamage = 3;
                bulletSpeed = 20;
                automatic = true;
                break;
            case 2:
                //subgun
                shootRate = 0.2f;
                shootDamage = 1;
                bulletSpeed = 18;
                automatic = true;

                break;
            case 1:
                //sniper 
                shootRate = 3f;
                shootDamage = 6;
                bulletSpeed = 40;
                automatic = false;

                break;
            default:
                shootRate = 0.4f;
                shootDamage = 1;
                bulletSpeed = 15;
                automatic = false;
                break;
        }
    }*/


    //Player damage done here
    //Coded/Updated By Mauricio
    public void takeDamage(int dmg)
    {
        healthPoints -= dmg;
        updatePlayerHP();

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
}
