using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveText : MonoBehaviour
{
    public GameObject text;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(text);
    }
}
