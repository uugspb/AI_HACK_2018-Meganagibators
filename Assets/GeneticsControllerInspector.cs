#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(GeneticsController))]
public class GeneticsControllerInspector : Editor
{
    private FieldInfo currentSkillPointsField = typeof(GeneticsController).GetField("currentBotSkillPoints", BindingFlags.Instance | BindingFlags.NonPublic);
    private FieldInfo currentBestField = typeof(GeneticsController).GetField("currentBestVariation", BindingFlags.Instance | BindingFlags.NonPublic);
    private FieldInfo lastGivenField = typeof(GeneticsController).GetField("lastGivenVariation", BindingFlags.Instance | BindingFlags.NonPublic);
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!EditorApplication.isPlaying)
            return;

        EditorGUILayout.Separator();
        
        EditorGUILayout.TextField("skill points", currentSkillPointsField.GetValue(target).ToString());

        EditorGUILayout.Separator();

        Variation currentBest = currentBestField.GetValue(target) as Variation;
        if (currentBest != null)
        {
            EditorGUILayout.LabelField("Current Best", EditorStyles.boldLabel);
            EditorGUILayout.TextField("fitness", currentBest.fitnessValue.ToString());
            EditorGUILayout.TextField("hp", currentBest.hpLevel.ToString() + " (" + currentBest.HPValue + ")");
            EditorGUILayout.TextField("armor", currentBest.armorLevel.ToString() + " (" + currentBest.ArmorValue + ")");
            EditorGUILayout.TextField("damage", currentBest.damageLevel.ToString() + " (" + currentBest.DamageValue + ")");
            EditorGUILayout.TextField("fireRate", currentBest.fireRateLevel.ToString() + " (" + currentBest.FireRateValue + ")");
            EditorGUILayout.TextField("speed", currentBest.speedLevel.ToString() + " (" + currentBest.SpeedValue + ")");
            EditorGUILayout.TextField("spawnRate", currentBest.spawnRateLevel.ToString() + " (" + currentBest.SpawnRateValue + ")");
        }

        EditorGUILayout.Separator();

        Variation lastGiven = lastGivenField.GetValue(target) as Variation;
        if (lastGiven != null)
        {
            EditorGUILayout.LabelField("Last given", EditorStyles.boldLabel);
            EditorGUILayout.TextField("fitness", lastGiven.fitnessValue.ToString());
            EditorGUILayout.TextField("hp", lastGiven.hpLevel.ToString() + " (" + lastGiven.HPValue + ")");
            EditorGUILayout.TextField("armor", lastGiven.armorLevel.ToString() + " (" + lastGiven.ArmorValue + ")");
            EditorGUILayout.TextField("damage", lastGiven.damageLevel.ToString() + " (" + lastGiven.DamageValue + ")");
            EditorGUILayout.TextField("fireRate", lastGiven.fireRateLevel.ToString() + " (" + lastGiven.FireRateValue + ")");
            EditorGUILayout.TextField("speed", lastGiven.speedLevel.ToString() + " (" + lastGiven.SpeedValue + ")");
            EditorGUILayout.TextField("spawnRate", lastGiven.spawnRateLevel.ToString() + " (" + lastGiven.SpawnRateValue + ")");
        }
    }
}
#endif
