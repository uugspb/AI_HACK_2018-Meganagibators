using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel{
    public float MaxHealth = 100.0f;
    public float Armor = 100.0f;
    public float RegenPerSecond = 2.0f;
}

public class GunModel
{
    public float Damage;
    public float linearMultiplicator;
}
