using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region Inspector
    public float shootingAnimRange = 0.2f;
    public Animator avatarAnimator;
    public event Action OnDead;
    #endregion

    PlayerModel playerBase;
    PlayerModel playerRuntime;
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
        playerBase = new PlayerModel();
        gun = new GunModel();
        settings = FindObjectOfType<UserSettings>();
        settings.Init(playerBase);
    }

    public void StartGame()
    {
        Flush();
        StartCoroutine(DamageCoroutine());
    }

    public void StopGame()
    {
        StopAllCoroutines();
    }

    public void Flush()
    {
        playerRuntime = new PlayerModel
        {
            Armor = playerBase.Armor,
            MaxHealth = playerBase.MaxHealth,
            RegenPerSecond = playerBase.RegenPerSecond,
            skillPoints = playerBase.skillPoints,
        };
    }

    public void Damage(float count)
    {
        playerRuntime.MaxHealth -= count;
        if (playerRuntime.MaxHealth <= 0)
            Dead();
    }

    private IEnumerator DamageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(gun.fireRate);
            ShowShotAnim();
            GameController.Instance.Damage(gun);
        }
    }

    private void ShowShotAnim()
    {
        avatarAnimator.SetBool("shot", true);
        LeanTween.delayedCall(shootingAnimRange, HideShotAnim);
    }

    private void HideShotAnim()
    {
        avatarAnimator.SetBool("shot", false);
    }

    private void Dead()
    {
        if (OnDead != null)
            OnDead();
    }
}
