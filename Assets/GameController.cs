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

    [Header("Префаб Патрона для стрельбы")]
    public GameObject Bullet;
    public float bulletSpeed = 0.1f;

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
            LeanTween.moveX(npc.gameObject, tower.bulletStartPlace.transform.position.x, moveTime)
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

        var bullet = Instantiate(Bullet);
        bullet.transform.position = Tower.Instance.bulletStartPlace.position;
        var npc = npcs[0];
        var distanceBullet = Vector3.Distance(bullet.transform.position, npc.transform.position);
        LeanTween.move(bullet, npc.transform.position, distanceBullet / bulletSpeed)
            .setOnComplete(() => 
            {
                BulletFlowen(npc, model, bullet);
            });   
    }
    
    private void BulletFlowen(NPC npc, GunModel model, GameObject bullet)
    {
        Destroy(bullet);
        npc.model.HP -= model.Damage;

        if (npc.model.HP <= 0)
        {
            // дистанция
            var startPosition = Tower.Instance.bulletStartPlace.transform.position;
            var fullDistance = Vector3.Distance(Spawner.Instance.StartPoint.position, startPosition);
            var distance = Vector3.Distance(npc.transform.position, startPosition);
            var distancePassed = distance / fullDistance;

            var variationStat = new VariationStats(distancePassed, npc.DamageDealt);

            variationStats.Add(variationStat);
            npcs.RemoveAt(0);
            npc.Kill();
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
