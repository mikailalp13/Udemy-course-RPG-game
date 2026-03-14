using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    private int max_distance;
    private float attacks_per_second;
    private float attack_timer;


    public override void SetupSword(Skill_SwordThrow sword_manager, Vector2 direction)
    {
        base.SetupSword(sword_manager, direction);

        anim?.SetTrigger("spin");

        max_distance = sword_manager.max_distance;
        attacks_per_second = sword_manager.attacks_per_second;

        Invoke(nameof(GetSwordBackToPlayer), sword_manager.max_spin_duration);
    }


    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeBack();
    }

    private void HandleStopping()
    {
        float distance_to_player = Vector2.Distance(transform.position, player_transform.position);

        if (distance_to_player > max_distance && rb.simulated == true)
            rb.simulated = false;
    }

    private void HandleAttack()
    {
        attack_timer -= Time.deltaTime;

        if (attack_timer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attack_timer = 1 / attacks_per_second;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
