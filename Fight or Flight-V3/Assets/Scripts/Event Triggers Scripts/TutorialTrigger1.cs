using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger1 : MonoBehaviour
{
    public GameObject areaTwo;
    public GameObject areaThree;
    public bool trigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!trigger) { areaTwo.SetActive(true); }
        else { areaTwo.SetActive(false); areaThree.SetActive(true); }
        
    }
}
