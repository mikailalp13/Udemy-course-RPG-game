using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skill_manager { get; private set; }
    public Player player { get; private set; }
    public DamageScaleData damage_scale_data { get; private set; }


    [Header("General Details")]
    [SerializeField] protected SkillType skill_type;
    [SerializeField] protected SkillUpgradeType upgrade_type;

    [SerializeField] protected float cooldown;
    private float last_time_used;


    protected virtual void Awake()
    {
        skill_manager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        last_time_used = last_time_used - cooldown;
        damage_scale_data = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {
        
    }

    public void SetSkillUpgrade(Skill_DataSO skill_data)
    {
        UpgradeData upgrade = skill_data.upgrade_data;

        upgrade_type = upgrade.upgrade_type;
        cooldown = upgrade.cooldown;
        damage_scale_data = upgrade.damage_scale_data;

        player.ui.in_game_ui.GetSkillSlot(skill_type).SetupSkillSlot(skill_data);
        ResetCooldown();
    }

    public virtual bool CanUseSkill()
    {
        if (upgrade_type == SkillUpgradeType.None)
            return false;
        
        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }
        
        return true;
    }


    public void SetSkillOnCooldown()
    {
        player.ui.in_game_ui.GetSkillSlot(skill_type).StartCooldown(cooldown);
        last_time_used = Time.time;
    } 

    public void ResetCooldown()
    {
        player.ui.in_game_ui.GetSkillSlot(skill_type).ResetCooldown();
        last_time_used = Time.time - cooldown;
    }
     
    
    protected bool Unlocked(SkillUpgradeType upgrade_to_check) => upgrade_type == upgrade_to_check;
    public void ReduceCooldownBy(float cooldown_reduction) => last_time_used = last_time_used + cooldown_reduction;
    protected bool OnCooldown() => Time.time < last_time_used + cooldown;
}
