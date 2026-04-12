using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
        stats = enemy.stats;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        float battle_anim_speed_multiplier = enemy.battle_move_speed / enemy.move_speed;
        anim.SetFloat("battleAnimSpeedMultiplier", battle_anim_speed_multiplier);
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.move_anim_speed_multiplier);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }
}
