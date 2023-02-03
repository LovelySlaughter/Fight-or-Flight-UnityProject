using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger1 : MonoBehaviour
{
    public GameObject areaTwo;

    private void OnTriggerEnter(Collider other)
    {
        areaTwo.SetActive(true);
    }
}
