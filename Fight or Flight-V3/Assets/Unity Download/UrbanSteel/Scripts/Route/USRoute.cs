using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, ILonion32@gmail.com, 2021.
public class USRoute : MonoBehaviour 
{
    [HideInInspector]
    public Transform[] RoutePoints;

    //collect all points (full route) in children
    //route is sensitive to the children gameobject hierarchy
    void Awake()
    {
        if (transform.childCount != 0)
        {
            RoutePoints = new Transform[transform.childCount];
        }
        else
        {
            Debug.Log(" <color=yellow> no route points! </color>");
            return;
        }
        for (int k = 0; k < transform.childCount; k++)
        {
            RoutePoints[k] = transform.GetChild(k);
        }
    }

	void Start () 
    {

	}
	
	void Update () 
    {
		
	}

    //draw lines (full route) in editor
    void OnDrawGizmos()
    {
        Vector3 Point = Vector3.zero;
        Vector3 LastPoint = Vector3.zero;
        for (int k = 0; k < transform.childCount; k++)
        {
            Point = transform.GetChild(k).transform.position;
            if (LastPoint != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(Point, LastPoint);
            }
            LastPoint = Point;
        }
    }
}
