using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject time_echo_prefab;
    [SerializeField] private float time_echo_duration;

    [Header("Attack Upgrades")]
    [SerializeField] private int max_attacks = 3;
    [SerializeField] private float duplicate_chance = 0.3f;

    [Header("Heal Wisp Upgrades")]
    [SerializeField] private float damage_percent_healed = 0.3f;
    [SerializeField] private float cooldown_reduced_in_seconds;

    public float GetPercentOfDamageHealed()
    {
        if (ShouldBeWisp() == false)
            return 0;
        
        return damage_percent_healed;
    }

    public float GetCooldownReduceInSeconds()
    {
        if (upgrade_type != SkillUpgradeType.TimeEcho_CooldownWisp)
            return 0;
        
        return cooldown_reduced_in_seconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgrade_type == SkillUpgradeType.TimeEcho_CleanseWisp;
    }
    public bool ShouldBeWisp()
    {
        return upgrade_type == SkillUpgradeType.TimeEcho_HealWisp
            || upgrade_type == SkillUpgradeType.TimeEcho_CleanseWisp
            || upgrade_type == SkillUpgradeType.TimeEcho_CooldownWisp;
    }

    public float GetDuplicateChance()
    {
        if (upgrade_type != SkillUpgradeType.TimeEcho_ChanceToDuplicate)
            return 0;
        
        return duplicate_chance;
    }

    public int GetMaxAttacks()
    {
        if (upgrade_type == SkillUpgradeType.TimeEcho_SingleAttack || upgrade_type == SkillUpgradeType.TimeEcho_ChanceToDuplicate)
            return 1;
        
        else if (upgrade_type == SkillUpgradeType.TimeEcho_MultiAttack)
            return max_attacks;
        
        return 0;
    }

    public float GetEchoDuration()
    {
        return time_echo_duration;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;
        
        CreateTimeEcho();
    }

    public void CreateTimeEcho(Vector3? target_position = null)
    {
        Vector3 position = target_position ?? transform.position;

        GameObject time_echo = Instantiate(time_echo_prefab, position, Quaternion.identity);
        time_echo.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
