using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USLighting : MonoBehaviour
{
    ParticleSystem _Particle;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

    // Start is called before the first frame update
    void Start()
    {
        _Particle = GetComponent<ParticleSystem>();
        var emission = _Particle.emission;
        emission.enabled = false;
        StartCoroutine(TimeOutCorot(UnityEngine.Random.Range(0.3f, 3.0f)));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TimeOutCorot(float rTime)
    {
        yield return new WaitForSeconds(rTime);
        var emission = _Particle.emission;
        emission.enabled = true;
    }
}
