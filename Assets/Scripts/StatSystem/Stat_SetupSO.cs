using UnityEngine;
// scriptable object

[CreateAssetMenu(menuName = "RPG Setup / Default Stat Setup", fileName = "Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float max_health = 100;
    public float health_regen;


    [Header("Offense - Physical Damage")]
    public float attack_speed = 1;
    public float damage = 10;
    public float crit_chance;
    public float crit_power = 150;
    public float armor_reduction;


    [Header("Offense - Elemental Damage")]
    public float fire_damage;
    public float ice_damage;
    public float lightning_damage;


    [Header("Deffence - Physical Damage")]
    public float armor;
    public float evasion;


    [Header("Deffence - Elemental Damage")]
    public float fire_resistance;
    public float ice_resistance;
    public float lightning_resistance;


    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
