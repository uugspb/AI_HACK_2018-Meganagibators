using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettings : MonoBehaviour {

    public const float ARM_PER_POINT = 1;
    public const float REG_PER_POINT = 0.2f;
    public const float MAX_HEALTH_PER_POINT = 1;

    private PlayerModel model;

    public Text AvailableSkills;

    public ParameterSlider MaxHealthSlider;
    public ParameterSlider ArmorSlider;
    public ParameterSlider RegenSlider;

    public void Init(PlayerModel model)
    {
        this.model = model;
        AvailableSkills.text = model.skillPoints.ToString();
        MaxHealthSlider.value = model.MaxHealth;
        MaxHealthSlider.OnAddButton += MaxHealthSlider_OnAddButton;
        ArmorSlider.value = model.Armor;
        ArmorSlider.OnAddButton += ArmorSlider_OnAddButton;
        RegenSlider.value = model.RegenPerSecond;
        RegenSlider.OnAddButton += RegenSlider_OnAddButton;
    }

    private void RegenSlider_OnAddButton(ParameterSlider obj)
    {
        var addingPoints = Math.Min(5, model.skillPoints);
        model.RegenPerSecond = REG_PER_POINT * addingPoints;
        RegenSlider.value = model.RegenPerSecond;
    }

    private void ArmorSlider_OnAddButton(ParameterSlider obj)
    {
        var addingPoints = Math.Min(5, model.skillPoints);
        model.Armor = ARM_PER_POINT* addingPoints;
        ArmorSlider.value = model.RegenPerSecond;
    }

    private void MaxHealthSlider_OnAddButton(ParameterSlider obj)
    {
        var addingPoints = Math.Min(5, model.skillPoints);
        model.MaxHealth = REG_PER_POINT * addingPoints;
        MaxHealthSlider.value = model.RegenPerSecond;
    }
}
