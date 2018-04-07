﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float shootingAnimRange = 0.2f;
    public Animator avatarAnimator;
    public event Action OnDead;
    public float Health;   

    PlayerModel playerBase;
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
        playerBase.Health = playerBase.MaxHealth;
    }

    public void LevelUp(int delta)
    {
        playerBase.skillPoints += delta;
    }

    public void Damage(float count)
    {
        playerBase.Health -= count;
        if (playerBase.Health <= 0)
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
