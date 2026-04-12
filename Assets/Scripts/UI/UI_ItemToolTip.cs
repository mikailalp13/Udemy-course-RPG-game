using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI item_name;
    [SerializeField] private TextMeshProUGUI item_type;
    [SerializeField] private TextMeshProUGUI item_info;


    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRect);

        item_name.text = itemToShow.item_data.item_name;
        item_type.text = itemToShow.item_data.item_type.ToString();
        item_info.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.item_data.item_type == ItemType.Material)
            return "Used for crafting.";

        if (item.item_data.item_type == ItemType.Consumable)
            return item.item_data.item_effect.effect_description;
        
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string mod_type = GetStatNameByType(mod.stat_type);
            string mod_value = IsPercentageStat(mod.stat_type) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine(" + " + mod_value + " " + mod_type);
        }

        if (item.item_effect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique effect: ");
            sb.AppendLine(item.item_effect.effect_description);
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
