using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public StatSetupDataSO default_stat_setup;

    public Stat_ResourceGroup resources;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
    public Stat_MajorGroup major;


    protected virtual void Awake()
    {
        
    }


    public void AdjustStatSetup(Stat_ResourceGroup resource_group, Stat_OffenseGroup offense_group, Stat_DefenseGroup defense_group, float penalty, float increase)
    {
        // INCREASE STATS
        offense.Damage.SetBaseValue(offense_group.Damage.GetValue() * increase);
        offense.AttackSpeed.SetBaseValue(offense_group.AttackSpeed.GetValue() * increase);
        offense.CritChance.SetBaseValue(offense_group.CritChance.GetValue() * increase);
        offense.CritPower.SetBaseValue(offense_group.CritPower.GetValue() * increase);
        offense.FireDamage.SetBaseValue(offense_group.FireDamage.GetValue() * increase);
        offense.IceDamage.SetBaseValue(offense_group.IceDamage.GetValue() * increase);
        offense.LightningDamage.SetBaseValue(offense_group.LightningDamage.GetValue() * increase);

        defense.Evasion.SetBaseValue(defense_group.Evasion.GetValue() * increase);

        // PENALTY STATS
        resources.MaxHealth.SetBaseValue(resource_group.MaxHealth.GetValue() * penalty);
        resources.HealthRegen.SetBaseValue(resource_group.HealthRegen.GetValue() * penalty);

        defense.Armor.SetBaseValue(defense_group.Armor.GetValue() * penalty);
        defense.FireRes.SetBaseValue(defense_group.FireRes.GetValue() * penalty);
        defense.IceRes.SetBaseValue(defense_group.IceRes.GetValue() * penalty);
        defense.LightningRes.SetBaseValue(defense_group.LightningRes.GetValue() * penalty);

    }


    public AttackData GetAttackData(DamageScaleData scale_data)
    {
        return new AttackData(this, scale_data);
    }


    public float GetElementalDamage(out ElementType element, float scale_factor = 1)
    {
        float fire_damage = offense.FireDamage.GetValue();
        float ice_damage = offense.IceDamage.GetValue();
        float lightning_damage = offense.LightningDamage.GetValue();
        float bonus_elemental_damage = major.Intelligence.GetValue(); // bonus elemental chance from intellegince +1

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
        float bonus_resistance = major.Intelligence.GetValue() * 0.5f; // bonus resistance from intelligence: +0.5f

        switch (element)
        {
            case ElementType.Fire:
                base_resistance = defense.FireRes.GetValue();
                break;
            case ElementType.Ice:
                base_resistance = defense.IceRes.GetValue();
                break;
            case ElementType.Lightning:
                base_resistance = defense.LightningRes.GetValue();
                break;
        }

        float resistance = base_resistance + bonus_resistance;
        float resistance_cap = 75f; // resistance will be capped at 75% 
        float final_resistance = Mathf.Clamp(resistance, 0, resistance_cap) / 100; // convert value to a multiplier

        return final_resistance;
    }


    public float GetPhysicalDamage(out bool is_crit, float scale_factor = 1)
    {
        float base_damage = GetBaseDamage();
        float crit_chance = GetCritChance();
        float crit_power = GetCritPower() / 100; // total crit power as multiplier

        is_crit = Random.Range(0, 100) < crit_chance;

        float final_damage = is_crit ? base_damage * crit_power : base_damage;

        return final_damage * scale_factor;
    }


    // bonus damage from Strength: +1
    public float GetBaseDamage() => offense.Damage.GetValue() + major.Strength.GetValue(); 
    
    // bonus crit chance from agility: +0.3%
    public float GetCritChance() => offense.CritChance.GetValue() + (major.Agility.GetValue() * 0.3f);
    
    // bonuc crit power from strength: +0.5%
    public float GetCritPower() => offense.CritPower.GetValue() + (major.Strength.GetValue() * 0.5f);


    public float GetArmorMitigation(float armor_reduction)
    {
        float total_armor = GetBaseArmor();

        float reduction_multiplier = Mathf.Clamp01(1 - armor_reduction); // same with Mathf.Clamp(1 - armor_reduction, 0 ,1);
        float effective_armor = total_armor * reduction_multiplier;

        float mitigation = effective_armor / (effective_armor + 100);
        float mitigation_cup = 0.85f; // max mitigation will be capped at %85
        float final_mitigation = Mathf.Clamp(mitigation, 0, mitigation_cup);

        return final_mitigation;
    }


    // each vitality points give you +1 armor
    public float GetBaseArmor() => defense.Armor.GetValue() + major.Vitality.GetValue();


    public float GetArmorReduction()
    {
        // total armor reduction as multiplier
        float final_reduction = offense.ArmorReduction.GetValue() / 100;

        return final_reduction;
    }


    public float GetEvasion()
    {
        float base_evasion = defense.Evasion.GetValue();
        float bonus_evasion = major.Agility.GetValue() * 0.5f; // each agility points give you %0.5 of evasion

        float total_evasion = base_evasion + bonus_evasion;
        float evasion_cap = 85f; // max evasion will be capped at %85

        float final_evasion = Mathf.Clamp(total_evasion, 0, evasion_cap); // clamps the given value between the given minumum float and maximum float values

        return final_evasion;
    }


    public float GetMaxHealth()
    {
        float base_max_health = resources.MaxHealth.GetValue();
        float bonus_max_health = major.Vitality.GetValue() * 5;
        float final_max_health = base_max_health + bonus_max_health;

        return final_max_health;
    }


    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth : return resources.MaxHealth;
            case StatType.HealthRegen : return resources.HealthRegen;

            case StatType.Strength : return major.Strength;
            case StatType.Agility : return major.Agility;
            case StatType.Intelligence : return major.Intelligence;
            case StatType.Vitality : return major.Vitality;

            case StatType.AttackSpeed : return offense.AttackSpeed;
            case StatType.Damage : return offense.Damage;
            case StatType.CritChance : return offense.CritChance;
            case StatType.CritPower : return offense.CritPower;
            case StatType.ArmorReduction : return offense.ArmorReduction;

            case StatType.FireDamage : return offense.FireDamage;
            case StatType.IceDamage : return offense.IceDamage;
            case StatType.LightningDamage : return offense.LightningDamage;

            case StatType.Armor : return defense.Armor;
            case StatType.Evasion : return defense.Evasion;

            case StatType.FireResistance : return defense.FireRes;
            case StatType.IceResistance : return defense.IceRes;
            case StatType.LightningResistance : return defense.LightningRes;

            default: 
                Debug.Log($"StatType {type} not implemented yet.");
                return null;
        }
    }


    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (default_stat_setup == null)
        {
            Debug.Log("No default stat setup assigned.");
            return;
        }

        resources.MaxHealth.SetBaseValue(default_stat_setup.max_health);
        resources.HealthRegen.SetBaseValue(default_stat_setup.health_regen);

        major.Strength.SetBaseValue(default_stat_setup.strength);
        major.Agility.SetBaseValue(default_stat_setup.agility);
        major.Intelligence.SetBaseValue(default_stat_setup.intelligence);
        major.Vitality.SetBaseValue(default_stat_setup.vitality);

        offense.AttackSpeed.SetBaseValue(default_stat_setup.attack_speed);
        offense.Damage.SetBaseValue(default_stat_setup.damage);
        offense.CritChance.SetBaseValue(default_stat_setup.crit_chance);
        offense.CritPower.SetBaseValue(default_stat_setup.crit_power);
        offense.ArmorReduction.SetBaseValue(default_stat_setup.armor_reduction);

        offense.FireDamage.SetBaseValue(default_stat_setup.fire_damage);
        offense.IceDamage.SetBaseValue(default_stat_setup.ice_damage);
        offense.LightningDamage.SetBaseValue(default_stat_setup.lightning_damage);

        defense.Armor.SetBaseValue(default_stat_setup.armor);
        defense.Evasion.SetBaseValue(default_stat_setup.evasion);

        defense.FireRes.SetBaseValue(default_stat_setup.fire_resistance);
        defense.IceRes.SetBaseValue(default_stat_setup.ice_resistance);
        defense.LightningRes.SetBaseValue(default_stat_setup.lightning_resistance);
    }
}
