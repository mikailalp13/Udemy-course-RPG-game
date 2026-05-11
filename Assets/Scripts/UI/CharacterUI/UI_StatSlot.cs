using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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

        if (stat_to_update == null && stat_slot_type != StatType.ElementalDamage)
        {
            Debug.Log($"You do not have {stat_slot_type} implemented on the player");
            return;
        }
        
        float value = 0;

        switch (stat_slot_type)
        {
            // Major Stats
            case StatType.Strength:
                value = player_stats.major.Strength.GetValue();
                break;
            
            case StatType.Agility:
                value = player_stats.major.Agility.GetValue();
                break;

            case StatType.Intelligence:
                value = player_stats.major.Intelligence.GetValue();
                break;

            case StatType.Vitality:
                value = player_stats.major.Vitality.GetValue();
                break;

            // Offense Stats
            case StatType.Damage:
                value = player_stats.GetBaseDamage();
                break;
            
            case StatType.CritChance:
                value = player_stats.GetCritChance();
                break;

            case StatType.CritPower:
                value = player_stats.GetCritPower();
                break;

            case StatType.ArmorReduction:
                value = player_stats.GetArmorReduction() * 100;
                break;

            case StatType.AttackSpeed:
                value = player_stats.offense.AttackSpeed.GetValue() * 100;
                break;

            // Defense Stats
            case StatType.MaxHealth: 
                value = player_stats.GetMaxHealth();
                break;

            case StatType.HealthRegen: 
                value = player_stats.resources.HealthRegen.GetValue();
                break;

            case StatType.Evasion: 
                value = player_stats.GetEvasion();
                break;

            case StatType.Armor: 
                value = player_stats.GetBaseArmor();
                break;

            // Elemental Damage Stats
            case StatType.IceDamage: 
                value = player_stats.offense.IceDamage.GetValue();
                break;

            case StatType.FireDamage: 
                value = player_stats.offense.FireDamage.GetValue();
                break;

            case StatType.LightningDamage: 
                value = player_stats.offense.LightningDamage.GetValue();
                break;

            case StatType.ElementalDamage: 
                value = player_stats.GetElementalDamage(out ElementType element, 1);
                break;

            // Elemental Resistance Stats
            case StatType.IceResistance:
                value = player_stats.GetElementalResistance(ElementType.Ice) * 100;
                break;

            case StatType.FireResistance:
                value = player_stats.GetElementalResistance(ElementType.Fire) * 100;
                break;

            case StatType.LightningResistance:
                value = player_stats.GetElementalResistance(ElementType.Lightning) * 100;
                break;
        }

        stat_value.text = IsPercentageStat(stat_slot_type) ? value + "%" : value.ToString();
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
            case StatType.ElementalDamage: return "Elemental Damage";

            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";

            default: return "Unknown Stat";    
        }
    }
}
