using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("--- Character Components ---")]
    [SerializeField] CharacterController characterController;

    [Header("--- Character Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumpAmount;
    [SerializeField] int playerSpeed;
    [SerializeField] int gravity;

    Vector3 movement;
    int jumpCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Movement()
    {
        movement = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Verticle"));
    }
}
