using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
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
            state_machine.ChangeState(player.fallState);

        if (player.wall_detected)
            state_machine.ChangeState(player.wallSlideState);
    }
}
