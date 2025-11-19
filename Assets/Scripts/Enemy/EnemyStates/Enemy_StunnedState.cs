using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX vfx;
    public Enemy_StunnedState(Enemy enemy, StateMachine state_machine, string animBoolName) : base(enemy, state_machine, animBoolName)
    {
        vfx = enemy.GetComponent<Enemy_VFX>();
    }

    public override void Enter()
    {
        base.Enter();

        vfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);

        stateTimer = enemy.stunned_duration;
        rb.linearVelocity = new Vector2(enemy.stunned_velocity.x * -enemy.facing_dir, enemy.stunned_velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
