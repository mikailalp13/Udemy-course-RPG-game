using UnityEngine;

public class Player_JumpAttackState : PlayerState
{
    private bool touched_ground;
    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (triggerCalled && player.ground_detected)
            stateMachine.ChangeState(player.idleState);

    }

}
