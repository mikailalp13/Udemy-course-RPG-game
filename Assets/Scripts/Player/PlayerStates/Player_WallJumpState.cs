using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.wall_jump_force.x * -player.facing_dir, player.wall_jump_force.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);

        if (player.wall_detected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
