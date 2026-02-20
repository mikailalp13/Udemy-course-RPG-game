using UnityEngine;
using System;

[Serializable]
public class AttackData 
{
    public float physcial_damage;
    public float elemental_damage;
    public bool is_crit;
    public ElementType element;

    public ElementalEffectData effect_data;


    public AttackData(Entity_Stats entity_stats, DamageScaleData scale_date)
    {
        physcial_damage = entity_stats.GetPhysicalDamage(out is_crit, scale_date.physical);
        elemental_damage = entity_stats.GetElementalDamage(out element, scale_date.elemental);

        effect_data = new ElementalEffectData(entity_stats, scale_date);
    }
}
