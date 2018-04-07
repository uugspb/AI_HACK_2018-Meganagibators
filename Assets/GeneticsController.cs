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
    #endregion

    #region inspector fields
    [Range(2, int.MaxValue)]
    public int parentsAmount = 2;

    public float baseBotSkillPoints = 20.0f;
    public float levelUpSkillPoints = 20.0f;
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
        // TODO: пока генетика не готова, пусть возвращается какая-то базовая ерунда
        return (new Variation(0, 0, 0, 0, 0, 0)).ToNPCModel();
        //lastGivenVariation = currentBestVariation;
        //return lastGivenVariation.ToNPCModel();
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

        // TODO собственно говоря начислить скиллпоинтов боту
    }

    public void OnPlayerLevelUp()
    {
        // добавляем боту столько скилллпоинтов, сколько было получено игроком
        currentBotSkillPoints += levelUpSkillPoints;
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
        return stats.distancePassed + stats.damageDealt;
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
