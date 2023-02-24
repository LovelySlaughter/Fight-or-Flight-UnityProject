using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

//Coded By Mauricio/Kat
public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    //public GameObject colliderIgnore;
    //public SphereCollider collides;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;

    [Header("---- Enemy Stats ----")]
    [SerializeField] Transform headPos;
    [Range(10, 150)] [SerializeField] int HP;
    [SerializeField] int playerfaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;
    [SerializeField] bool enemySniper;

    [Header("---- Gun Stats ----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [Range(15, 70)] [SerializeField] int bulletSpeed;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [Range(10, 70)] [SerializeField] int shootDist;
    [Range(1, 10)] [SerializeField] int shootDamage;

    [Header("---- Explosion Stats ----")]
    [SerializeField] bool explodeOnDead;
    [SerializeField] Transform explosionPos;
    [SerializeField] GameObject explosion;
    [Range(1, 100)] [SerializeField] int explosionDamage;


    bool explode;
    bool isShotting;
    Vector3 playerDir;
    bool playerInRange;
    float angleToPlayer;
    bool destinationChosen;
    float speedOrig;
    float stoppingDistOrig;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        //colliderIgnore = GameObject.FindGameObjectWithTag("Collider");
        //collides = colliderIgnore.GetComponent<SphereCollider>();
        gameManager.instance.updateEnemyRemaining(1);
        speedOrig = agent.speed;
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

        if (playerInRange)
        {
            facePlayer();
            if (!canSeePlayer() && !destinationChosen && agent.remainingDistance < 0.1f)
            {
                StartCoroutine(roam());
            }
            if (canSeePlayer())
            {
                if (!enemySniper)
                {
                    agent.stoppingDistance = 5;
                }
                else if (enemySniper)
                {
                    agent.stoppingDistance = stoppingDistOrig;
                }
            }

        }
        else if (!destinationChosen && agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
        {
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        destinationChosen = true;
        agent.stoppingDistance = 0;

        yield return new WaitForSeconds(waitTime);

        destinationChosen = false;

        Vector3 randomDir = Random.insideUnitSphere * roamDist;
        startingPos = transform.position;
        randomDir += startingPos;
        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(randomDir.x, 0, randomDir.z), out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        if (hit.position != null)
        {
            agent.CalculatePath(hit.position, path);
        }
        agent.SetPath(path);
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }
                //this remaindistance<20 prevents the enemies from shooting when out of range
                if (!isShotting && angleToPlayer <= shootAngle && agent.remainingDistance < 15)
                {
                    StartCoroutine(shoot());
                }

                return true;
            }
            else if (angleToPlayer > viewAngle)
            {
                agent.stoppingDistance = 0;
            }
        }
        return false;

    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        StartCoroutine(flashDamage());
        agent.stoppingDistance = 0;

        agent.SetDestination(gameManager.instance.player.transform.position);
        if (HP <= 0)
        {
            if (explodeOnDead)
            {
                StartCoroutine(startExplosion());
            }
            Destroy(gameObject);
            gameManager.instance.updateEnemyRemaining(-1);
            gameManager.instance.UpdateEnemiesKilled(1);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShotting = true;

        if (!explodeOnDead)
        {
            anim.SetTrigger("Shoot");
        }

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
        /*     Begining of Collider Fix tried     */
        //other = colliderIgnore.GetComponent<SphereCollider>();
        if (other.CompareTag("Player"))
        {

            playerInRange = true;

        }
    }
    public void OnTriggerExit(Collider other)
    {
        /*     Begining of Collider Fix tried     */
        //other = collides;
        if (other.CompareTag("Player"))
        {
            agent.stoppingDistance = 0;
            playerInRange = false;
        }
    }

    IEnumerator startExplosion()
    {
        explode = true;
        anim.SetTrigger("Explode");
        GameObject explosionClone = Instantiate(explosion, explosionPos.position, explosion.transform.rotation);
        explosionClone.GetComponent<enemyExplosion>().explosionDamage = explosionDamage;
        yield return new WaitForSeconds(1);
        Destroy(explosionClone);
        explode = false;
    }
}
