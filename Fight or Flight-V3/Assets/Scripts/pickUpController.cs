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

        // Make Weapon Child of Anchor and Move it into position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one;

        // Rigidbody Kinematic and BC = Trigger
        rb.isKinematic = true;
        boxCollider.isTrigger = true;

        // Enable Script
        basicWeapon.enabled = true;
    }

    private void Drop()
    {
        eqipped = false;
        gunInHand = false;

        // Parent = Null
        transform.SetParent(null);

        // Rigidbody not Kinematic and BC = Not a Trigger
        rb.isKinematic = false;
        boxCollider.isTrigger = false;

        // Gun Momentum
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        // Make it one with the force
        rb.AddForce(cam.forward * dropForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        // Disable Script
        basicWeapon.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup Stuff
        if (!eqipped)
        {
            basicWeapon.enabled = false;
            rb.isKinematic = false;
            boxCollider.isTrigger = false;
        }
        if (eqipped)
        {
            basicWeapon.enabled = true;
            rb.isKinematic = true;
            boxCollider.isTrigger = true;
            gunInHand = true;
        }
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
