using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticsController : MonoBehaviour
{
    #region consts 
    public const int MIN_PARENTS_AMOUNT = 1;
    public const int MAX_PARENTS_AMOUNT = 300;

    public const int MIN_MUTATIONS_PER_PARENT = 1;
    public const int MAX_MUTATIONS_PER_PARENT = 300;

    public const int MIN_ALLOWED_MUTATION_VALUE = 1;
    public const int MAX_ALLOWED_MUTATION_VALUE = 20;

    public const int MIN_ALLOWED_COMPENSATION_FIELDS = 1;
    public const int MAX_ALLOWED_COMPENSATION_FIELDS = 4;
    #endregion

    #region helper classes
    public class GeneticsStateObject
    {
        public bool stop = false;
        public Population currentPopulation;
    }
    #endregion

    #region inspector fields
    [Range(MIN_PARENTS_AMOUNT, MAX_PARENTS_AMOUNT)]
    public int parentsAmount = 15;

    [Range(MIN_MUTATIONS_PER_PARENT, MAX_MUTATIONS_PER_PARENT)]
    public int mutationsPerParent = 30;

    [Range(MIN_ALLOWED_MUTATION_VALUE, MAX_ALLOWED_MUTATION_VALUE)]
    public int maxMutationValue = 5;

    [Range(MIN_ALLOWED_COMPENSATION_FIELDS, MAX_ALLOWED_COMPENSATION_FIELDS)]
    public int maxCompensationFields = MAX_ALLOWED_COMPENSATION_FIELDS;

    public float baseBotSkillPoints = 20.0f;
    public float levelUpSkillPoints = 20.0f;
    #endregion

    #region fields
    private System.Random rngesus = new System.Random();

    private object currentBestLock = new object();

    private Variation currentBestVariation;
    private Variation lastGivenVariation;
    private float prevLastGivenVariationFitness = float.NaN;
    
    private float currentBotSkillPoints;

    private GeneticsStateObject geneticsState = new GeneticsStateObject();
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
    public void ActivateGenetics()
    {
        if (geneticsState.currentPopulation != null)
        {
            ResumeGenetics();
            return;
        }

        FindObjectOfType<UserSettings>().OnParamsChanged += OnUserSkillIncreased;

        currentBotSkillPoints = baseBotSkillPoints;
        prevLastGivenVariationFitness = float.NaN;

        Population firstPopulation = GetFirstPopulation();

        CalcFitnessValues(firstPopulation);
        float maxFitnessValue = float.MinValue;
        for (int i = 0; i < firstPopulation.variations.Length; i++)
            if (firstPopulation.variations[i].fitnessValue > maxFitnessValue)
            {
                currentBestVariation = firstPopulation.variations[i];
                maxFitnessValue = currentBestVariation.fitnessValue;
            }

        geneticsState.currentPopulation = firstPopulation;
        geneticsState.stop = false;

        System.Threading.ThreadPool.QueueUserWorkItem(CalculateGenetics, geneticsState);
    }
    
    public void ResumeGenetics()
    {
        if (geneticsState.currentPopulation == null)
        {
            ActivateGenetics();
            return;
        }

        geneticsState.stop = false;
        System.Threading.ThreadPool.QueueUserWorkItem(CalculateGenetics, geneticsState);
    }

    public void StopGenetics()
    {
        geneticsState.stop = true;
    }

    public NPCModel GetModel()
    {
        lock (currentBestLock)
        {
            lastGivenVariation = currentBestVariation;
        }

        return lastGivenVariation.ToNPCModel();
    }

    public void OnVariationDied(VariationStats[] stats)
    {
        float statsFitness = CalcAvgFitnessValue(stats);
        float simFitness = lastGivenVariation.fitnessValue;
        float prevFitness = float.IsNaN(prevLastGivenVariationFitness) ? statsFitness : prevLastGivenVariationFitness;

        float simQualityGrade = Mathf.Clamp(statsFitness - simFitness, 0.0f, 1.0f);
        float degradeGrade = Mathf.Clamp(prevFitness - statsFitness, 0.0f, 1.0f);

        // бонусные скиллпоинты начисляем, если реальный результат оказался хуже, чем симуляция,
        // или хуже, чем предыдущий результат
        float bonusGrade = 0.3f * simQualityGrade + 0.7f * degradeGrade;

        // 0.5 чтоб не так резко возрастало, 
        // при введении бустов можно увеличить скорость роста
        // с мыслью, что это будет компенсировано бустами
        currentBotSkillPoints += 0.5f * bonusGrade;

        prevLastGivenVariationFitness = statsFitness;
        
        // TODO юзер может вкачать что-нибудь пока волна активна,
        // соответственно полученная здесь оценка будет не очень адекватна, 
        // потому что часть мобов получит стату со старой прокачкой, а часть с новой
    }

    public void OnPlayerLevelUp()
    {
        currentBotSkillPoints += levelUpSkillPoints;
    }

    public void OnUserSkillIncreased()
    {
        // пересчитываем, потому что с новыми скиллами оценка должна стать хуже
        lock (currentBestLock)
        {
            CalcFitnessValue(currentBestVariation);
        }
    }
    #endregion

    #region private methods
    private void Start()
    {
        ActivateGenetics();
    }

    private void CalculateGenetics(object geneticsStateObject)
    {
        GeneticsStateObject stateObject = geneticsStateObject as GeneticsStateObject;
        CalcFitnessValues(stateObject.currentPopulation);

        while (!stateObject.stop)
        {
            // вместо скрещеваний будут почкующиеся мутанты
            stateObject.currentPopulation = ApplyMutation(SelectParents(stateObject.currentPopulation));
            CalcFitnessValues(stateObject.currentPopulation);
            Variation newBest = stateObject.currentPopulation.GetBestVariation();
            if (newBest != null && newBest.fitnessValue > currentBestVariation.fitnessValue)
            {
                lock (currentBestLock)
                {
                    currentBestVariation = newBest;
                }
            }
        }
    }

    private Population GetFirstPopulation()
    {
        // ну шоб наверняка
        Variation baseVariation = new Variation(CurrentEffectiveBotSkillPoints / 5 + CurrentEffectiveBotSkillPoints % 5,
            0, CurrentEffectiveBotSkillPoints / 5,
            CurrentEffectiveBotSkillPoints / 5, CurrentEffectiveBotSkillPoints / 5, CurrentEffectiveBotSkillPoints / 5);

        int amount = parentsAmount * mutationsPerParent;
        List<Variation> variations = new List<Variation>(amount);
        variations.Add(baseVariation);
        
        for (int i = 1; i < amount; i++)
        {
            Variation variation = baseVariation.Mutate(rngesus.Next(1, maxMutationValue + 1), rngesus.Next(1, maxCompensationFields + 1), CurrentEffectiveBotSkillPoints);
            if (variation == null)
                continue;
            variations.Add(variation);
        }

        return new Population(variations.ToArray(), new Variation[] { baseVariation });
    }
    
    /// <summary>
    /// Оценка эффективности путем симуляции (0 - все очень плохо)
    /// </summary>
    /// <param name="variation"></param>
    private void CalcFitnessValue(Variation variation)
    {
        variation.fitnessValue = CalcAvgFitnessValue(Simulation.Simulate(variation));
    }

    /// <summary>
    /// Оценка эффективности по результатам применения вариации
    /// </summary>
    /// <param name="stats"></param>
    /// <returns>0 - все очень плохо (ничего не пройденно и не задамажено)</returns>
    private float CalcFitnessValue(VariationStats stats)
    {
        // пройденная дистанция и так в [0, 1]
        float distanceGrade = stats.distancePassed;
        // оценку дамага считаем как снятую часть жизней игрока
        float damageGrade = stats.damageDealt / Tower.Instance.playerBase.MaxHealth;
        
        return 0.2f * distanceGrade + 0.8f * damageGrade;
    }

    private void CalcFitnessValues(Population population)
    {
        for (int i = 0; i < population.variations.Length; i++)
            CalcFitnessValue(population.variations[i]);
    }

    private float CalcAvgFitnessValue(VariationStats[] variationStats)
    {
        // так как приходят статы по каждому отдельному мобу, считаем среднее
        VariationStats avg = new VariationStats(0.0f, 0.0f);
        for (int i = 0; i < variationStats.Length; i++)
        {
            avg.damageDealt += variationStats[i].damageDealt;
            avg.distancePassed += variationStats[i].distancePassed;
        }

        avg.damageDealt /= (float)variationStats.Length;

        return CalcFitnessValue(avg);
    }

    private Variation[] SelectParents(Population population)
    {
        int maxParents = parentsAmount < population.variations.Length ? parentsAmount : population.variations.Length;
        Variation[] retval = new Variation[maxParents];

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

    private Population ApplyMutation(Variation[] candidates)
    {
        List<Variation> mutations = new List<Variation>(candidates.Length * mutationsPerParent);
        for (int i = 0; i < candidates.Length; i++)
        {
            for (int j = 0; j < mutationsPerParent; j++)
            {
                lock (rngesus)
                {
                    Variation mutation = candidates[i].Mutate(rngesus.Next(1, maxMutationValue + 1), rngesus.Next(1, maxCompensationFields + 1), CurrentEffectiveBotSkillPoints);
                    if (mutation == null)
                        continue;

                    mutations.Add(mutation);
                }
            }
        }

        return new Population(mutations.ToArray(), candidates);
    }

    private void OnDestroy()
    {
        StopGenetics();
    }
    #endregion
}
