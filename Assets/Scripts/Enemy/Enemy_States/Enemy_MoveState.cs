using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
    }


    public override void Enter()
    {
        base.Enter();
        if (enemy.ground_detected == false || enemy.wall_detected)
            enemy.Flip();
    }


    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.GetMoveSpeed() * enemy.facing_dir, rb.linearVelocity.y);

        if (enemy.ground_detected == false || enemy.wall_detected)
            state_machine.ChangeState(enemy.idle_state);
    }
}
