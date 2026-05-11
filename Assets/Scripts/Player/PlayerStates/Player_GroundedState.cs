using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
    }

    
    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && player.ground_detected == false)
            state_machine.ChangeState(player.fallState);

        if (input.Player.Jump.WasPressedThisFrame())
            state_machine.ChangeState(player.jumpState);

        if (input.Player.Attack.WasPressedThisFrame())
            state_machine.ChangeState(player.basicAttackState);

        if (input.Player.CounterAttack.WasPressedThisFrame())
            state_machine.ChangeState(player.counterAttackState);
        
        if (input.Player.RangeAttack.WasPressedThisFrame() && skill_manager.sword_throw.CanUseSkill())
            state_machine.ChangeState(player.swordThrowState);
    }
}
