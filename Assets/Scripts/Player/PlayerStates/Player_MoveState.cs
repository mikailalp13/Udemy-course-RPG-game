using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.move_input.x == 0 || player.wall_detected)
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.move_input.x * player.move_speed, rb.linearVelocity.y); //this way we keep y velocity unchanged. we cant't type 0 for y.
    }
}
