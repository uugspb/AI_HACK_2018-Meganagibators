using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticsController : MonoBehaviour
{
    private static GeneticsController _instance;

    public static GeneticsController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GeneticsController>();
            return _instance;
        }
    }

    public NPCModel GetModel()
    {
        // TODO
        return null;
    }

    public void OnVariationDied(VariationStats stats)
    {
        // TODO
    }
}
