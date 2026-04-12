using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player_Stats player_stats;
    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatType stat_slot_type;
    [SerializeField] private TextMeshProUGUI stat_name;
    [SerializeField] private TextMeshProUGUI stat_value;


    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(stat_slot_type);
        stat_name.text = GetStatNameByType(stat_slot_type);
    }


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        player_stats = FindFirstObjectByType<Player_Stats>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.stat_tool_tip.ShowToolTip(true, rect, stat_slot_type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.stat_tool_tip.ShowToolTip(false, null);
    }


    public void UpdateStatValue()
    {
        Stat stat_to_update = player_stats.GetStatByType(stat_slot_type);

        if (stat_to_update == null && stat_slot_type != StatType.elemental_damage)
        {
            Debug.Log($"You do not have {stat_slot_type} implemented on the player");
            return;
        }
        
        float value = 0;

        switch (stat_slot_type)
        {
            // Major Stats
            case StatType.strength:
                value = player_stats.major.strength.GetValue();
                break;
            
            case StatType.agility:
                value = player_stats.major.agility.GetValue();
                break;

            case StatType.intelligence:
                value = player_stats.major.intelligence.GetValue();
                break;

            case StatType.vitality:
                value = player_stats.major.vitality.GetValue();
                break;

            // Offense Stats
            case StatType.damage:
                value = player_stats.GetBaseDamage();
                break;
            
            case StatType.crit_chance:
                value = player_stats.GetCritChance();
                break;

            case StatType.crit_power:
                value = player_stats.GetCritPower();
                break;

            case StatType.armor_reduction:
                value = player_stats.GetArmorReduction() * 100;
                break;

            case StatType.attack_speed:
                value = player_stats.offense.attack_speed.GetValue() * 100;
                break;

            // Defense Stats
            case StatType.max_health: 
                value = player_stats.GetMaxHealth();
                break;

            case StatType.health_regen: 
                value = player_stats.resources.health_regen.GetValue();
                break;

            case StatType.evasion: 
                value = player_stats.GetEvasion();
                break;

            case StatType.armor: 
                value = player_stats.GetBaseArmor();
                break;

            // Elemental Damage Stats
            case StatType.ice_damage: 
                value = player_stats.offense.ice_damage.GetValue();
                break;

            case StatType.fire_damage: 
                value = player_stats.offense.fire_damage.GetValue();
                break;

            case StatType.lightning_damage: 
                value = player_stats.offense.lightning_damage.GetValue();
                break;

            case StatType.elemental_damage: 
                value = player_stats.GetElementalDamage(out ElementType element, 1);
                break;

            // Elemental Resistance Stats
            case StatType.ice_resistance:
                value = player_stats.GetElementalResistance(ElementType.Ice) * 100;
                break;

            case StatType.fire_resistance:
                value = player_stats.GetElementalResistance(ElementType.Fire) * 100;
                break;

            case StatType.lightning_resistance:
                value = player_stats.GetElementalResistance(ElementType.Lightning) * 100;
                break;
        }

        stat_value.text = IsPercentageStat(stat_slot_type) ? value + "%" : value.ToString();
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
            case StatType.elemental_damage: return "Elemental Damage";

            case StatType.ice_resistance: return "Ice Resistance";
            case StatType.fire_resistance: return "Fire Resistance";
            case StatType.lightning_resistance: return "Lightning Resistance";

            default: return "Unknown Stat";    
        }
    }
}
