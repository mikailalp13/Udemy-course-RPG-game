using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);
        //if player detecting the ground below, if yes -> go to idle state

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
