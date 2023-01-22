using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, ILonion32@gmail.com, 2021.
public class USFlash : MonoBehaviour 
{
    [Header("type: _EmissiveColor for hdrp usage")]
    public string EmissiveString = "_EmissionColor";
    public bool IsActive;
    [ColorUsageAttribute (true, true)]
    public Color MaxBright;
    public int BoostBright = 1;
    public float SwitchTime = 0.1f;
    public enum SwitchMode
    {
        simple = 0,
        multi = 1,
        broken = 2,
        stat = 3,
    }
    public SwitchMode SwitchModes = SwitchMode.simple;
    public int MatId = 1;
    int mode;
    int count;
    Material _Material;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

	// Use this for initialization
	void Start () 
    {
        if (IsActive)
        {
            float random = Random.Range(0.07f, 0.95f);
            InvokeRepeating("ColorSwitch", random, SwitchTime);
            _Material = GetComponent<Renderer>().materials[MatId];

            MaxBright = MaxBright * BoostBright;
        }

	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void ColorSwitch()
    {
        //0 - 1 - 0 - 1 - 0...
        if (SwitchModes == SwitchMode.simple)
        {
            mode = 1 - mode;
            if (mode == 0)
            {
                _Material.SetColor(EmissiveString, Color.black);
            }
            else
            {
                _Material.SetColor(EmissiveString, MaxBright);
            }
        }

        //0 - 0.5 - 1 - 0...
        else if (SwitchModes == SwitchMode.multi)
        {
            mode++;
            if (mode > 2) mode = 0;
            if (mode == 0)
            {
                _Material.SetColor(EmissiveString, Color.black);
            }
            else if (mode == 1)
            {
                _Material.SetColor(EmissiveString, MaxBright * 0.5f);
            }
            else
            {
                _Material.SetColor(EmissiveString, MaxBright);
            }
        }

        //broken lamp
        else
        {
            if (count < 10)
            {
                mode++;
                if (mode > 2) mode = 0;
                if (mode == 0)
                {
                    _Material.SetColor(EmissiveString, Color.black);
                }
                else if (mode == 1)
                {
                    _Material.SetColor(EmissiveString, MaxBright * 0.5f);
                }
                else
                {
                    _Material.SetColor(EmissiveString, MaxBright);
                }
            }
            else if (count < 20)
            {
                _Material.SetColor(EmissiveString, MaxBright * 0.5f);
            }
            else
            {
                _Material.SetColor(EmissiveString, MaxBright);
            }
            count++;
            if (count > 50) count = 0;
        }

        if (SwitchModes == SwitchMode.stat)
        {
            _Material.SetColor(EmissiveString, MaxBright);
        }
    }
}
