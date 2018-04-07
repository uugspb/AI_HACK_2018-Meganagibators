using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Черезе сколько спавнов получим поинты")]
    public int SpawnsToAward;
    
    [Header("Сколько авард поинтов")]
    public int AwardPoints;
    
    [Header("Пауза в секундах")]
    public int PauseTime;

    private int wavesCount;
    
    private List<NPC> npcs = new List<NPC>();
    private List<VariationStats> variationStats = new List<VariationStats>();
    
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
        wavesCount = 0;
        Tower.Instance.Initialize();
        Tower.Instance.OnDead += TowerDead;
    }

    private void TowerDead()
    {
        
    }

    public void StartWave()
    {
        variationStats.Clear();
        wavesCount++;
        
        var spawner = Spawner.Instance;
        StartCoroutine(StartWaveCour(spawner));
    }

    IEnumerator StartWaveCour(Spawner spawner)
    {
        if (wavesCount % SpawnsToAward == 0)
        {
            LevelUp(AwardPoints);
            yield return new WaitForSeconds(PauseTime);
        }
        var spawnEnemes = 0;
        var partTime = spawner.SpawnTime / spawner.SpawnRate;
        var tower = Tower.Instance;
        while (spawnEnemes <= spawner.SpawnRate)
        {
            var npc = spawner.SpawnBot();
            npcs.Add(npc);

            var moveTime = 1/npc.model.Speed;
            LeanTween.moveX(npc.gameObject, tower.transform.position.x, moveTime)
                .setOnComplete(() =>
                {
                    NPCDamageStart(npc);
                    npc.NearTower();
                });
            spawnEnemes++;
            yield return new WaitForSeconds(partTime);
        }
    }

    private void NPCDamageStart(NPC npc)
    {
        StartCoroutine(DamageTower(npc));
    }

    IEnumerator DamageTower(NPC npc)
    {
        npc.DamageDealt += npc.model.Damage;
        Tower.Instance.Damage(npc.model.Damage);
        yield return new WaitForSeconds(npc.model.FireRate);
    }

    private void LevelUp(int skillPoints)
    {
        Tower.Instance.LevelUp(skillPoints);
        GeneticsController.Instance.OnPlayerLevelUp();
    }

    public void Damage(GunModel model)
    {
        if (npcs.Count == 0)
        {
            Debug.LogError("WrongDamage");
            return;
        }
        
        var npc = npcs[0];
        npc.model.HP -= model.Damage;
        
        Debug.Log("damage " + npc.model.HP + " | " + model.Damage);
        
        if (npc.model.HP <= 0)
        {
            // дистанция
            var fullDistance = Vector3.Distance(Spawner.Instance.StartPoint.position, Tower.Instance.transform.position);
            var distance = Vector3.Distance(npc.transform.position, Tower.Instance.transform.position);
            var distancePassed = distance / fullDistance;

            var variationStat = new VariationStats(distancePassed, npc.DamageDealt);
           
            variationStats.Add(variationStat);
            npcs.RemoveAt(0);
            npc.Kill();
            Debug.Log("destroy");
            if (npcs.Count == 0)
            {
                GeneticsController.Instance.OnVariationDied(variationStats.ToArray());
                StartWave();
            }
        } 
    }

    public void OnStartClick()
    {
        Tower.Instance.StartGame();
        StartWave();
    }
}
