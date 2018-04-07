using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    private static GameController _instance;
    
    private List<NPC> npcs = new List<NPC>();

    public static GameController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameController>();
            return _instance;
        }
    }

    public void StartWave()
    {
        var spawner = Spawner.Instance;
        StartCoroutine(StartWaveCour(spawner));
    }

    IEnumerator StartWaveCour(Spawner spawner)
    {
        var spawnEnemes = 0;
        var partTime = spawner.SpawnTime / spawner.SpawnRate;
        while (spawnEnemes <= spawner.SpawnRate)
        {
            npcs.Add(spawner.SpawnBot());
            spawnEnemes++;
            yield return new WaitForSeconds(partTime);
        }
    }

    public void Damage()
    {
        
    }
}
