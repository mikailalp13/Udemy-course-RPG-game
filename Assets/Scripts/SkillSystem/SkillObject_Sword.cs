using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow sword_manager;

    protected Transform player_transform;
    protected bool should_come_back;
    protected float come_back_speed = 20f;
    protected float max_allowed_distance = 25f;

    protected virtual void Update()
    {
        transform.right = rb.linearVelocity;
        HandleComeBack();
    }

    public virtual void SetupSword(Skill_SwordThrow sword_manager, Vector2 direction)
    {
        rb.linearVelocity = direction;

        this.sword_manager = sword_manager;

        player_transform = sword_manager.transform.root;
        player_stats = sword_manager.player.stats;
        damage_scale_data = sword_manager.damage_scale_data;
    }

    public void GetSwordBackToPlayer() => should_come_back = true;   

    protected void HandleComeBack()
    {
        float distance = Vector2.Distance(transform.position, player_transform.position);

        if (distance > max_allowed_distance)
            GetSwordBackToPlayer();

        if (should_come_back == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, player_transform.position, come_back_speed * Time.deltaTime);

        if (distance < 0.5f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesInRadius(transform, 1);
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }

}
