using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow sword_throw { get; private set; }
    public Skill_TimeEcho time_echo { get; private set; }
    public Skill_DomainExpansion domain_expansion { get; private set; }

    public Skill_Base[] all_skills { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        sword_throw = GetComponentInChildren<Skill_SwordThrow>();
        time_echo = GetComponentInChildren<Skill_TimeEcho>();
        domain_expansion = GetComponentInChildren<Skill_DomainExpansion>();

        all_skills = GetComponentsInChildren<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in all_skills)
            skill.ReduceCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.SwordThrow: return sword_throw;
            case SkillType.TimeEcho: return time_echo;
            case SkillType.DomainExpansion: return domain_expansion;

            default: 
                Debug.Log($"Skill type {type} is not implemented yet.");
                return null;
        }

    }
}
