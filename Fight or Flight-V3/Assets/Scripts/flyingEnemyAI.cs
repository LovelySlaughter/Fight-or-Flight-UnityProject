using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Coded By Mauricio
public class flyingEnemyAI : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    public bool chase = false;
    public Transform startingPoint;
    [Range(-5, 5)] [SerializeField] public int zdistToPlayer;
    [Range(-5, 5)][SerializeField] public int xdistToPlayer;
    [Range(-5, 5)][SerializeField] public int heightToPlayer;
    [SerializeField] Renderer model;


    [Header("---- Enemy Stats ----")]
    [SerializeField] Transform headPos;
    [Range(10, 150)] [SerializeField] int HP;
    public float flySpeed;
    [SerializeField] int faceplayerSpeed;

    [Header("---- Gun Stats ----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(15, 35)] [SerializeField] int bulletSpeed;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int shootDamage;

    bool isShotting;
    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.player = GameObject.FindGameObjectWithTag("Player");
        gameManager.instance.updateEnemyRemaining(1);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (chase == true)
        {
            if (gameManager.instance.player == null)
            {
                return;
            }
            playerDir = gameManager.instance.player.transform.position;

            facePlayer();
            Chase();
            if (!isShotting)
            {
                StartCoroutine(shoot());
            }
        }
        else
        {
            //Go to starting Position
            returnStartingPos();
        }
        

    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            gameManager.instance.updateEnemyRemaining(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.30f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShotting = true;

        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        bulletClone.GetComponent<bullet>().bulletDamage = shootDamage;

        yield return new WaitForSeconds(shootRate);
        isShotting = false;
    }
    private void Chase()
    {

        playerDir.z += zdistToPlayer;
        playerDir.x += xdistToPlayer;
        playerDir.y += heightToPlayer;

        transform.position = Vector3.MoveTowards(transform.position, playerDir, flySpeed * Time.deltaTime);
    }

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir - headPos.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceplayerSpeed);
    }
    void returnStartingPos()
    {
        //Returns to starting point
        transform.position = Vector3.MoveTowards(transform.position, startingPoint.transform.position, flySpeed * Time.deltaTime);

        if (transform.position == startingPoint.position && transform.rotation.x != 0)
        {
            Quaternion rot = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceplayerSpeed);
        }

    }

}
