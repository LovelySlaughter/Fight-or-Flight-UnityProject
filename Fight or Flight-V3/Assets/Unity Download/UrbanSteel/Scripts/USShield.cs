using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, ILonion32@gmail.com, 2021.
public class USShield : MonoBehaviour
{
    Animator _Animator;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        StartCoroutine(TimeOutCorot(UnityEngine.Random.Range(0.3f,3.0f)));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TimeOutCorot(float rTime)
    {
        yield return new WaitForSeconds(rTime);
        _Animator.CrossFade("Base Layer.ShieldState", 0.1f);
    }
}
