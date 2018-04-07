﻿public class VariationStats
{
    /// <summary>
    /// 0..1 
    /// 0 - нифига не пройдено, 
    /// 1 - все пройдено
    /// </summary>
    public float distancePassed;

    public float damageDealt;

    public VariationStats(float distancePassed, float damageDealt)
    {
        this.distancePassed = distancePassed;
        this.damageDealt = damageDealt;
    }
}
