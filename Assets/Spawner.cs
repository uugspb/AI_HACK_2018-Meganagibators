using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject NPCprefab;

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

        var npc = Instantiate(NPCprefab).GetComponent<NPC>();
        npc.transform.position = StartPoint.position;

        npc.model = npcModel;

        return npc;
    }
}
