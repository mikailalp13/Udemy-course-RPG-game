using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.ground_detected)
            stateMachine.ChangeState(player.idleState);
        //if player detecting the ground below, if yes -> go to idle state

        if (player.wall_detected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
