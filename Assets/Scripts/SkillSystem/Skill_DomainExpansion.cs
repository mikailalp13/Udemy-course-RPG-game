using UnityEngine;
using System.Collections.Generic;
public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domain_prefab;


    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slow_down_percent = 0.8f;
    [SerializeField] private float slow_down_domain_duration = 5f;
    

    [Header("Shard Cast Upgrade")]
    [SerializeField] private int shards_to_cast = 10;
    [SerializeField] private float shard_cast_domain_slow = 1f;
    [SerializeField] private float shard_cast_domain_duration = 8f;
    private float spell_cast_timer;
    private float spells_per_second;

    [Header("Time Echo Cast Upgrade")]
    [SerializeField] private int echo_to_cast = 8;
    [SerializeField] private float echo_cast_domain_slow = 1f;
    [SerializeField] private float echo_cast_domain_duration = 6f;
    [SerializeField] private float health_to_restore_with_echo = 0.05f;


    [Header("Domain Details")]
    public float max_domain_size = 10f;
    public float expand_speed = 3f;


    private List<Enemy> trapped_targets = new List<Enemy>();
    private Transform current_target;


    public void CreateDomain()
    {
        spells_per_second = GetSpellsToCast() / GetDomainDuration();

        GameObject domain = Instantiate(domain_prefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }


    public void DoSpellCasting()
    {
        spell_cast_timer -= Time.deltaTime;

        if (current_target == null)
            current_target = FindTargetInDomain();

        if (current_target != null && spell_cast_timer < 0)
        {
            CastSpell(current_target);
            spell_cast_timer = 1 / spells_per_second;
            current_target = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgrade_type == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skill_manager.time_echo.CreateTimeEcho(target.position + offset);
        }

        if (upgrade_type == SkillUpgradeType.Domain_ShardSpam)
        {
            skill_manager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        trapped_targets.RemoveAll(target => target == null || target.health.is_dead);

        if (trapped_targets.Count == 0)
            return null;
        
        int random_index = Random.Range(0, trapped_targets.Count);
        return trapped_targets[random_index].transform;
    }

    public float GetDomainDuration()
    {
        if (upgrade_type == SkillUpgradeType.Domain_SlowingDown)
            return slow_down_domain_duration;
        else if (upgrade_type == SkillUpgradeType.Domain_ShardSpam)
            return shard_cast_domain_duration;
        else if (upgrade_type == SkillUpgradeType.Domain_EchoSpam)
            return echo_cast_domain_duration;

        return 0;
    }


    public float GetSlowPercentage()
    {
        if (upgrade_type == SkillUpgradeType.Domain_SlowingDown)
            return slow_down_percent;
        else if (upgrade_type == SkillUpgradeType.Domain_ShardSpam)
            return shard_cast_domain_slow;
        else if (upgrade_type == SkillUpgradeType.Domain_EchoSpam)
            return echo_cast_domain_slow;
            
        return 0;
    }


    private int GetSpellsToCast()
    {
        if (upgrade_type == SkillUpgradeType.Domain_ShardSpam)
            return shards_to_cast;
        else if (upgrade_type == SkillUpgradeType.Domain_EchoSpam)
            return echo_to_cast;

        return 0;        
    }


    public bool InstantDomain()
    {
        return upgrade_type != SkillUpgradeType.Domain_EchoSpam
            && upgrade_type != SkillUpgradeType.Domain_ShardSpam; 
    }

    public void AddTarget(Enemy target_to_add)
    {
        trapped_targets.Add(target_to_add);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trapped_targets)
            enemy.StopSlowDown();
        
        trapped_targets = new List<Enemy>();
    }
}
