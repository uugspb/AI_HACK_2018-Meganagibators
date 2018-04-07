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
        player = new PlayerModel();
        gun = new GunModel();
        settings = FindObjectOfType<UserSettings>();
        settings.Init(player);
    }

    public void StartGame()
    {
        StartCoroutine(DamageCoroutine());
    } 

    public void StopGame()
    {
        StopAllCoroutines();
    }

    private IEnumerator DamageCoroutine()
    {
        while (true) { 
            yield return new WaitForSeconds(gun.fireRate);
            GameController.Instance.Damage(gun);
        }
    }
}
