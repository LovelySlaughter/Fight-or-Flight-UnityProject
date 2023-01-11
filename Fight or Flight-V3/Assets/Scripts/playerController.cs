using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Coded by Kat
public class playerController : MonoBehaviour //, gunParent
{

    [Header("--- Character Components ---")]
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject MainCamera;
    [SerializeField] int sprintModifier;

    [Header("--- Character Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumpAmount;
    [SerializeField] int playerSpeed;
    [SerializeField] int gravity;

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


    Vector3 movement;
    Vector3 velocity;
    int jumpCounter;
    bool sprint = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();

      



        //if (!isShooting)
        //{
        //    selectGun(gunID);
        //    if (!automatic && Input.GetButtonDown("Shoot"))
        //    {
        //        StartCoroutine(shoot());
        //    }
        //    else if(automatic && Input.GetButton("Shoot"))
        //    {
        //        StartCoroutine(shoot());
        //    }
        //}
    }

    void Movement()
    {  
        bool isSpringting = sprint;
        int sprintSpeed = playerSpeed;

        //reset jump counter
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpCounter = 0;
        }
        // sprint activator
        if (isSpringting)
        {
            sprintSpeed *= sprintModifier;
        }

        
        movement = (transform.right * Input.GetAxis("Horizontal")) +
            (transform.forward * Input.GetAxis("Vertical"));

        characterController.Move(movement * Time.deltaTime * sprintSpeed);
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

    GameObject bulletClone;
    //IEnumerator shoot()
    //{
    //    isShooting = true;
    //    //creates bullet
    //    bulletClone = Instantiate(bullet, P1ShootPOS.position, bullet.transform.rotation);

    //    // bulletClone.GetComponent<SphereCollider>().radius = 4;   this changes BULLET size

    //    //makes bullet go where your facing
    //    SetRotate(bulletClone.GetComponent<Rigidbody>().gameObject, MainCamera);

    //    //sets speed and direction
    //    bulletClone.GetComponent<Rigidbody>().velocity = bulletClone.GetComponent<Rigidbody>().transform.forward * bulletSpeed;

    //    //sets damage
    //    bulletClone.GetComponent<bullet>().bulletDamage = shootDamage;


    //    //sets shootrate
    //    yield return new WaitForSeconds(shootRate);

    //    isShooting = false;
    //}


    void SetRotate(GameObject toRotate, GameObject camera)
    {

        bulletClone.transform.rotation = Quaternion.Lerp(toRotate.transform.rotation, camera.transform.rotation, 500 * Time.deltaTime);
    }

    //public void selectGun(int x)
    //{
    //    switch(gunID)
    //    {
    //        case 4:
    //            //pistol
    //              shootRate = 0.4f;
    //            shootDamage = 1;
    //            bulletSpeed = 15;
    //            automatic = false;
    //            break;
    //        case 3:
    //            //ar rifle
    //            shootRate = 0.4f;
    //            shootDamage = 3;
    //            bulletSpeed = 20;
    //            automatic = true;
    //            break;
    //        case 2:
    //            //subgun
    //            shootRate = 0.2f;
    //            shootDamage = 1;
    //            bulletSpeed = 18;
    //            automatic = true;

    //            break;
    //        case 1:
    //            //sniper 
    //            shootRate = 3f;
    //            shootDamage = 6;
    //            bulletSpeed = 40;
    //            automatic = false;

    //            break;
    //        default:
    //            shootRate = 0.4f;
    //            shootDamage = 1;
    //            bulletSpeed = 15;
    //            automatic = false;
    //            break;
    //    }
    //}


    //Player damage done here
    //Coded By Mauricio
    public void takeDamage(int dmg)
    {
        healthPoints -= dmg;
    }

}
