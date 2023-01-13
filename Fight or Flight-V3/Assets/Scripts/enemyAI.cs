using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Coded By Mauricio
public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    [SerializeField] NavMeshAgent agent;

    [Header("---- Enemy Stats ----")]
    [SerializeField] Transform headPos;
    [Range(10, 150)] [SerializeField] int HP;
    [SerializeField] int playerfaceSpeed;
    [SerializeField] int viewAngle;

    [Header("---- Gun Stats ----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(15, 35)] [SerializeField] int bulletSpeed;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [Range(10, 50)] [SerializeField] int shootDist;
    [Range(1, 10)] [SerializeField] int shootDamage;

    float angleToPlayerw;
    bool isShotting;
    Vector3 playerDir;
    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateEnemyRemaining(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
           canSeePlayer();
        }


    }

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayerw = Vector3.Angle(playerDir, transform.forward);

        Debug.Log(angleToPlayerw);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit impact;
        if (Physics.Raycast(headPos.position, new Vector3(playerDir.x,0, playerDir.z), out impact))
        {
            if (impact.collider.CompareTag("Player") && angleToPlayerw <= viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isShotting)
                {
                    StartCoroutine(shoot());
                }
            }
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            gameManager.instance.updateEnemyRemaining(-1);
            Destroy(gameObject);
        }
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

    void facePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);

        playerDir.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerfaceSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }
    //miguel
    //public void selectGun(int x)
    //{
    //    switch (gunID)
    //    {
    //        case 4:
    //            //pistol
    //            shootRate = 0.4f;
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
    //}
    //}

}
