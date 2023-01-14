using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicWeapon : MonoBehaviour
{
    [Header("--- Gun Components ---")]
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject bullets;
    [SerializeField] int rateOfFire;
    [SerializeField] int damage;
    [SerializeField] int shootRange;

    public bool automatic;
    public bool burst;
    public int burstTime;
    public int burstRate;
    public bool scoped;
    public int scopedTime;
    bool ADS;
    public int swayRate;


    int magazine;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
