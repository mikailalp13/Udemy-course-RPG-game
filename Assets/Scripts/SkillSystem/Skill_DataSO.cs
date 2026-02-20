using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Description")]
    public string display_name;
    [TextArea]
    public string description;
    public Sprite icon;


    [Header("Unlock & Upgrade")]
    public int cost; 
    public bool unlocked_by_default;
    public SkillType skill_type;
    public UpgradeData upgrade_data;
}  

[System.Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgrade_type;
    public float cooldown;
    public DamageScaleData damage_scale_data;
}
