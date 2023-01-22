using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

//Coded By Mauricio
public class DeliveryDrone : MonoBehaviour
{
    [Header("---- Components ----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] Renderer model;
    [SerializeField] AudioSource sounds;

    [Header("---- Enemy Stats ----")]
    [SerializeField] Transform headPos;
    [SerializeField] int playerfaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;


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

            
            }
        }
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
