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
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Evasion: return "Evasion";
            case StatType.Armor: return "Armor";
            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.CritChance: return "Crit Chance";
            case StatType.CritPower: return "Crit Power";
            case StatType.ArmorReduction: return "Armor Reduction";
            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";
            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknown Stat";    
        }
    }


    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}
