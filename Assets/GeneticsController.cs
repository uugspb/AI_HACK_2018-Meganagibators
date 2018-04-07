using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticsController : MonoBehaviour
{
    #region helper classes
    public class Population
    {
        public Variation[] parents;
        public Variation[] variations;
    }

    public class Variation
    {
        // пусть будут паблик статик, чтоб если что был доступ через какую-нибудь дебажную тулзу
        // (если конечно до нее дойдет дело)
        #region static
        public static float hpValueByLevel = 1.0f;
        public static float armorValueByLevel = 1.0f;
        public static float speedValueByLevel = 1.0f;
        public static float damageValueByLevel = 1.0f;
        public static float fireRateValueByLevel = 1.0f;
        public static float spawnRateValueByLevel = 1.0f;
        #endregion

        #region fields
        public int hpLevel;
        public int armorLevel;
        public int speedLevel;
        public int damageLevel;
        public int fireRateLevel;
        public int spawnRateLevel;
        
        public float fitnessValue;
        #endregion

        #region propeties
        public float HPValue { get { return hpValueByLevel * hpLevel; } }
        public float ArmorValue { get { return armorValueByLevel * armorLevel; } }
        public float SpeedValue { get { return speedValueByLevel * speedLevel; } }
        public float DamageValue { get { return damageValueByLevel * damageLevel; } }
        public float FireRateValue { get { return fireRateValueByLevel * fireRateLevel; } }
        public float SpawnRateValue { get { return spawnRateValueByLevel * spawnRateLevel; } }
        #endregion

        #region public methods
        public Variation(int hpLevel, int armorLevel, int speedLevel, int damageLevel, int fireRateLevel, int spawnRateLevel)
        {
            this.hpLevel = hpLevel;
            this.armorLevel = armorLevel;
            this.speedLevel = speedLevel;
            this.damageLevel = damageLevel;
            this.fireRateLevel = fireRateLevel;
            this.spawnRateLevel = spawnRateLevel;

            fitnessValue = float.NaN;
        }

        public NPCModel ToNPCModel()
        {
            return new NPCModel(HPValue, ArmorValue, SpeedValue, DamageValue, FireRateValue, SpawnRateValue);
        }
        #endregion
    }
    #endregion

    #region inspector fields
    [Range(2, int.MaxValue)]
    public int parentsAmount = 2;
    #endregion

    #region fields
    private Variation currentBestVariation;
    private Variation lastGivenVariation;
    
    private float currentBotSkillPoints;
    #endregion

    #region properties
    private static GeneticsController _instance;

    public static GeneticsController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GeneticsController>();
            return _instance;
        }
    }

    public int CurrentEffectiveBotSkillPoints { get { return Mathf.FloorToInt(currentBotSkillPoints); } }
    #endregion

    #region public methods
    public NPCModel GetModel()
    {
        lastGivenVariation = currentBestVariation;
        return lastGivenVariation.ToNPCModel();
    }

    public void OnVariationDied(VariationStats[] stats)
    {
        // TODO считаем оценку по стате, при очень низкой (или при сильно меньшей, чем была посчитана)
        // даем боту бонусные скиллпоинты (возможно бонусные скиллпоинты должны зависить от разницы между оценками)

        // так как приходят статы по каждому отдельному мобу, считаем среднее
        VariationStats avg = new VariationStats(0.0f, 0.0f);
        for (int i = 0; i < stats.Length; i++)
        {
            avg.damageDealt += stats[i].damageDealt;
            avg.distancePassed += stats[i].distancePassed;
        }

        avg.damageDealt /= (float)stats.Length;

        float fitnessValue = CalcFitnessValue(avg);

        // TODO собственно говоря начислить скиллпоинтов бот
    }

    public void OnPlayerLevelUp(int receivedSkillPoints)
    {
        // добавляем боту столько скилллпоинтов, сколько было получено игроком
        currentBotSkillPoints += receivedSkillPoints;
    }
    #endregion

    #region private methods
    private NPCModel[] GetFirstPopulation()
    {
        // TODO
        return null;
    }
    
    /// <summary>
    /// Оценка эффективности путем симуляции (0 - все очень плохо)
    /// </summary>
    /// <param name="variation"></param>
    private void CalcFitnessValue(Variation variation)
    {
        // TODO провести симуляцию
        variation.fitnessValue = 0.0f;
    }

    /// <summary>
    /// Оценка эффективности по результатам применения вариации
    /// </summary>
    /// <param name="stats"></param>
    /// <returns>0 - все очень плохо (ничего не пройденно и не задамажено)</returns>
    private float CalcFitnessValue(VariationStats stats)
    {
        // TODO коэффициенты?
        return stats.distancePassed + stats.damageDealt ;
    }

    private void CalcFitnessValues(Population population)
    {
        Variation[] variations = new Variation[population.variations.Length];
        for (int i = 0; i < population.variations.Length; i++)
            CalcFitnessValue(variations[i]);
    }

    private Variation[] SelectParents(Population population)
    {
        int maxParents = parentsAmount < population.variations.Length ? parentsAmount : population.variations.Length;
        Variation[] retval = new Variation[maxParents];

        CalcFitnessValues(population);

        List<Variation> variations = new List<Variation>(population.variations);
        for (int i = 0; i < maxParents; i++)
        {
            int maxInd = 0;
            float maxFitness = float.MinValue;
            for (int j = 0; j < variations.Count; j++)
            {
                Variation variation = variations[j];
                if (variation.fitnessValue > maxFitness)
                {
                    maxFitness = variation.fitnessValue;
                    maxInd = j;
                }
            }

            retval[i] = variations[maxInd];
            variations.RemoveAt(maxInd);
        }

        return retval;
    }

    private Population CrossoverParents(Variation[] parents)
    {
        // TODO
        return null;
    }

    private void ApplyMutation(Population population)
    {
        // TODO

    }
    #endregion
}
