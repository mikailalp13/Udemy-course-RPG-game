using UnityEngine;
using TMPro;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats player_stats;
    private TextMeshProUGUI stat_tool_tip_text;

    protected override void Awake()
    {
        base.Awake();

        player_stats = FindFirstObjectByType<Player_Stats>();
        stat_tool_tip_text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform target_rect, StatType stat_type)
    {
        base.ShowToolTip(show, target_rect);

        stat_tool_tip_text.text = GetStatTextByType(stat_type);
    }

    public string GetStatTextByType(StatType type)
    {
        switch (type)
        {
            // Major Stats
            case StatType.strength:
                return "Increases physical damage by 1 per point." +
                    "\n Increases critical power by 0.5% per point.";
                
            case StatType.agility:
                return "Increases critacal chance by 0.3% per point." +
                    "\n Increases evasion by 0.5% per point.";

            case StatType.intelligence:
                return "Increases elemental resistances by 0.5% per point." +
                    "\n Adds 1 elemental damage per point as a bonus." + 
                    "\n If all elements have 0 damage, the bonus won't be applied.";

            case StatType.vitality:
                return "Increases maximum health by 5 per point." +
                    "\n Increases armor by 1 per point.";


            // Physical Damage Stats
            case StatType.damage:
                return "Determines the physical damage of your attacks.";

            case StatType.crit_chance:
                return "Chance for your attacks to critically strike.";

            case StatType.crit_power:
                return "Increases the damage dealt by critical strikes.";

            case StatType.armor_reduction:
                return "Percent of armor that will be ignored by your attacks.";

            case StatType.attack_speed:
                return "Determines how quickly you can attack.";


            // Defense Stats
            case StatType.max_health:
                return "Determines how much total health you have.";

            case StatType.health_regen:
                return "Amount of health restored per second.";

            case StatType.armor:
                return "Reduces incoming physical damage." +
                    "\n Armor mitigation is limited at 85%." +
                    "\n Current mitigation is: " + player_stats.GetArmorMitigation(0) * 100 + "%.";

            case StatType.evasion:
                return "Chance to compeletly avoid attacks." + 
                    "\n Evasion is limited at 85%.";
            

            // Elemental Damage
            case StatType.fire_damage:
                return "Determines the fire damage of your attacks.";

            case StatType.ice_damage:
                return "Determines the ice damage of your attacks.";

            case StatType.lightning_damage:
                return "Determines the lightning damage of your attacks.";

            case StatType.elemental_damage:
                return "Elemental damage combines all three elements." + 
                    "\n The highest element applies corresponding element status effect and full damage." + 
                    "\n The other two elements contribute 50% of their damage as a bonus.";

            
            // Elemental Resistance
            case StatType.fire_resistance:
                return "Reduces fire damage taken.";

            case StatType.ice_resistance:
                return "Reduces ice damage taken.";

            case StatType.lightning_resistance:
                return "Reduces lightning damage taken.";

            default:
                return "No tooltip avaliable for this stat.";
        }
    }
}
