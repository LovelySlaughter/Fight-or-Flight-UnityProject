using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public NewGuns gunScript;
    public Rigidbody rb;
    public BoxCollider colli;
    public Transform player, anchor, cameraMc;
    public float pickUpRange, dropForce, dropUpForce;
    public bool quipped;
    public static bool slotFull;


    private void PickUp()
    {
        quipped = true;
        slotFull = true;

        transform.SetParent(anchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        colli.isTrigger = true;

        gunScript.enabled = true;
    }

    private void Drop()
    {
        quipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        colli.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(cameraMc.forward * dropForce, ForceMode.Impulse);
        rb.AddForce(cameraMc.up * dropUpForce, ForceMode.Impulse);
        gunScript.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!quipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            colli.isTrigger = false;

        }
        if (quipped) 
        { 
            gunScript.enabled = true;
            rb.isKinematic = true;
            colli.isTrigger = true;
            slotFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // To Pick Up!!
        Vector3 distanceFromPlayer = player.position - transform.position;
        if(!quipped && distanceFromPlayer.magnitude < pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) { PickUp(); }

        // To Drop!!
        if(quipped && Input.GetKeyDown(KeyCode.Q)) { Drop(); }

        // To Pick up W/ gun equipped!!
        if(quipped && Input.GetKeyDown(KeyCode.E)) { Drop(); PickUp(); }
    }
}
