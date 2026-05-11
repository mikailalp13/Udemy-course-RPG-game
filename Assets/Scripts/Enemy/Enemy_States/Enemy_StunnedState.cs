using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX vfx;


    public Enemy_StunnedState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        vfx = enemy.GetComponent<Enemy_VFX>();
    }


    public override void Enter()
    {
        base.Enter();

        vfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);

        state_timer = enemy.stunned_duration;
        rb.linearVelocity = new Vector2(enemy.stunned_velocity.x * -enemy.facing_dir, enemy.stunned_velocity.y);
    }
    

    public override void Update()
    {
        base.Update();

        if (state_timer < 0)
            state_machine.ChangeState(enemy.idle_state);
    }
}
