using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, ILonion32@gmail.com, 2021.
public class USTraffVehicle : MonoBehaviour
{
    [Header("speed movement:")]
    public float CarSpeed = 5;
    [Header("distance: start - destroy")]
    public float CarLife = 900;
    [HideInInspector]
    public USTraffic _USTrafficLinked;
    Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.TransformDirection(Vector3.forward) * CarSpeed * Time.deltaTime;
        if (FastDistance(transform.position, StartPosition, CarLife))
        {
            _USTrafficLinked.VehicleCount--;
            Destroy(this.gameObject);
        }
    }

    //fast fake distance (w/o 'Y')
    bool FastDistance(Vector3 Self, Vector3 Target, float Radius)
    {
        bool Xpass = false;
        bool Zpass = false;

        //x
        if ((Self.x >= 0 & Target.x >= 0) | (Self.x < 0 & Target.x < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.x) - Mathf.Abs(Target.x)) > Radius) Xpass = true;
        }
        else
        {
            if (Mathf.Abs(Self.x) + Mathf.Abs(Target.x) > Radius) Xpass = true;
        }

        //z
        if ((Self.z >= 0 & Target.z >= 0) | (Self.z < 0 & Target.z < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.z) - Mathf.Abs(Target.z)) > Radius) Zpass = true;
        }
        else
        {
            if (Mathf.Abs(Self.z) + Mathf.Abs(Target.z) > Radius) Zpass = true;
        }

        if (Xpass | Zpass) return true;
        else return false;
    }
}
