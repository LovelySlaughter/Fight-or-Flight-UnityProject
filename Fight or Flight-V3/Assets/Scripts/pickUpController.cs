using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpController : MonoBehaviour
{
    public basicWeapon basicWeapon;
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public Transform player;
    public Transform gunContainer;
    public Transform cam;

    public float grabRange;
    public float dropForce;
    public float dropUpwardForce;

    public bool eqipped;
    public static bool gunInHand;

    private void PickUP()
    {
        eqipped = true;
        gunInHand = true;

        rb.isKinematic = true;
        boxCollider.isTrigger = true;
        basicWeapon.enabled = true;
    }

    private void Drop()
    {
        eqipped = false;
        gunInHand = false;

        rb.isKinematic = false;
        boxCollider.isTrigger = false;
        basicWeapon.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // to pickup weapon
        Vector3 gunDistance = player.position - transform.position;
        if (!eqipped && gunDistance.magnitude <= grabRange && Input.GetKeyDown(KeyCode.E) && !gunInHand)
        {
            PickUP();
        }

        // to drop weapon (hands empty)
        if (eqipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }

        if(eqipped && Input.GetKeyDown(KeyCode.E))
        {
            Drop();
            PickUP();
        }
    }
}
