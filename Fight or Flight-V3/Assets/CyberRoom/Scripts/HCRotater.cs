using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCRotater : MonoBehaviour
{
    public bool x;
    public bool y;
    public bool z;
    public float Speed;
    public GameObject[] Rotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Rotate != null)
        {
            if (x)
            {
                foreach (GameObject obj in Rotate)
                {
                    obj.transform.Rotate(Speed * Time.deltaTime, 0, 0);
                }
            }
            if (y)
            {
                foreach (GameObject obj in Rotate)
                {
                    obj.transform.Rotate(0, Speed * Time.deltaTime, 0);
                }
            }
            if (z)
            {
                foreach (GameObject obj in Rotate)
                {
                    obj.transform.Rotate(0, 0, Speed * Time.deltaTime);
                }
            }
        }
    }
}
