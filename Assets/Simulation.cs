using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation
{
    public static VariationStats[] Simulate(Variation variation)
    {
        List<VariationStats> stats = new List<VariationStats>();
        var spawnCount = Spawner.Instance.SpawnRate;
        var userDamage = Tower.Instance.gun;
        var spawnDelta = Spawner.Instance.SpawnTime / spawnCount;
        float attack = 0.0f;
        for (int i = 0; i < spawnCount; i++)
        {
            float killtime;
            stats.Add(CalcStats(variation, i * spawnDelta, attack, userDamage, out killtime));
            attack += killtime;
        }

        return stats.ToArray();
    }
    
    private static VariationStats CalcStats(Variation variation,float spawnTime, float atackTime, GunModel gun, out float killTime)
    {
        killTime = Mathf.Ceil(variation.HPValue / gun.Damage) * gun.fireRate;
        var runTime = 1 / variation.SpeedValue + spawnTime;
        var lifetime = atackTime + killTime;
        var npcFightTime = lifetime - runTime;
        var damage = npcFightTime > 0? Mathf.FloorToInt(npcFightTime / variation.FireRateValue) * variation.DamageValue: 0;
        var distance = atackTime + killTime >= runTime ? 1 : 1 / variation.SpeedValue / (1 / variation.SpeedValue - ((runTime) - (atackTime + killTime)));
        return new VariationStats(distance, damage);
    }
}
