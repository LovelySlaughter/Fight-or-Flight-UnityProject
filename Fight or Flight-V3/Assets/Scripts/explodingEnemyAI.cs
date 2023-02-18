using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class explodingEnemyAI : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;

    [Header("---- Enemy Stats ----")]
    [SerializeField] Transform headPos;
    [Range(10, 150)] [SerializeField] int HP;
    [SerializeField] int playerfaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;

    [Header("---- Explosion Stats ----")]
    [SerializeField] Transform explosionPos;
    [SerializeField] GameObject explosion;
    [Range(1, 50)] [SerializeField] int explosionDamage;
    [SerializeField] float explodeTriggerDist;
    [Range(0, 3)] [SerializeField] float explodeWaitTime;



    bool exploded = false;
    float dist;
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
        gameManager.instance.updateEnemyRemaining(1);
        speedOrig = agent.speed;
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        dist = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);

        if (playerInRange)
        {
            if (dist <= explodeTriggerDist)
            {
                StartCoroutine(startExplosion());
            }
            else if (!canSeePlayer() && !destinationChosen && agent.remainingDistance < 0.1f)
            {
                StartCoroutine(roam());
            }
            else if (canSeePlayer())
            {
                agent.stoppingDistance = stoppingDistOrig;
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
        if (HP <= 0 && exploded == false)
        {
            updateEnemyUI();
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

    IEnumerator startExplosion()
    {
        exploded = true;
        yield return new WaitForSeconds(explodeWaitTime);
        updateEnemyUI();
        GameObject explosionClone = Instantiate(explosion, explosionPos.position, explosion.transform.rotation);
        explosionClone.GetComponent<enemyExplosion>().explosionDamage = explosionDamage;
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
            agent.stoppingDistance = 0;
            playerInRange = false;
        }
    }

    public void updateEnemyUI()
    {
        gameManager.instance.updateEnemyRemaining(-1);
        Destroy(gameObject);
        gameManager.instance.UpdateEnemiesKilled(1);
    }
}
