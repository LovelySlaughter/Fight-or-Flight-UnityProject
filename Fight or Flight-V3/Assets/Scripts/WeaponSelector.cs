using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform weaponTransform;
    public float distance = 10f;
    GameObject currentWeapon;
    GameObject targetWeapon;

    bool canGrab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponCheck();

        if (canGrab)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentWeapon != null)
                {
                    DropDown();
                }
                PickUp();
            }
        }
        if (currentWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropDown();
            }
        }
    }



    private void WeaponCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if (hit.transform.tag == "CanGrab")
            {
                canGrab = true;
                targetWeapon = hit.transform.gameObject;
            }
            else
            {
                canGrab = false;
            }
        }
    }

    private void PickUp()
    {
        currentWeapon = targetWeapon;
        currentWeapon.transform.position = weaponTransform.position;
        currentWeapon.transform.parent = weaponTransform;
        currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void DropDown()
    {
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon = null;
    }
}
