using UnityEngine;
using System;

[Serializable]
public class DamageScaleData 
{
    [Header("Damage")]
    public float physical = 1f;
    public float elemental = 1f;


    [Header("Chill")]
    public float chill_duration = 3f;
    public float chill_slow_multiplier = 0.2f;


    [Header("Burn")]
    public float burn_duration = 3f;
    public float burn_damage_scale = 1f;


    [Header("Shock")]
    public float shock_duration = 3f;
    public float shock_damage_scale = 1f;
    public float shock_charge = 0.4f;
}
