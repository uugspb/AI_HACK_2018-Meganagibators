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

    private void Start()
    {
        global::Tower.Instance.Initialize();
        global::Tower.Instance.OnDead += TowerDead;
    }

    private void TowerDead()
    {
        throw new System.NotImplementedException();
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
        var tower = global::Tower.Instance;
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

    private void LevelUp(int skillPoints)
    {
        Tower.Instance.LevelUp(skillPoints);
    }

    public void Damage(GunModel model)
    {
        var npc = npcs[npcs.Count - 1];
        npc.model.HP -= model.Damage;
        if (npc.model.HP <= 0)
        {
            npcs.RemoveAt(npcs.Count - 1);
            Destroy(npc.gameObject);
            if (npcs.Count == 0)
            {
                
            }
        } 
    }

    public void OnStartClick()
    {
        global::Tower.Instance.StartGame();
        StartWave();
    }
}
