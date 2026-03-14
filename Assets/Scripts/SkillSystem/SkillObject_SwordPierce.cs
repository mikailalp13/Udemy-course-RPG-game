using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int amount_to_pierce;

    public override void SetupSword(Skill_SwordThrow sword_manager, Vector2 direction)
    {
        base.SetupSword(sword_manager,direction);

        amount_to_pierce = sword_manager.amount_to_pierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool ground_hit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (amount_to_pierce <= 0 || ground_hit)
        {
            DamageEnemiesInRadius(transform, 0.3f);
            StopSword(collision);
            return;
        }
        amount_to_pierce--;
        DamageEnemiesInRadius(transform, 0.3f);
    }
}
