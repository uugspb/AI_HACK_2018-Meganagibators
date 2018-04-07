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
    public Transform StartPoint2;

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
        var randY = Random.Range(StartPoint.position.y, StartPoint2.position.y);
        npc.transform.position = new Vector3(StartPoint.position.x, randY);

        npc.model = npcModel;

        return npc;
    }
}
