using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class explodingProceduralAI : MonoBehaviour, IDamage
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


    //Start of enemy AI(not procedural)
    bool exploded = false;
    float dist;
    Vector3 playerDir;
    bool playerInRange;
    float angleToPlayer;
    bool destinationChosen;
    float speedOrig;
    float stoppingDistOrig;
    Vector3 startingPos;
    //End of enemy AI(not procedural)



    //Start of Enemy AI(procedural)
    [Header("---- Leg Move Settings ----")]
    [SerializeField] float _speed = 3f;
    [SerializeField] float smoothness = 5f;
    [SerializeField] int raysNb = 8;
    [SerializeField] float raysEccentricity = 0.2f;
    [SerializeField] float outerRaysOffset = 2f;
    [SerializeField] float innerRaysOffset = 25f;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastPosition;
    private Vector3 forward;
    private Vector3 upward;
    private Quaternion lastRot;
    private Vector3[] pn;
    private float multiplier;

    static Vector3[] GetClosestPoint(Vector3 point, Vector3 forward, Vector3 up, float halfRange, float eccentricity, float offset1, float offset2, int rayAmount)
    {
        Vector3[] res = new Vector3[2] { point, up };
        Vector3 right = Vector3.Cross(up, forward);
        float normalAmount = 1f;
        float positionAmount = 1f;

        Vector3[] dirs = new Vector3[rayAmount];
        float angularStep = 2f * Mathf.PI / (float)rayAmount;
        float currentAngle = angularStep / 2f;
        for (int i = 0; i < rayAmount; ++i)
        {
            dirs[i] = -up + (right * Mathf.Cos(currentAngle) + forward * Mathf.Sin(currentAngle)) * eccentricity;
            currentAngle += angularStep;
        }

        foreach (Vector3 dir in dirs)
        {
            RaycastHit hit;
            Vector3 largener = Vector3.ProjectOnPlane(dir, up);
            Ray ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset1 / 100f, dir);
            //Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                normalAmount += 1;
                positionAmount += 1;
            }
            ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset2 / 100f, dir);
            //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                normalAmount += 1;
                positionAmount += 1;
            }
        }
        res[0] /= positionAmount;
        res[1] /= normalAmount;
        return res;
    }
    //End of Enemy AI(procedural)

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateEnemyRemaining(1);
        speedOrig = agent.speed;
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;

        //Procedural Components
        velocity = new Vector3();
        forward = transform.forward;
        upward = transform.up;
        lastRot = transform.rotation;
        multiplier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        dist = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);

        //Procedural Animation
        velocity = (smoothness * velocity + (transform.position - lastPosition)) / (1f + smoothness);
        if (velocity.magnitude < 0.00025f)
            velocity = lastVelocity;
        lastPosition = transform.position;
        lastVelocity = velocity;

        if (playerInRange)
        {
            multiplier = 2f;
            StartProceduralAnimation();
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
            StartProceduralAnimation();
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

        GameObject explosionClone = Instantiate(explosion, explosionPos.position, explosion.transform.rotation);
        explosionClone.GetComponent<enemyExplosion>().explosionDamage = explosionDamage;
        updateEnemyUI();
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
        Destroy(gameObject);
        gameManager.instance.updateEnemyRemaining(-1);
        gameManager.instance.UpdateEnemiesKilled(1);
    }

    public void StartProceduralAnimation()
    {
        float valueY = agent.velocity.y;
        if (valueY != 0)
        {
            transform.position += transform.forward * valueY * _speed * multiplier * Time.fixedDeltaTime;
        }

        float valueX = agent.velocity.x;
        if (valueX != 0)
        {
            transform.position += Vector3.Cross(transform.up, transform.forward) * valueX * _speed * multiplier * Time.fixedDeltaTime;
        }
        if (valueX != 0 || valueY != 0)
        {
            pn = GetClosestPoint(transform.position, transform.forward, transform.up, 0.5f, 0.1f, 30, -30, 4);
            upward = pn[1];
            Vector3[] pos = GetClosestPoint(transform.position, transform.forward, transform.up, 0.5f, raysEccentricity, innerRaysOffset, outerRaysOffset, raysNb);
            transform.position = Vector3.Lerp(lastPosition, pos[0], 1f / (1f + smoothness));
            forward = velocity.normalized;
            Quaternion q = Quaternion.LookRotation(forward, upward);
            transform.rotation = Quaternion.Lerp(lastRot, q, 1f / (1f + smoothness));
        }

        lastRot = transform.rotation;
    }
}