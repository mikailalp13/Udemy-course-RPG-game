using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool touched_ground;

    public Player_JumpAttackState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
    }


    public override void Enter()
    {
        base.Enter();
        touched_ground = false;

        player.SetVelocity(player.jump_attack_velocity.x * player.facing_dir, player.jump_attack_velocity.y);
    }
    

    public override void Update()
    {
        base.Update();

        if (player.ground_detected && touched_ground == false)
        {
            touched_ground = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if (trigger_called && player.ground_detected)
            state_machine.ChangeState(player.idleState);
    }
}
