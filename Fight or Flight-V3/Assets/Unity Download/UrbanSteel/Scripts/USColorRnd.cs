using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IL.ranch, ILonion32@gmail.com, 2021.
public class USColorRnd : MonoBehaviour
{
    [Header("type: _EmissiveColor for hdrp usage")]
    public string EmissiveString = "_EmissionColor";
    public bool IsActive;
    public int MatId = 0;
    public bool IsMatLod;
    [ColorUsageAttribute(true, true)]
    public Color ColorVar1;
    [ColorUsageAttribute(true, true)]
    public Color ColorVar2;
    [ColorUsageAttribute(true, true)]
    public Color ColorVar3;

    Material _Material;
    Material _MaterialLod;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

    // Start is called before the first frame update
    void Start()
    {
        _Material = GetComponent<Renderer>().materials[MatId];
        if(IsMatLod) _MaterialLod = transform.parent.GetChild(1).GetComponent<Renderer>().materials[MatId];

        int _random = UnityEngine.Random.Range(0, 100);
        if (_random > 66)
        {
            _Material.SetColor(EmissiveString, ColorVar1);
            if (IsMatLod) _MaterialLod.SetColor(EmissiveString, ColorVar1);
        }
        else if (_random > 33)
        {
            _Material.SetColor(EmissiveString, ColorVar2);
            if (IsMatLod) _MaterialLod.SetColor(EmissiveString, ColorVar2);
        }
        else
        {
            _Material.SetColor(EmissiveString, ColorVar3);
            if (IsMatLod) _MaterialLod.SetColor(EmissiveString, ColorVar3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
