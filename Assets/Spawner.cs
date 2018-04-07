using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> NPCprefabs;

    [Header("Сколько выпускаем")]
    public int SpawnRate;
    
    [Header("Сколько по времени")]
    public int SpawnTime;
    
    [Header("Точка спавна npc")]
    public Transform StartPoint;
    
    private static Spawner _instance;

    public static Spawner Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Spawner>();
            return _instance;
        }
    }
    
    public NPC SpawnBot()
    {
        var gen = GeneticsController.Instance;
        var npcModel = gen.GetModel();

        var randPrefab = NPCprefabs[Random.Range(0, NPCprefabs.Count)];
        var npc = Instantiate(randPrefab).GetComponent<NPC>();
        npc.transform.position = StartPoint.position;

        npc.model = npcModel;

        return npc;
    }
}
