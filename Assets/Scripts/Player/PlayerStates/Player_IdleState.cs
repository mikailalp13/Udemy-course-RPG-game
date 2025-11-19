using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();

        if (player.move_input.x == player.facing_dir && player.wall_detected)
            return;

        if (player.move_input.x != 0)
                stateMachine.ChangeState(player.moveState);

    }    
}
