using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    PlayerModel player;
    GunModel gun;
    UserSettings settings;

    public static Tower Instance
    {
        get; private set;
    }

    private Tower()
    {
        Instance = this;
    }

    public void Initialize()
    {
        player = new PlayerModel()
        {
            Armor = 10,
            MaxHealth = 50,
            RegenPerSecond = 0
        };
        gun = new GunModel();
        settings = FindObjectOfType<UserSettings>();
        settings.Init(player);
    }

    public void StartGame()
    {

    } 
}
