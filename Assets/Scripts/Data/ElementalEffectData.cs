using System;
using UnityEngine;

[Serializable]
public class ElementalEffectData 
{
    public float chill_duration;
    public float chill_slow_multiplier;

    public float burn_duration;
    public float total_burn_damage;

    public float shock_duration;
    public float shock_damage;
    public float shock_charge;

    public ElementalEffectData(Entity_Stats entityStats, DamageScaleData damageScale)
    {
        chill_duration = damageScale.chill_duration;
        chill_slow_multiplier = damageScale.chill_slow_multiplier;

        burn_duration = damageScale.burn_duration;
        total_burn_damage = entityStats.offense.fire_damage.GetValue() * damageScale.burn_damage_scale;

        shock_duration = damageScale.shock_duration;
        shock_damage = entityStats.offense.lightning_damage.GetValue() * damageScale.shock_damage_scale;
        shock_charge = damageScale.shock_charge;
    }
}
