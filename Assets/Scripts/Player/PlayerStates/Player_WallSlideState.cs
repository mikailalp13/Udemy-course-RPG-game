using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
    }
    

    public override void Update()
    {
        base.Update();
        HandleWallSlide();


        if (input.Player.Jump.WasPressedThisFrame())
            state_machine.ChangeState(player.wallJumpState);

        if (player.wall_detected == false)
            state_machine.ChangeState(player.fallState);

        if (player.ground_detected)
        {
            state_machine.ChangeState(player.idleState);

            if(player.facing_dir != player.move_input.x)
                player.Flip();
        }
    }


    private void HandleWallSlide()
    {
        if (player.move_input.y < 0)
            player.SetVelocity(player.move_input.x, rb.linearVelocity.y);
        else
            player.SetVelocity(player.move_input.x, rb.linearVelocity.y * player.wall_slide_slow_multiplier);
    }
}
