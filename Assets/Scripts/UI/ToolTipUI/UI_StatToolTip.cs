using TMPro;
using UnityEngine;

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
            case StatType.Strength:
                return "Increases physical damage by 1 per point." +
                    "\n Increases critical power by 0.5% per point.";
                
            case StatType.Agility:
                return "Increases critacal chance by 0.3% per point." +
                    "\n Increases evasion by 0.5% per point.";

            case StatType.Intelligence:
                return "Increases elemental resistances by 0.5% per point." +
                    "\n Adds 1 elemental damage per point as a bonus." + 
                    "\n If all elements have 0 damage, the bonus won't be applied.";

            case StatType.Vitality:
                return "Increases maximum health by 5 per point." +
                    "\n Increases armor by 1 per point.";


            // Physical Damage Stats
            case StatType.Damage:
                return "Determines the physical damage of your attacks.";

            case StatType.CritChance:
                return "Chance for your attacks to critically strike.";

            case StatType.CritPower:
                return "Increases the damage dealt by critical strikes.";

            case StatType.ArmorReduction:
                return "Percent of armor that will be ignored by your attacks.";

            case StatType.AttackSpeed:
                return "Determines how quickly you can attack.";


            // Defense Stats
            case StatType.MaxHealth:
                return "Determines how much total health you have.";

            case StatType.HealthRegen:
                return "Amount of health restored per second.";

            case StatType.Armor:
                return "Reduces incoming physical damage." +
                    "\n Armor mitigation is limited at 85%." +
                    "\n Current mitigation is: " + player_stats.GetArmorMitigation(0) * 100 + "%.";

            case StatType.Evasion:
                return "Chance to compeletly avoid attacks." + 
                    "\n Evasion is limited at 85%.";
            

            // Elemental Damage
            case StatType.FireDamage:
                return "Determines the fire damage of your attacks.";

            case StatType.IceDamage:
                return "Determines the ice damage of your attacks.";

            case StatType.LightningDamage:
                return "Determines the lightning damage of your attacks.";

            case StatType.ElementalDamage:
                return "Elemental damage combines all three elements." + 
                    "\n The highest element applies corresponding element status effect and full damage." + 
                    "\n The other two elements contribute 50% of their damage as a bonus.";

            
            // Elemental Resistance
            case StatType.FireResistance:
                return "Reduces fire damage taken.";

            case StatType.IceResistance:
                return "Reduces ice damage taken.";

            case StatType.LightningResistance:
                return "Reduces lightning damage taken.";

            default:
                return "No tooltip avaliable for this stat.";
        }
    }
}
