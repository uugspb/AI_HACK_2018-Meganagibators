using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Variation
{
    public class AllowedToMutate: Attribute
    {

    }

    // пусть будут паблик статик, чтоб если что был доступ через какую-нибудь дебажную тулзу
    // (если конечно до нее дойдет дело)
    #region static
    private static Random rngesus = new Random();    

    public static float hpBaseValue = 10.0f;
    /// <summary>
    /// Судьба армора пока что неясна, поэтому пусть будет 0
    /// </summary>
    public static float armorBaseValue = 0.0f;
    public static float speedBaseValue = 0.2f;
    public static float damageBaseValue = 5.0f;
    public static float fireRateBaseValue = 0.5f;
    public static float spawnRateBaseValue = 1.0f;

    public static float hpValueByLevel = 3.0f;
    /// <summary>
    /// Судьба армора пока что неясна, поэтому пусть будет 0
    /// </summary>
    public static float armorValueByLevel = 0.0f;
    public static float speedValueByLevel = 0.04f;
    public static float damageValueByLevel = 1.0f;
    public static float fireRateValueByLevel = 0.1f;
    public static float spawnRateValueByLevel = 0.02f;
    #endregion

    #region fields
    [AllowedToMutate]
    public int hpLevel;
    public int armorLevel;
    [AllowedToMutate]
    public int speedLevel;
    [AllowedToMutate]
    public int damageLevel;
    [AllowedToMutate]
    public int fireRateLevel;
    [AllowedToMutate]
    public int spawnRateLevel;

    public float fitnessValue;
    #endregion

    #region propeties
    public float HPValue { get { return hpBaseValue + hpValueByLevel * hpLevel; } }
    public float ArmorValue { get { return armorBaseValue + armorValueByLevel * armorLevel; } }
    public float SpeedValue { get { return speedBaseValue + speedValueByLevel * speedLevel; } }
    public float DamageValue { get { return damageBaseValue + damageValueByLevel * damageLevel; } }
    public float FireRateValue { get { return fireRateBaseValue + fireRateValueByLevel * fireRateLevel; } }
    public float SpawnRateValue { get { return spawnRateBaseValue + spawnRateValueByLevel * spawnRateLevel; } }

    public bool IsValid
    {
        get
        {
            return hpLevel >= 0
                && armorLevel >= 0
                && speedLevel >= 0
                && damageLevel >= 0
                && fireRateLevel >= 0
                && spawnRateLevel >= 0;
        }
    }
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

    public Variation(Variation from) : this(from.hpLevel, from.armorLevel, from.speedLevel, from.damageLevel, from.fireRateLevel, from.spawnRateLevel) { }

    public Variation Mutate(int maxDelta, int compensationTargetsAmount, int maxSkillLevel)
    {
        List<FieldInfo> fields = typeof(Variation).GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Where(field => field.IsDefined(typeof(AllowedToMutate), false))
            .ToList();
        
        bool add = false;
        int randomTargetInd = 0;
        int randomDeltaValue = 0;

        int[] randomCompensatingIndexes = new int[compensationTargetsAmount];
        int[] randomCompensatingValues = new int[compensationTargetsAmount];

        int sum = 0;
        for (int i = 0; i < fields.Count; i++)
            sum += (int)fields[i].GetValue(this);

        lock (rngesus)
        {
            // случайно выбранное поле, которое мы будем менять
            randomTargetInd = rngesus.Next(0, fields.Count);
            // на сколько мы его будем менять
            randomDeltaValue = rngesus.Next(1, maxDelta + 1);
            // +/-
            add = rngesus.Next(0, 2) == 0;
            // остальные случайно выбранные поля, которые будут изменены на обратное значение
            for (int i = 0; i < compensationTargetsAmount; i++)
                try
                {
                    randomCompensatingIndexes[i] = rngesus.Next(0, fields.Count - i - 1);
                }
                catch (Exception e)
                {
                    throw e;
                }
            
            for (int i = 0, compensationSum = randomDeltaValue; compensationSum != 0; i = (i + 1) % compensationTargetsAmount)
            {
                int compensationValue = rngesus.Next(1, compensationSum + 1);
                compensationSum -= compensationValue;
                if (add)
                    randomCompensatingValues[i] -= compensationValue;
                else
                    randomCompensatingValues[i] += compensationValue;
            }
        }

        if (!add)
            randomDeltaValue *= -1;

        // к моменту мутации количество скиллпоинтов могло увеличиться, 
        // для простоты пусть разница пойдет к изменению целевого поля
        randomDeltaValue += maxSkillLevel - sum;
        
        FieldInfo targetField = fields[randomTargetInd];
        fields.RemoveAt(randomTargetInd);
        
        Variation mutated = new Variation(this);
        targetField.SetValue(mutated, (int)targetField.GetValue(mutated) + randomDeltaValue);

        for (int i = 0; i < compensationTargetsAmount; i++)
        {
            FieldInfo f = fields[randomCompensatingIndexes[i]];
            fields.RemoveAt(randomCompensatingIndexes[i]);
            f.SetValue(mutated, (int)f.GetValue(mutated) + randomCompensatingValues[i]);
        }

        // а теперь проверяем что мутант не мертворожденный (не должно быть отрицательных статов)
        if (!mutated.IsValid)
            return null;

        return mutated;
    }

    public NPCModel ToNPCModel()
    {
        return new NPCModel(HPValue, ArmorValue, SpeedValue, DamageValue, FireRateValue, SpawnRateValue);
    }
    #endregion
}
