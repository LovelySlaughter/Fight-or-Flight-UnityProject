using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Coded by Kat
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

    //Missing Gun Stats and Guns

    Vector3 movement;
    Vector3 velocity;
    int jumpCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //reset jump counter
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpCounter = 0;
        }

        if (Input.GetButtonDown("Sprint") && characterController.isGrounded)
        {
            velocity = velocity * 2;
        }
        
        movement = (transform.right * Input.GetAxis("Horizontal")) + 
            (transform.forward * Input.GetAxis("Vertical"));

        characterController.Move(movement * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumpAmount)
        {
            velocity.y = jumpHeight;
            jumpCounter++;
        }

        velocity.y -= gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
    //Shoot method missing here

    //Player damage done here
    //Coded By Mauricio
    public void takeDamage(int dmg)
    {
        healthPoints -= dmg;
    }

}
