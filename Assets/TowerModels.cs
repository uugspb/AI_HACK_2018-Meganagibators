using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel : object
{
    public int skillPoints = 0;
    public float MaxHealth = 50.0f;
    public float Armor = 0.0f;
    public float RegenPerSecond = 0.5f;
    public float Health = 50.0f;
}

public class GunModel : object
{
    public float Damage = 7.0f;
    public float linearMultiplicator = 0.5f;
    public float fireRate = 1.0f;
}
