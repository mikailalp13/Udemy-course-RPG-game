using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard current_shard;
    private Entity_Health player_health;

    [SerializeField] private GameObject shard_prefab;
    [SerializeField] private float detonate_time = 2f;


    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shard_speed = 7f;


    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int max_charges = 3;
    [SerializeField] private int current_charges;
    [SerializeField] private bool is_recharging;


    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shard_exist_duration = 10;


    [Header("Health Rewind Shard Uograde")]
    [SerializeField] private float saved_health_percent;

    protected override void Awake()
    {
        base.Awake();
        current_charges = max_charges;
        player_health = GetComponentInParent<Entity_Health>();
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;
        
        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_Multicast))
            HandleShardMulticast();
        
        if (Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if (Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }

    private void HandleShardHealthRewind()
    {
        if (current_shard == null)
        {
            CreateShard();
            saved_health_percent = player_health.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            player_health.SetHealthToPercent(saved_health_percent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if (current_shard == null)
            CreateShard();
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shard_position = current_shard.transform.position;
        Vector3 player_position = player.transform.position;

        current_shard.transform.position = player_position;
        current_shard.Explode();

        player.TeleportPlayer(shard_position);
    }

    private void HandleShardMulticast()
    {
        if (current_charges <= 0)
            return;

        CreateShard();
        current_shard.MoveTowardsClosestTarget(shard_speed);
        current_charges--;

        if (is_recharging == false) 
            StartCoroutine(ShardRechargeCo());
    }

    private IEnumerator ShardRechargeCo()
    {
        is_recharging = true;

        while (current_charges < max_charges)
        {
            yield return new WaitForSeconds(cooldown);
            current_charges++;
        }

        is_recharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        current_shard.MoveTowardsClosestTarget(shard_speed);

        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        float detonate_time = GetDetonateTime();

        GameObject shard = Instantiate(shard_prefab, transform.position, Quaternion.identity);
        current_shard = shard.GetComponent<SkillObject_Shard>();
        current_shard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            current_shard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard()
    {
        bool can_move = Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_Multicast);

        GameObject shard = Instantiate(shard_prefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonate_time, can_move, shard_speed);
    }

    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shard_exist_duration;
        
        return detonate_time;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            current_shard.OnExplode -= ForceCooldown;
        }
    }
    
}
