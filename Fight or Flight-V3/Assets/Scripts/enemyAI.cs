using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

//Coded By Mauricio
public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    [Header("---- Enemy Stats ----")]
    [SerializeField] Transform headPos;
    [Range(10, 150)] [SerializeField] int HP;
    [SerializeField] int playerfaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;


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
    // Updat by Kat
    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (playerInRange)
        {
           canSeePlayer();
        }


    }

    // Update by Kat
    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayerw = Vector3.Angle(playerDir, transform.forward);

        Debug.Log(angleToPlayerw);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit impact;
        if (Physics.Raycast(headPos.position, playerDir, out impact))
        {
            if (impact.collider.CompareTag("Player") && angleToPlayerw <= viewAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isShotting && angleToPlayerw <= shootAngle)
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
        bulletClone.GetComponent<Rigidbody>().velocity = (gameManager.instance.player.transform.position - headPos.transform.position).normalized * bulletSpeed;
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
   
}
