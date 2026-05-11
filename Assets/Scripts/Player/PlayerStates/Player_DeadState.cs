using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
    }

    
    public override void Enter()
    {
        base.Enter();
        input.Disable();

        rb.simulated = false;
    }
}
