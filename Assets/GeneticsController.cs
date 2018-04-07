using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticsController : MonoBehaviour
{
    private static Spawner _instance;

    public static Spawner Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Spawner>();
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
