using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO default_stat_setup;

    public Stat_ResourceGroup resources;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
    public Stat_MajorGroup major;

    public float GetElementalDamage(out ElementType element, float scale_factor = 1)
    {
        float fire_damage = offense.fire_damage.GetValue();
        float ice_damage = offense.ice_damage.GetValue();
        float lightning_damage = offense.lightning_damage.GetValue();
        float bonus_elemental_damage = major.intelligence.GetValue(); // bonus elemental chance from intellegince +1

        float highest_damage = fire_damage;
        element = ElementType.Fire;

        if (ice_damage > highest_damage)
        {
            highest_damage = ice_damage;
            element = ElementType.Ice;
        }
        else if (lightning_damage > highest_damage)
        {
            highest_damage = lightning_damage;
            element = ElementType.Lightning;
        }

        if (highest_damage <= 0)
        {
            element = ElementType.None;
            return 0;
        }


        float bonus_fire = (element == ElementType.Fire) ? 0 : fire_damage * 0.5f;
        float bonus_ice = (element == ElementType.Ice) ? 0 : ice_damage * 0.5f;
        float bonus_lightning = (element == ElementType.Lightning) ? 0 : lightning_damage * 0.5f;

        float weaker_elements_damage = bonus_fire + bonus_ice + bonus_lightning;
        float final_damage = highest_damage + weaker_elements_damage + bonus_elemental_damage;

        return final_damage * scale_factor;
    }


    public float GetElementalResistance(ElementType element)
    {
        float base_resistance = 0;
        float bonus_resistance = major.intelligence.GetValue() * 0.5f; // bonus resistance from intelligence: +0.5f

        switch (element)
        {
            case ElementType.Fire:
                base_resistance = defense.fire_res.GetValue();
                break;
            case ElementType.Ice:
                base_resistance = defense.ice_res.GetValue();
                break;
            case ElementType.Lightning:
                base_resistance = defense.lightning_res.GetValue();
                break;
        }

        float resistance = base_resistance + bonus_resistance;
        float resistance_cap = 75f; // resistance will be capped at 75% 
        float final_resistance = Mathf.Clamp(resistance, 0, resistance_cap) / 100; // convert value to a multiplier

        return final_resistance;
    }


    public float GetPhysicalDamage(out bool is_crit, float scale_factor = 1)
    {
        float base_damage = offense.damage.GetValue();
        float bonus_damage = major.strength.GetValue();
        float total_base_damage = base_damage + bonus_damage;

        float base_crit_chance = offense.crit_chance.GetValue();
        float bonus_crit_chance = major.agility.GetValue() * 0.3f; // bonus crit chance from agility: +0.3%
        float crit_chance = base_crit_chance + bonus_crit_chance;

        float base_crit_power = offense.crit_power.GetValue();
        float bonus_crit_power = major.strength.GetValue() * 0.5f; // bonuc crit chance from strength: +0.5%
        float crit_power = (base_crit_power + bonus_crit_power) / 100; // total crit power as multiplier

        is_crit = Random.Range(0, 100) < crit_chance;

        float final_damage = is_crit ? total_base_damage * crit_power : total_base_damage;

        return final_damage * scale_factor;
    }


    public float GetArmorMitigation(float armor_reduction)
    {
        float base_armor = defense.armor.GetValue();
        float bonus_armor = major.vitality.GetValue(); // each vitality points give you +1 armor
        float total_armor = base_armor + bonus_armor;

        float reduction_multiplier = Mathf.Clamp01(1 - armor_reduction); // same with Mathf.Clamp(1 - armor_reduction, 0 ,1);
        float effective_armor = total_armor * reduction_multiplier;

        float mitigation = effective_armor / (effective_armor + 100);
        float mitigation_cup = 0.85f; // max mitigation will be capped at %85
        float final_mitigation = Mathf.Clamp(mitigation, 0, mitigation_cup);

        return final_mitigation;
    }

    public float GetArmorReduction()
    {
        // total armor reduction as multiplier
        float final_reduction = offense.armor_reduction.GetValue() / 100;

        return final_reduction;
    }

    public float GetEvasion()
    {
        float base_evasion = defense.evasion.GetValue();
        float bonus_evasion = major.agility.GetValue() * 0.5f; // each agility points give you %0.5 of evasion

        float total_evasion = base_evasion + bonus_evasion;
        float evasion_cap = 85f; // max evasion will be capped at %85

        float final_evasion = Mathf.Clamp(total_evasion, 0, evasion_cap); // clamps the given value between the given minumum float and maximum float values

        return final_evasion;
    }

    public float GetMaxHealth()
    {
        float base_max_health = resources.max_health.GetValue();
        float bonus_max_health = major.vitality.GetValue() * 5;
        float final_max_health = base_max_health + bonus_max_health;

        return final_max_health;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.max_health : return resources.max_health;
            case StatType.health_regen : return resources.health_regen;

            case StatType.strength : return major.strength;
            case StatType.agility : return major.agility;
            case StatType.intelligence : return major.intelligence;
            case StatType.vitality : return major.vitality;

            case StatType.attack_speed : return offense.attack_speed;
            case StatType.damage : return offense.damage;
            case StatType.crit_chance : return offense.crit_chance;
            case StatType.crit_power : return offense.crit_power;
            case StatType.armor_reduction : return offense.armor_reduction;

            case StatType.fire_damage : return offense.fire_damage;
            case StatType.ice_damage : return offense.ice_damage;
            case StatType.lightning_damage : return offense.lightning_damage;

            case StatType.armor : return defense.armor;
            case StatType.evasion : return defense.evasion;

            case StatType.fire_resistance : return defense.fire_res;
            case StatType.ice_resistance : return defense.ice_res;
            case StatType.lightning_resistance : return defense.lightning_res;

            default: 
                Debug.Log($"StatType {type} not implemented yet.");
                return null;
        }
    }


    [ContextMenu("Update Default Setup")]

    public void ApplyDefaultStatSetup()
    {
        if (default_stat_setup == null)
        {
            Debug.Log("No default stat setup assigned. ");
            return;
        }

        resources.max_health.SetBaseValue(default_stat_setup.max_health);
        resources.health_regen.SetBaseValue(default_stat_setup.health_regen);

        major.strength.SetBaseValue(default_stat_setup.strength);
        major.agility.SetBaseValue(default_stat_setup.agility);
        major.intelligence.SetBaseValue(default_stat_setup.intelligence);
        major.vitality.SetBaseValue(default_stat_setup.vitality);

        offense.attack_speed.SetBaseValue(default_stat_setup.attack_speed);
        offense.damage.SetBaseValue(default_stat_setup.damage);
        offense.crit_chance.SetBaseValue(default_stat_setup.crit_chance);
        offense.crit_power.SetBaseValue(default_stat_setup.crit_power);
        offense.armor_reduction.SetBaseValue(default_stat_setup.armor_reduction);

        offense.fire_damage.SetBaseValue(default_stat_setup.fire_damage);
        offense.ice_damage.SetBaseValue(default_stat_setup.ice_damage);
        offense.lightning_damage.SetBaseValue(default_stat_setup.lightning_damage);

        defense.armor.SetBaseValue(default_stat_setup.armor);
        defense.evasion.SetBaseValue(default_stat_setup.evasion);

        defense.fire_res.SetBaseValue(default_stat_setup.fire_resistance);
        defense.ice_res.SetBaseValue(default_stat_setup.ice_resistance);
        defense.lightning_res.SetBaseValue(default_stat_setup.lightning_resistance);



    }
}
