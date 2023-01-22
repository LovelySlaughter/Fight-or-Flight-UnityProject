using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//IL.ranch, ILonion32@gmail.com, 2021.
public class USVehicle : MonoBehaviour 
{
    [Header("[simple route system]")]
    [Space(10)]
    public USRoute Route;
    [Space(15)]
    [Header("if enabled: first point will be after last point")]
    [Header("if disabled: penultimate point will be after last point")]
    public bool IsLoop;
    public float SpeedMove = 1.1f;
    [Header("in case your vehicle spinning around increase HitDistance:")]
    public float HitDistance = 0.7f;
    Transform CurrentTarget;
    int k;
    bool direction = true;
    bool RouteFound;
    bool IsExecute;
    Animator _Animator;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

    void Start ()
    {
        _Animator = GetComponent<Animator>();
        _Animator.CrossFade("Base Layer.Idle", 0.1f);

        //get first nearest route point:
        float LastDistance = 10000f;
        for (int i = 0; i < Route.RoutePoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, Route.RoutePoints[i].position);
            if (dist < LastDistance)
            {
                CurrentTarget = Route.RoutePoints[i];
                LastDistance = dist;
                RouteFound = true;
                k = i;
            }
        }

        StartCoroutine(TimeOutCorot(UnityEngine.Random.Range(1.0f, 3.0f)));
    }

    void FixedUpdate()
    {
        if (RouteFound)
        {
            if (IsExecute)
            {
                //lookat
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(CurrentTarget.position - transform.position), 6 * Time.deltaTime);

                //physics move
                Vector3 futur_pos = transform.TransformDirection(new Vector3(0, 0, SpeedMove * Time.deltaTime));
                Vector3 step_pos = transform.position + futur_pos;
                GetComponent<Rigidbody>().MovePosition(step_pos);

                //hit route point
                if (FastDistance(transform, CurrentTarget, HitDistance))
                {
                    //positive
                    if (direction)
                    {
                        k++;
                        //get new one
                        if (k < Route.RoutePoints.Length)
                        {
                            CurrentTarget = Route.RoutePoints[k];
                        }
                        else
                        {
                            if (!IsLoop)
                            {
                                k -= 2;
                                direction = false;
                                CurrentTarget = Route.RoutePoints[k];
                            }
                            else
                            {
                                k = 0;
                                CurrentTarget = Route.RoutePoints[k];
                            }
                        }
                    }
                    //negative
                    else
                    {
                        k--;
                        //get new one
                        if (k >= 0)
                        {
                            CurrentTarget = Route.RoutePoints[k];
                        }
                        else
                        {
                            if (!IsLoop)
                            {
                                k += 2;
                                direction = true;
                                CurrentTarget = Route.RoutePoints[k];
                            }
                            else
                            {
                                k = Route.RoutePoints.Length - 1;
                                CurrentTarget = Route.RoutePoints[k];
                            }
                        }
                    }

                    //wait, move again
                    IsExecute = false;
                    _Animator.CrossFade("Base Layer.Idle", 0.9f);
                    StartCoroutine(TimeOutCorot(UnityEngine.Random.Range(1.3f, 5.0f)));
                }
            }
        }
    }

    //fast fake distance:
    bool FastDistance(Transform Self, Transform Target, float Radius)
    {
        bool Xpass = false;
        bool Zpass = false;
        bool Ypass = false;

        //x
        if ((Self.position.x >= 0 & Target.position.x >= 0) | (Self.position.x < 0 & Target.position.x < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.position.x) - Mathf.Abs(Target.position.x)) < Radius) Xpass = true;
        }
        else
        {
            if (Mathf.Abs(Self.position.x) + Mathf.Abs(Target.position.x) < Radius) Xpass = true;
        }

        //y
        if ((Self.position.y >= 0 & Target.position.y >= 0) | (Self.position.y < 0 & Target.position.y < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.position.y) - Mathf.Abs(Target.position.y)) < Radius) Ypass = true;
        }
        else
        {
            if (Mathf.Abs(Self.position.y) + Mathf.Abs(Target.position.y) < Radius) Ypass = true;
        }

        //z
        if ((Self.position.z >= 0 & Target.position.z >= 0) | (Self.position.z < 0 & Target.position.z < 0))
        {
            if (Mathf.Abs(Mathf.Abs(Self.position.z) - Mathf.Abs (Target.position.z)) < Radius) Zpass = true;
        }
        else
        {
            if (Mathf.Abs(Self.position.z) + Mathf.Abs(Target.position.z) < Radius) Zpass = true;
        }

        if (Xpass & Zpass & Ypass) return true;
        else return false;
    }

    IEnumerator TimeOutCorot(float rTime)
    {
        yield return new WaitForSeconds(rTime);
        _Animator.CrossFade("Base Layer.Move", 0.5f);
        IsExecute = true;
    }

}
