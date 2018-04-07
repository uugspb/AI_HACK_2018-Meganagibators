public class Variation
{
    // пусть будут паблик статик, чтоб если что был доступ через какую-нибудь дебажную тулзу
    // (если конечно до нее дойдет дело)
    #region static
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
    public int hpLevel;
    public int armorLevel;
    public int speedLevel;
    public int damageLevel;
    public int fireRateLevel;
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
