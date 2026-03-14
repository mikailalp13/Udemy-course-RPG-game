using UnityEngine;

public class Player_DashState : PlayerState
{
    private float original_gravity_scale;
    private int dash_dir;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skill_manager.dash.OnStartEffect();
        player.vfx.DoImageEchoEffect(player.dash_duration);

        dash_dir = player.move_input.x != 0 ? ((int)player.move_input.x) : player.facing_dir;
        state_timer = player.dash_duration;

        original_gravity_scale = rb.gravityScale;
        rb.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
    }


    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dash_speed * dash_dir, 0);

        if (state_timer < 0)
        {
            if (player.ground_detected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        skill_manager.dash.OnEndEffect();

        player.health.SetCanTakeDamage(true);
        player.SetVelocity(0, 0);
        rb.gravityScale = original_gravity_scale;
    }

    private void CancelDashIfNeeded()
    {
        if (player.wall_detected)
        {
            if (player.ground_detected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }

    }
}
