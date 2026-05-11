using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
    }


    public override void Update()
    {
        base.Update();

        if (player.ground_detected)
            state_machine.ChangeState(player.idleState);
        //if player detecting the ground below, if yes -> go to idle state

        if (player.wall_detected)
            state_machine.ChangeState(player.wallSlideState);
    }
}
