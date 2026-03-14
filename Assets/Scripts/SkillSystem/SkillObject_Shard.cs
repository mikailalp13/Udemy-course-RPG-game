using UnityEngine;
using System;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;
    private Skill_Shard shard_manager;

    [SerializeField] private GameObject vfx_prefab;
    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed, Transform new_target = null)
    {
        target = new_target == null ? FindClosestTarget() : new_target;
        this.speed = speed;
    }

    public void SetupShard(Skill_Shard shard_manager)
    {
        this.shard_manager = shard_manager;

        player_stats = shard_manager.player.stats;
        damage_scale_data = shard_manager.damage_scale_data;

        float detonation_time = shard_manager.GetDetonateTime();

        Invoke(nameof(Explode), detonation_time);
    }

    public void SetupShard(Skill_Shard shard_manager, float detonation_time, bool can_move, float shard_speed, Transform target = null)
    {
        this.shard_manager = shard_manager;
        player_stats = shard_manager.player.stats;
        damage_scale_data = shard_manager.damage_scale_data;

        Invoke(nameof(Explode), detonation_time);

        if (can_move)
            MoveTowardsClosestTarget(shard_speed, target);
        
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, check_radius);
        GameObject vfx = Instantiate(vfx_prefab, transform.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = shard_manager.player.vfx.GetElementColor(used_element);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }

}
