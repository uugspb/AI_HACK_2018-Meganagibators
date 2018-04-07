using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Черезе сколько спавнов получим поинты")]
    public int SpawnsToAward;
    
    [Header("Сколько авард поинтов")]
    public int AwardPoints;
    
    private List<NPC> npcs = new List<NPC>();
    
    private static GameController _instance;

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
        var tower = Tower.Instance;
        while (spawnEnemes <= spawner.SpawnRate)
        {
            var npc = spawner.SpawnBot();
            npcs.Add(npc);

            var moveTime = npc.model.Speed;
            LeanTween.move(npc.gameObject, tower.transform.position, moveTime);
            spawnEnemes++;
            yield return new WaitForSeconds(partTime);
        }
    }

    public void Damage()
    {
        //var npc = npcs[npcs.Count];
        //npc.model.HP
    }
}
