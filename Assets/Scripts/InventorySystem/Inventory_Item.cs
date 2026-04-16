using System;
using UnityEngine;
using System.Text;


[Serializable]
public class Inventory_Item 
{
    private string item_id;

    public ItemDataSO item_data;
    public int stack_size = 1;

    public ItemModifier[] modifiers { get; private set; }
    public ItemEffectDataSO item_effect;

    public int buy_price { get; private set; }
    public float sell_price { get; private set; }


    public Inventory_Item(ItemDataSO item_data)
    {
        this.item_data = item_data;
        item_effect = item_data.item_effect;
        buy_price = item_data.item_price;
        sell_price = item_data.item_price * 0.34f;

        modifiers = EquipmentData()?.modifiers;

        item_id = item_data.item_name + " - " + Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats player_stats)
    {
        foreach (var mod in modifiers)
        {
            Stat stat_to_modify = player_stats.GetStatByType(mod.stat_type);
            stat_to_modify.AddModifier(mod.value, item_id);
        }
    }

    public void RemoveModifiers(Entity_Stats player_stats)
    {
        foreach (var mod in modifiers)
        {
            Stat stat_to_modify = player_stats.GetStatByType(mod.stat_type);
            stat_to_modify.RemoveModifier(item_id);
        }
    }

    private EquipmentDataSO EquipmentData()
    {
        if (item_data is EquipmentDataSO equipment)
            return equipment;
        
        return null;
    }

    public void AddItemEffect(Player player) => item_effect?.Subscribe(player);
    public void RemoveItemEffect(Player player) => item_effect?.Unsubscribe();

    public bool CanAddStack() => stack_size < item_data.max_stack_size;
    public void AddStack() => stack_size++;   
    public void RemoveStack() => stack_size--;


    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();

        if (item_data.item_type == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Used for crafting.");
            sb.AppendLine("");

            return sb.ToString();
        }

        if (item_data.item_type == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(item_effect.effect_description);

            return sb.ToString();
        }
    
        sb.AppendLine("");

        foreach (var mod in modifiers)
        {
            string mod_type = GetStatNameByType(mod.stat_type);
            string mod_value = IsPercentageStat(mod.stat_type) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine(" + " + mod_value + " " + mod_type);
        }


        if (item_effect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique effect: ");
            sb.AppendLine(item_effect.effect_description);
        }
        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.max_health: return "Max Health";
            case StatType.health_regen: return "Health Regeneration";
            case StatType.evasion: return "Evasion";
            case StatType.armor: return "Armor";
            case StatType.strength: return "Strength";
            case StatType.agility: return "Agility";
            case StatType.intelligence: return "Intelligence";
            case StatType.vitality: return "Vitality";
            case StatType.attack_speed: return "Attack Speed";
            case StatType.damage: return "Damage";
            case StatType.crit_chance: return "Crit Chance";
            case StatType.crit_power: return "Crit Power";
            case StatType.armor_reduction: return "Armor Reduction";
            case StatType.fire_damage: return "Fire Damage";
            case StatType.ice_damage: return "Ice Damage";
            case StatType.lightning_damage: return "Lightning Damage";
            case StatType.ice_resistance: return "Ice Resistance";
            case StatType.fire_resistance: return "Fire Resistance";
            case StatType.lightning_resistance: return "Lightning Resistance";
            default: return "Unknown Stat";    
        }
    }

    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.crit_chance:
            case StatType.crit_power:
            case StatType.armor_reduction:
            case StatType.ice_resistance:
            case StatType.fire_resistance:
            case StatType.lightning_resistance:
            case StatType.attack_speed:
            case StatType.evasion:
                return true;
            default:
                return false;
        }
    }
}
