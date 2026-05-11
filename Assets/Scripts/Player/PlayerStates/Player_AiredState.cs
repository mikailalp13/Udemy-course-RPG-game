using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
    }


    public override void Update()
    {
        base.Update();

        if (player.move_input.x != 0)
            player.SetVelocity(player.move_input.x * (player.move_speed * player.in_air_move_multiplier), rb.linearVelocity.y);

        if (input.Player.Attack.WasPressedThisFrame())
            state_machine.ChangeState(player.jumpAttackState);
    }
}
