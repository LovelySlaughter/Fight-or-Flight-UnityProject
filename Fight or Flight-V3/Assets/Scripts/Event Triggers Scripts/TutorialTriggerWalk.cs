using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerWalk : MonoBehaviour
{
    public GameObject WalkTutorial;

    private void OnTriggerEnter(Collider other)
    {
        WalkTutorial.SetActive(false);
    }
}
